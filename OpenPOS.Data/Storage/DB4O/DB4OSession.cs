using System;
using System.Collections.Generic;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

namespace OpenPOS.Data.Storage.DB4O
{

    public class Db4oSession : ISession
    {
        private readonly IObjectContainer _db;

        public Db4oSession(INoSqlServer server)
        {
            _db = server.DB;

        }

        public IQueryable<T> All<T>() where T : class
        {
            return (from T items in _db
                    select items).AsQueryable();
        }

        public T Single<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class
        {
            return All<T>().SingleOrDefault(expression);
        }

        public void Add<T>(T item) where T : class
        {
            _db.Store(item);
        }

        public void Add<T>(IEnumerable<T> items) where T : class
        {
            foreach (var item in items)
            {
                _db.Store(item);
            }
        }

        /// <summary>
        /// Updates an item in the database
        /// </summary>
        /// <param name="item"></param>
        public void Update<T>(T item) where T : class
        {
            _db.Store(item);
        }


        /// <summary>
        /// Deletes an item from the database
        /// </summary>
        /// <param name="item"></param>
        public void Delete<T>(T item) where T : class
        {
            _db.Delete(item);
        }

        /// <summary>
        /// Deletes subset of objects
        /// </summary>
        public void Delete<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class
        {
            var items = All<T>().Where(expression).ToList();
            items.ForEach(x => _db.Delete(x));
        }

        /// <summary>
        /// Deletes all T objects
        /// </summary>
        public void DeleteAll<T>() where T : class
        {
            var items = All<T>().ToList();
            items.ForEach(x => _db.Delete(x));
        }


        /// <summary>
        /// Commits changes to disk
        /// </summary>
        public void CommitChanges()
        {
            //commit the changes
            _db.Commit();

        }
        public void Dispose()
        {
            //explicitly close
            _db.Close();
            //dispose 'em
            _db.Dispose();
        }
    }
}