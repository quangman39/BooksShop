using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShop.DataAccess.Repository.IReponsitory
{
    public interface IUnitOfWork
    {
        ICategoryReponsitory Category { get; }
        IProductReponsitory Product { get; }
        void Save();
    }
}
