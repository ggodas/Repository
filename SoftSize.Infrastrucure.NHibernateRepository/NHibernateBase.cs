﻿using System;
using NHibernate;

namespace SoftSize.Infrastrucure.NHibernateRepository
{
    public abstract class NHibernateBase
    {
        protected readonly ISessionFactory _sessionFactory;
        protected virtual ISession session
        {
            get
            {
                return _sessionFactory.GetCurrentSession();
            }
        }

        public NHibernateBase(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        protected virtual TResult Transact<TResult>(Func<TResult> func)
        {
            if (!session.Transaction.IsActive)
            {
                // Wrap in transaction
                TResult result;
                using (var tx = session.BeginTransaction())
                {
                    result = func.Invoke();
                    tx.Commit();
                }
                return result;
            }
            // Don't wrap;
            return func.Invoke();
        }

        protected virtual void Transact(Action action)
        {
            Transact(() =>
            {
                action.Invoke();
                return false;
            });
        }
    }
}
