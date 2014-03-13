using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using LinqSpecs;

namespace SoftSize.Infrastructure
{
    public interface IRepository<T, TId> : IEnumerable<T> where T : Entity<TId>, IAggregateRoot
    {
        void Add(T item);
        bool Contains(T item);
        long Count { get; }
        bool Remove(T item);
        bool RemoveAll(Func<T, bool> function);

        IEnumerable<T> FindAll(Specification<T> specification);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> function);
        T FindOne(Specification<T> specification);
    }
 }
