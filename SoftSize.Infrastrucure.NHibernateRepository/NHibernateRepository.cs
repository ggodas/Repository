using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Media;
using LinqSpecs;
using NHibernate;
using NHibernate.Linq;
using SoftSize.Infrastructure;

namespace SoftSize.Infrastrucure.NHibernateRepository
{
    public class NHibernateRepository<T, TId> : NHibernateBase, IRepository<T, TId> where T : Entity<TId>, IAggregateRoot
    {

        public NHibernateRepository(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }

        public void Add(T item)
        {
            Transact(() => session.Save(item));
        }

        public bool Contains(T item)
        {
            var id = default(TId);
            if (item.Id.Equals(id))
                return false;
            return Transact(() => session.Get<T>(item.Id)) != null;
        }

        public long Count
        {
            get
            {
                return Transact(() => session.Query<T>().Count());
            }
        }

        public bool Remove(T item)
        {
            session.Clear();
            Transact(() => session.Delete(item));
            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Transact(() => session.Query<T>()
                .Take(1000).GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Transact(() => GetEnumerator());
        }


        public IEnumerable<T> FindAll(Specification<T> specification)
        {
            var query = GetQuery(specification);
            return Transact(() => query.ToList());
        }

        public T FindOne(Specification<T> specification)
        {
            var query = GetQuery(specification);
            return Transact(() => query.FirstOrDefault());
        }

        private IQueryable<T> GetQuery(Specification<T> specification)
        {
            return session.Query<T>()
            .Where(specification.IsSatisfiedBy());
        }


        public IEnumerable<T> FindAll(Expression<Func<T, bool>> function)
        {
            return session.QueryOver<T>().Where(function).List();
        }


        public bool RemoveAll(Func<T, bool> function)
        {
            throw new NotImplementedException();
        }
    }

}
