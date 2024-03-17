using BooksShop.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShop.DataAccess.Repository.IReponsitory
{
    public interface IShoppingCartReponsitory : IReponsitory <ShoppingCart>
    {
        void Update(ShoppingCart obj);
    }
}
