using System.Web.Configuration;
using System.Web.Mvc;
using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using SmartArch.Data;
using SmartArch.Data.NH;
using KitchIn.BL.Implementation;
using KitchIn.Core.Interfaces;
using KitchIn.Data.NHibernate;
using SmartArch.Web.Membership;

namespace KitchIn.ServiceLocation
{
    /// <summary>
    /// The WCF initializer
    /// </summary>
    public class WcfNHibernateInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            new DbInitializer(WebConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString).Initialize();

            container.Register(Component.For<ISessionFactory>().Instance(DbInitializer.Factory).LifeStyle.Singleton);
            container.Register(Component.For<ISession>().UsingFactoryMethod(x => x.Resolve<ISessionFactory>().OpenSession()).LifestylePerWebRequest());

            container.Register(Component.For(typeof(IRepository<>)).ImplementedBy(typeof(Repository<>)).LifeStyle.Transient);
            container.Register(Classes.FromThisAssembly().BasedOn<IController>().LifestyleTransient());
            container.Register(Component.For<ITransactionManager>().ImplementedBy<TransactionManager>().LifestylePerWcfOperation());
            container.AddFacility<WcfFacility>().Register(Component.For<IMembershipProvider>().ImplementedBy<MembershipProvider>());
            container.Register(Component.For<IProvider>().ImplementedBy<BaseProvider>());
            container.Register(Component.For<IManageUserProvider>().ImplementedBy<ManageUserProvider>());
            container.Register(Component.For<IManageProductProvider>().ImplementedBy<ManageProductProvider>());
            container.Register(Component.For<IManageKitchenProvider>().ImplementedBy<ManageKitchenProvider>());
            container.Register(Component.For<IManageFavoritesProvider>().ImplementedBy<ManageFavoritesProvider>());
            container.Register(Component.For<IAuthenticationService>().ImplementedBy<AuthenticationService>().LifestylePerWcfOperation());
            container.Register(
                AllTypes
                 .FromAssembly(typeof(IAuthenticationService).Assembly)
                 .Pick()
                 .WithService.DefaultInterfaces()
                 .Configure(m => m.LifestylePerWcfOperation()));
            
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }
    }
}