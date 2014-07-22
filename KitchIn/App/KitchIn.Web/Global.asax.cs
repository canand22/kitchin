using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

using Castle.MicroKernel.Registration;
using Castle.Windsor;

using FluentValidation.Mvc;

using KitchIn.Core;
using KitchIn.ServiceLocation;

namespace KitchIn.Web
{
    /// <summary>
    /// MVC Application main class
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// Castle Windsor container
        /// </summary>
        private static IWindsorContainer container;

        /// <summary>
        /// Registers the global filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Account", action = "LogOn", id = UrlParameter.Optional }, // Parameter defaults
                new[] { "KitchIn.Web" });
        }

        /// <summary>
        /// Application_s the start.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            container = new WindsorContainer();

            // NHibernate Initialization
            container.Install(new WebNHibernateInstaller());

            container.Register(Classes.FromThisAssembly().BasedOn<IController>().LifestylePerWebRequest());
            FluentValidationModelValidatorProvider.Configure();

            LogWriter.WriteInfo("Application started successfully!");
        }

        /// <summary>
        /// Application end event.
        /// </summary>
        protected void Application_End()
        {
            container.Dispose();
            LogWriter.WriteInfo("Application stopped!");
        }
    }
}