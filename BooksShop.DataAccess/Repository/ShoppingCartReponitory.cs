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
    public class ShoppingCartReponitory : Reponsitory<ShoppingCart>, IShoppingCartReponsitory
    {
        private readonly ApplicationDbContext _db;

        public ShoppingCartReponitory(ApplicationDbContext db):base(db)       
        {
            _db = db;
        }

        public void Update (ShoppingCart obj)
        {
            _db.ShoppingCarts.Update(obj);
        }


    }
}
