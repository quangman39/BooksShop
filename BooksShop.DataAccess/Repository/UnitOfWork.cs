using BooksShop.DataAccess.Data;
using BooksShop.DataAccess.Repository.IReponsitory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShop.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryReponsitory Category {  get;private set; }
        public IProductReponsitory Product {  get;private set; }
        public ICompanyReponsitory Company {  get;private set; }
        public IShoppingCartReponsitory ShoppingCart { get;private set; }
        public IApplicationUserReponsitory ApplicationUser { get;private set; } 

        public UnitOfWork(ApplicationDbContext db)
        { 
            _db = db;
            Category = new CategoryReponsitory(_db);
            Product = new ProductReponsitoty(_db);
            Company = new CompanyReponsitory(_db); 
            ShoppingCart = new ShoppingCartReponitory(_db);
            ApplicationUser = new ApplicationUSerReponsitory(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
