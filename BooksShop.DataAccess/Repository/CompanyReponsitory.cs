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
    public class CompanyReponsitory : Reponsitory<Company>, ICompanyReponsitory
    {
        private ApplicationDbContext _db;
        public CompanyReponsitory(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }


        public void Update(Company obj)
        {
            _db.Companies.Update(obj);
        }

    }
}
