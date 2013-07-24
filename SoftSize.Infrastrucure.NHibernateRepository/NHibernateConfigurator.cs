using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;

namespace SoftSize.Infrastrucure.NHibernateRepository
{

    public static class NHConfigurator<T>
    {
        private static readonly Configuration configuration;
        private static readonly ISessionFactory sessionFactory;

        static NHConfigurator()
        {


            configuration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2005
                              .CurrentSessionContext("thread_static")
                              .ConnectionString(
                                  c => c
                                           .FromConnectionStringWithKey("ApplicationServices"))
                              .ShowSql()
                )
                .Mappings(m => m.
                //FluentMappings.AddFromAssembly(Assembly.Load("CTF.Fidelidade.Premmia.Data.Maps"))) 
                FluentMappings.AddFromAssemblyOf<T>())
                .BuildConfiguration();

            sessionFactory = configuration.BuildSessionFactory();
        }

        public static Configuration Configuration
        {
            get
            {
                return configuration;
            }
        }



        public static ISessionFactory SessionFactory
        {
            get
            {
                return sessionFactory;
            }
        }
    }
}
