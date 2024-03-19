using BooksShop.DataAccess.Repository.IReponsitory;
using BooksShop.Models.Models;
using BooksShop.Models.Models.ViewsModels;
using BooksShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace BooksShop.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        
        public IActionResult Index()
        {
            IEnumerable<OrderHeader> objorderHeader;
           
            if(User.IsInRole(SD.Role_Admin)||User.IsInRole(SD.Role_Employee))
            {
                objorderHeader = _unitOfWork.OrderHeader.GetAll(includeProperties:"ApplicationUser") ;
            }
            else
            {
                var claimIndentity = (ClaimsIdentity)User.Identity;
                var userId = claimIndentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                objorderHeader = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == userId,includeProperties: "ApplicationUser");
            }

            return View(objorderHeader);
        }

        public IActionResult Detail(int orderId)
        {
            OrderVM = new()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(u => u.OrderHeaderId == orderId, includeProperties: "Product"),
            };
            return View(OrderVM);
        }

        [HttpPost]
        [Authorize(Roles =SD.Role_Admin+","+SD.Role_Employee)]
        public IActionResult UpdateOrderDetail()
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeaderFromDb.Name = OrderVM.OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.Address = OrderVM.OrderHeader.Address;
            orderHeaderFromDb.City = OrderVM.OrderHeader.City;
            orderHeaderFromDb.State = OrderVM.OrderHeader.State;
            orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;

            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();
            return RedirectToAction("Index");

        }

        [HttpPost]
        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusInProcess);
            _unitOfWork.Save();
            TempData["success"] = "OrderDetails is in Process";
            return RedirectToAction("Detail",new {orderId = OrderVM.OrderHeader.Id});
        }

        [HttpPost]
        [Authorize(Roles =SD.Role_Admin+","+SD.Role_Employee)]
        public IActionResult ShipOrder()
        {

            var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u=>u.Id == OrderVM.OrderHeader.Id);
            orderHeaderFromDb.OrderStatus = SD.StatusShipped;
            orderHeaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderHeaderFromDb.ShipingDate = DateTime.Now;
            orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
            if (orderHeaderFromDb.PaymentStatus == SD.PaymentSatusDelaydPayment)
            {
                orderHeaderFromDb.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
            }

            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();
            TempData["success"] = "OrderDetails is Shipped";
            return RedirectToAction("Detail", new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult CancelOrder()
        {
            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);
            if(orderHeader.PaymentStatus  == SD.PaymentSatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId,
                };

                var service = new RefundService();
                Refund refund = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusRefunded);
            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusCancelled);
            }
            _unitOfWork.Save();
            TempData["success"] = "cancelled successfully";
            return RedirectToAction("Detail", new {orderId =  orderHeader.Id});
        }


        [ActionName("Detail")]
        [HttpPost]
        public IActionResult Details_PAY_Now()
        {
            OrderVM.OrderHeader = _unitOfWork.OrderHeader.Get(u =>u.Id == OrderVM.OrderHeader.Id, "ApplicationUser");
            OrderVM.OrderDetails = _unitOfWork.OrderDetails.GetAll(u => u.OrderHeader.Id == OrderVM.OrderHeader.Id, "Product");

            var domain = Request.Scheme + "://" + Request.Host.Value + "/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"admin/order/PaymentConfirm?id={OrderVM.OrderHeader.Id}",
                CancelUrl = domain + $"admin/order/detail?orderId={OrderVM.OrderHeader.Id}",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (var item in OrderVM.OrderDetails)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100), // $20.50 => 2050
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.OrderHeader.UpdateStripePaymentID(OrderVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }

        
        public IActionResult PaymentConfirm(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(temp => temp.Id == id, includeProperties: "ApplicationUser");
            if (orderHeader.PaymentStatus == SD.PaymentSatusDelaydPayment)
            { //this is an order by company 
                var service = new SessionService();
                Session session = service.Get(orderHeader.SesstionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(id, orderHeader.OrderStatus, SD.PaymentSatusApproved);
                    _unitOfWork.Save();
                }
            }
            return View(orderHeader.Id);
        }

    }
}
