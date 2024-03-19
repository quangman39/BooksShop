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
    public class OrderHeaderReponsitory : Reponsitory<OrderHeader>, IOrderHeaderReponsitory
    {
        private ApplicationDbContext _db;
        public OrderHeaderReponsitory(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }


        public void Update(OrderHeader obj)
        {
            _db.OrderHeader.Update(obj);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
          var orderFromDb = _db.OrderHeader.FirstOrDefault(u => u.Id == id);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if(paymentStatus != null)
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }

        }

        public void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId)
        {

            var orderFromDb = _db.OrderHeader.FirstOrDefault(u => u.Id == id);
            
            if(!string.IsNullOrEmpty(sessionId))
            {
                orderFromDb.SesstionId = sessionId;
            }
            if(!string.IsNullOrEmpty(paymentIntentId))
            {
                orderFromDb.PaymentIntentId = paymentIntentId;
                orderFromDb.PaymentDate = DateTime.Now; 
            }
        }

    }
}
