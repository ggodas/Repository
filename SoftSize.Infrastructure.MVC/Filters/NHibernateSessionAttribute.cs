using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Context;


namespace SoftSize.Infrastructure.MVC
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class NHibernateSessionFactor : System.Web.Http.Filters.ActionFilterAttribute
    {
        ISessionFactory sessionFactory;

        protected ISessionFactory SessionFactory
        {
            get
            {
                return sessionFactory;
            }
        }

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var session = sessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
        }

        public override void OnActionExecuted(System.Web.Http.Filters.HttpActionExecutedContext actionExecutedContext)
        {
            var session = CurrentSessionContext.Unbind(sessionFactory);
            session.Close();
        }
    }
}