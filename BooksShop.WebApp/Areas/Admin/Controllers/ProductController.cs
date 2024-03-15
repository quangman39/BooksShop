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

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnviroment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnviroment = webHostEnviroment;
        }

        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Product.GetAll("category").ToList();
           
            return View(products);
        }

        public IActionResult UpSert(int? id)// combine update and insert
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

            if (id == null || id == 0)
            {
            
                return View(obj);
            }
            else
            {
                //update
                obj.Product = _unitOfWork.Product.Get(temp => temp.Id == id, "category");
                if(obj.Product == null) return NotFound();
                return View(obj);

            }

          
        }

        [HttpPost]
        public IActionResult UpSert(ProductVM obj, IFormFile? file)//Upsert
        {
           if(ModelState.IsValid)
            {
                string wwRootPath = _webHostEnviroment.WebRootPath;
                if (file != null) { 
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string  productPath = Path.Combine(wwRootPath, @"image\product");

                    if(!string.IsNullOrEmpty(obj.Product.image))
                    {
                        string oldImagePath = Path.Combine(wwRootPath, obj.Product.image.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using( var fileStream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                   obj.Product.image = @"\image\product\" + fileName;
                }


                if(obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);

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
