﻿using BooksShop.DataAccess.Repository.IReponsitory;
using BooksShop.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BooksShop.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<OrderHeader> obj = _unitOfWork.OrderHeader.GetAll(includeProperties:"ApplicationUser").ToList() ;
            return View(obj);
        }

      
    }
}