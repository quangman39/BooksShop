using BooksShop.DataAccess.Repository;
using BooksShop.DataAccess.Repository.IReponsitory;
using BooksShop.Models.Models;
using BooksShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BooksShop.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> categories = _unitOfWork.Category.GetAll().ToList();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public  IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "create successfully ";
            }
            return RedirectToAction("Index");
        }


      
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                throw new ArgumentNullException("id");
            }
            Category? category = _unitOfWork.Category.Get(temp => temp.Id == id);
            if(category == null)
            {
               return NotFound();
            }

            return View(category);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Category? category)
        {
            if (category == null)
            {
                throw new ArgumentNullException();
            }

            if(ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
                TempData["edit"] = "edit successfully ";

                return RedirectToAction("Index");   
            }

            return View();
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id ==0)
            {
                throw new ArgumentNullException();
            }

            Category? obj = _unitOfWork.Category.Get(temp => temp.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();

            TempData["deleted"] = "deleted successfully ";


            return RedirectToAction("Index");
        }

    }
}
