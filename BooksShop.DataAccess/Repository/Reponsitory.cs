using BooksShop.DataAccess.Data;
using BooksShop.DataAccess.Repository.IReponsitory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BooksShop.DataAccess.Repository
{
    public class Reponsitory<T> : IReponsitory<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Reponsitory(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();

        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();

        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query;
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
