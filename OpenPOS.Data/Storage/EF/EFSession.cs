using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Objects;
using OpenPOS.Data.Models;
using System.Data.Entity.Infrastructure;

namespace OpenPOS.Data.Storage.EF
{
    public class EFSession : ISession
    {
        DbContext _context;

        public EFSession(DbContext context)
        {
            _context = context;
        }

        public void CommitChanges()
        {
            _context.SaveChanges();
        }

        public void Delete<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class
        {
            foreach (var item in _context.Set<T>().Where(expression))
                Delete<T>(item);
        }

        public void Delete<T>(T item) where T : class
        {
            _context.Set<T>().Remove(item);
        }

        public void DeleteAll<T>() where T : class
        {
            foreach (var item in _context.Set<T>().AsQueryable())
                Delete<T>(item);
        }

        public T Single<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class
        {
            return _context.Set<T>().SingleOrDefault(expression);
        }

        public IQueryable<T> All<T>() where T : class
        {
            return _context.Set<T>().AsQueryable();
        }

        public void Add<T>(T item) where T : class
        {
            _context.Set<T>().Add(item);
        }

        public void Add<T>(IEnumerable<T> items) where T : class
        {
            foreach (var item in items)
                Add<T>(item);
        }

        public void Update<T>(T item) where T : class
        {
            //nothing needed?
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
