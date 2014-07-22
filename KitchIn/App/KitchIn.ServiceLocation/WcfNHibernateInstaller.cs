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
    using KitchIn.Core.Services.Cache;

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

            container.Register(Component.For(typeof(IRepository<>)).ImplementedBy(typeof(Repository<>)).LifestylePerWebRequest());
            container.Register(Classes.FromThisAssembly().BasedOn<IController>().LifestylePerWebRequest());
            container.Register(Component.For<ITransactionManager>().ImplementedBy<TransactionManager>().LifestylePerWebRequest());
            //container.AddFacility<WcfFacility>().Register(Component.For<IMembershipProvider>().ImplementedBy<MembershipProvider>());
            container.AddFacility<WcfFacility>();

            container.Register(Component.For<IProvider>().ImplementedBy<BaseProvider>().LifestylePerWebRequest());
            container.Register(Component.For<IManageUserProvider>().ImplementedBy<ManageUserProvider>().LifestylePerWebRequest());
            container.Register(Component.For<IManageProductProvider>().ImplementedBy<ManageProductProvider>().LifestylePerWebRequest());
            container.Register(Component.For<IManageKitchenProvider>().ImplementedBy<ManageKitchenProvider>().LifestylePerWebRequest());
            container.Register(Component.For<IManageFavoritesProvider>().ImplementedBy<ManageFavoritesProvider>().LifestylePerWebRequest());
            container.Register(Component.For<IAuthenticationService>().ImplementedBy<AuthenticationService>().LifestylePerWcfOperation());
            container.Register(Component.For<IManageStoreProvider>().ImplementedBy<ManageStoreProvider>().LifestylePerWebRequest());
            container.Register(Component.For<IManageMatchingTexts>().ImplementedBy<ManageMatchingTexts>().LifestylePerWebRequest());
            container.Register(Component.For<IManageCaches>().ImplementedBy<ManageCaches>().LifestylePerWebRequest());
            container.Register(Component.For<IManageProductByUserProvider>().ImplementedBy<ManageProductByUserProvider>().LifestylePerWebRequest());
            container.Register(Component.For<IManageIngredientProvider>().ImplementedBy<ManageIngredientProvider>().LifestylePerWebRequest());

            container.Register(
                AllTypes
                 .FromAssembly(typeof(IAuthenticationService).Assembly)
                 .Pick()
                 .WithService.DefaultInterfaces()
                 .Configure(m => m.LifestylePerWebRequest()));
            
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }
    }
}