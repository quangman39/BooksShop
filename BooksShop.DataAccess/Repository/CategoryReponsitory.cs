using BooksShop.DataAccess.Repository.IReponsitory;
using BooksShop.Models.Models;
using BooksShop.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShop.DataAccess.Repository
{
    public class CategoryReponsitory : Reponsitory<Category>, ICategoryReponsitory
    {
        private ApplicationDbContext _db;
        public CategoryReponsitory(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }


        public void Update(Category obj)
        {
            _db.categories.Update(obj);
        }

    }
}
