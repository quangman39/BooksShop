using Microsoft.AspNetCore.Mvc;

namespace BooksShop.WebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {

           
            return View();
        }
    }
}
