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
    public class OrderDetailsReponsitory : Reponsitory<OrderDetails>, IOrderDetailsReponsitory
    {
        private ApplicationDbContext _db;
        public OrderDetailsReponsitory(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }


        public void Update(OrderDetails obj)
        {
            _db.OrderDetails.Update(obj);
        }

    }
}
