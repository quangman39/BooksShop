using BooksShop.DataAccess.Repository.IReponsitory;
using BooksShop.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BooksShop.WebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll("category");
            return View(products);
        }
        public IActionResult Details(int id)
        {
            Product product = _unitOfWork.Product.Get(temp => temp.Id==id, "category");
            return View(product);
        }
    }
}
