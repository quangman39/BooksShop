using BooksShop.DataAccess.Data;
using BooksShop.DataAccess.Repository.IReponsitory;
using BooksShop.Models.Models;
using BooksShop.Models.Models.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace BooksShop.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Product.GetAll().ToList();
           
            return View(products);
        }

        public IActionResult Create()
        {
            ProductVM obj = new()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(
               u => new SelectListItem
               {
                   Text = u.Name,
                   Value = u.Id.ToString()
               })
            };
           
           // ViewBag.CategoryList = CategoryList;
            return View(obj);
        }

        [HttpPost]
        public IActionResult Create(ProductVM obj)
        {
           if(ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save();
                TempData["success"] = "Created successfull";

                return RedirectToAction("Index");
            }
            else
            {
                // Extract error messages
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors)
                                                     .Select(e => e.ErrorMessage)
                                                     .ToList();

                // Return error response to client
                return BadRequest(errorMessages); // You can return error messages to client in a suitable format
            }



        }

        public IActionResult Edit(int id )
        {

            Product obj = _unitOfWork.Product.Get(temp => temp.Id == id);
            return View(obj);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.Product.Update(product);
                _unitOfWork.Save();
                TempData["edit"] = "Edit successfull";

                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int id)
        {
            if (id == 0) throw new ArgumentException(nameof(Product.Id));
            Product product = _unitOfWork.Product.Get(temp => temp.Id == id);
            if(product == null) return NotFound();

            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            TempData["deleted"] = "Deleted successfull";

            return RedirectToAction("Index");
        }
    }
}
