﻿using BooksShop.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShop.DataAccess.Repository.IReponsitory
{
    public interface ICompanyReponsitory : IReponsitory<Company>
    {
        void Update(Company obj);
       
    }
}
