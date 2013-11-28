using System;
using NHibernate;

namespace SoftSize.Infrastructure.MVC
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class NeedsPersistenseAttribute : NHibernateSessionFactor
    {
        private bool transactional;
        public NeedsPersistenseAttribute(bool transactional = false)
        {
            this.transactional = transactional;
        }
        protected ISession session
        {
            get
            {
                return SessionFactory.GetCurrentSession();
            }
        }
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            if (transactional)
                session.BeginTransaction();
        }
        public override void OnActionExecuted(System.Web.Http.Filters.HttpActionExecutedContext actionExecutedContext)
        {
            if (transactional)
            {
                var tx = session.Transaction;
                if (actionExecutedContext.Exception == null && tx != null && tx.IsActive)
                    try
                    {
                        session.Transaction.Commit();
                    }
                    catch
                    {
                        session.Transaction.Rollback();
                        throw;
                    }
                else
                    session.Transaction.Rollback();

            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
