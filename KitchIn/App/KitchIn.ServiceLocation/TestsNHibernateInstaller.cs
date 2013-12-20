using System.Configuration;

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using NHibernate;

using SmartArch.Data;
using SmartArch.Data.NH;
using KitchIn.Data.NHibernate;

namespace KitchIn.ServiceLocation
{
    public class TestsNHibernateInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            new DbInitializer(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);

            container.Register(Component.For<ITransactionManager>().ImplementedBy<TransactionManager>().LifeStyle.Transient);
            container.Register(Component.For<ISessionFactory>().Instance(DbInitializer.Factory).LifestyleTransient());
            container.Register(Component.For<ISession>().UsingFactoryMethod(x => x.Resolve<ISessionFactory>().OpenSession()).LifeStyle.Transient);

            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }
    }
}