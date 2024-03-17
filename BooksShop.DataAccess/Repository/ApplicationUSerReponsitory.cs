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
    public class ApplicationUSerReponsitory : Reponsitory<ApplicationUser>, IApplicationUserReponsitory
    {
        private ApplicationDbContext _db;
        public ApplicationUSerReponsitory(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }

    }
}
