using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using LinqSpecs;
using System.Collections;
using System.Linq.Expressions;

namespace SoftSize.Infrastructure.MongoDbRepository
{
    public class MongoDbRepository<T, TId> : IRepository<T, TId> where T : Entity<TId>, IAggregateRoot
    {
        private MongoCollection<T> collection;
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

        public MongoDbRepository()
        {
            //MongoDbRepository(ConnectionString, DatabaseName);
        }

        public MongoDbRepository(string connectionString, string databaseName)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;

            var server = MongoServer.Create(connectionString);

            var database = server.GetDatabase(databaseName);
            collection = database.GetCollection<T>(typeof(T).Name);
        }


        public void Add(T item)
        {
            collection.Save<T>(item);
        }

        public bool Contains(T item)
        {
            return collection
                    .AsQueryable<T>().Any(k => k.Id.Equals(item.Id));
        }

        public long Count
        {
            get { return collection.Count(); }
        }

        public bool Remove(T item)
        {
            this.collection
                        .Remove(
                                Query.EQ("_id", new ObjectId(item.Id.ToString())));
            return true;
        }

        public bool RemoveAll(Func<T, bool> function)
        {
            //MongoDB.Driver.Builders.QueryBuilder<T> query = new MongoDB.Driver.Builders.QueryBuilder<T>();
            //System.Linq.Expressions.Expression<T>.Call( Expression<Func<T, bool>> functionFinal = function;
            //var finalquery = query.All<T>(functionFinal);
            //this.collection.Remove(finalquery);
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> function)
        {
            return collection
                    .AsQueryable<T>()
                    .Where(function);
        }


        public IEnumerable<T> FindAll(Specification<T> specification)
        {
            return collection
                    .AsQueryable<T>()
                    .Where(specification.IsSatisfiedBy());
        }

        public T FindOne(Specification<T> specification)
        {
            return collection
                    .AsQueryable<T>()
                    .Where(specification.IsSatisfiedBy())
                    .FirstOrDefault();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return collection
                    .FindAll()
                    .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
