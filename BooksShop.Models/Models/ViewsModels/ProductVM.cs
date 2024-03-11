using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BooksShop.Models.Models.ViewsModels
{
    public class ProductVM
    {
        public Product  Product { get; set; }
        public IEnumerable<SelectListItem> CategoryList {  get; set; }
    }
}
