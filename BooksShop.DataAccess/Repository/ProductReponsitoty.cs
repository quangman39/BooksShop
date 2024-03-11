using BooksShop.DataAccess.Data;
using BooksShop.DataAccess.Repository.IReponsitory;
using BooksShop.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShop.DataAccess.Repository
{
    public class ProductReponsitoty : Reponsitory<Product>, IProductReponsitory
    {
        private ApplicationDbContext _db;
        public ProductReponsitoty(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }
       
       
        public void Update(Product obj)
        {
           _db.products.Update(obj);
        }
    }
}
