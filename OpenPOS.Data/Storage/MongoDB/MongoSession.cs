using System;
using System.Collections.Generic;
using System.Linq;
using Norm;
using Norm.Linq;

namespace OpenPOS.Data.Storage.MongoDB
{
    public class MongoSession : ISession
    {
        private readonly MongoQueryProvider _provider;

        public MongoSession(string db)
        {
            _provider = new MongoQueryProvider(db);
        }

        public void CommitChanges()
        {
        }

        public void Delete<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class
        {
            var items = All<T>().Where(expression);
            foreach (var item in items)
            {
                Delete(item);
            }
        }

        public void Delete<T>(T item) where T : class
        {
            _provider.DB.GetCollection<T>().Delete(item);
        }

        public void DeleteAll<T>() where T : class
        {
            _provider.DB.DropCollection(typeof(T).Name);

        }

        public T Single<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class
        {
            return new MongoQuery<T>(_provider).Where(expression).SingleOrDefault();
        }

        public IQueryable<T> All<T>() where T : class
        {
            return new MongoQuery<T>(_provider);
        }

        public void Add<T>(T item) where T : class
        {
            _provider.DB.GetCollection<T>().Insert(item);
        }

        public void Add<T>(IEnumerable<T> items) where T : class
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void Update<T>(T item) where T : class
        {
            Guid id = ((dynamic)item).Id;
            _provider.DB.GetCollection<T>().UpdateOne(new { _id = id }, item);
        }

        //this is just some sugar if you need it.
        public T MapReduce<T>(string map, string reduce)
        {
            var result = default(T);
            using (var mr = _provider.Server.CreateMapReduce())
            {
                var response =
                    mr.Execute(new MapReduceOptions(typeof(T).Name)
                    {
                        Map = map,
                        Reduce = reduce
                    });
                var coll = response.GetCollection<MapReduceResult<T>>();
                var r = coll.Find().FirstOrDefault();
                if (r != null) result = r.Value;
            }
            return result;
        }

        public void Dispose()
        {
            _provider.Server.Dispose();
        }


    }
}