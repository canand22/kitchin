using System.Web.Configuration;
using System.Web.Mvc;
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
    /// Controllers installer
    /// </summary>
    public class WebNHibernateInstaller : IWindsorInstaller
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
            container.Register(Component.For<ISession>().UsingFactoryMethod(x => x.Resolve<ISessionFactory>().OpenSession()).LifeStyle.PerWebRequest);

            container.Register(Component.For(typeof(IRepository<>)).ImplementedBy(typeof(Repository<>)).LifeStyle.Transient);
            container.Register(Classes.FromThisAssembly().BasedOn<IController>().LifestyleTransient());
            container.Register(Component.For<ITransactionManager>().ImplementedBy<TransactionManager>().LifeStyle.PerWebRequest);
            container.Register(Component.For<IMembershipProvider>().ImplementedBy<MembershipProvider>().LifeStyle.PerWebRequest);
            container.Register(Component.For<IAuthenticationService>().ImplementedBy<AuthenticationService>().LifeStyle.PerWebRequest);
            container.Register(
                AllTypes
                 .FromAssembly(typeof(IAuthenticationService).Assembly)
                 .Pick()
                 .WithService.DefaultInterfaces()
                 .Configure(m => m.LifestylePerWebRequest()));

            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }
    }
}