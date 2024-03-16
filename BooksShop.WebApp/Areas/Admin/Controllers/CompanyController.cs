
using BooksShop.DataAccess.Data;
using BooksShop.DataAccess.Repository.IReponsitory;
using BooksShop.Models.Models;
using BooksShop.Models.Models.ViewsModels;
using BooksShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace BooksShop.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Company> Companys = _unitOfWork.Company.GetAll().ToList();

            return View(Companys);
        }

        public IActionResult UpSert(int? id)// combine update and insert
        {   
            Company obj = new Company();
            if (id == null) return View(obj);
            //update
             obj = _unitOfWork.Company.Get(temp => temp.Id == id);
            if (obj == null) return NotFound();
            return View(obj);




        }

        [HttpPost]
        public IActionResult UpSert(Company obj)//Upsert
        {
            if (ModelState.IsValid)
            {

                if (obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj);
                }
                else
                {
                    _unitOfWork.Company.Update(obj);

                }

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


        public IActionResult Delete(int id)
        {
            if (id == 0) throw new ArgumentNullException("id cant be null");
            Company Company = _unitOfWork.Company.Get(temp => temp.Id == id);
            if (Company == null) return NotFound();

            _unitOfWork.Company.Remove(Company);
            _unitOfWork.Save();
            TempData["deleted"] = "Deleted successfull";

            return RedirectToAction("Index");
        }
    }
}
