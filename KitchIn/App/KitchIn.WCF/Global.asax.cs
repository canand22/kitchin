using System;

using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using KitchIn.Core.Services.Jobs;
using KitchIn.Core.Services.Yummly;
using KitchIn.ServiceLocation;
using KitchIn.WCF.Contracts;
using KitchIn.WCF.Services;

namespace KitchIn.WCF
{
    using KitchIn.Core.Services.Cache;
    using KitchIn.Core;

    /// <summary>
    /// The application global events.
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// The Windsor conteiner
        /// </summary>
        private static IWindsorContainer container;

        /// <summary>
        /// The start application
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Application_Start(object sender, EventArgs e)
        {
            container = new WindsorContainer();

            // NHibernate Initialization
            container.Install(new WcfNHibernateInstaller());

            ////container.Register(Component.For<IAccountService>().ImplementedBy<AccountService>());
            //container.Register(Component.For<IKitchInAppService>().ImplementedBy<KitchInAppService>());

            container.Register(Component.For<IKitchInAppService>().ImplementedBy<KitchInAppService>().AsWcfService(new DefaultServiceModel().
            Hosted()).IsDefault().LifestylePerWebRequest());
            
            container.Register(Component.For<IRunable>().ImplementedBy<YummlyMetaUpdater>().LifestyleSingleton());
            container.Register(Component.For<IYummly>().ImplementedBy<YummlyManager>().LifestyleSingleton());

            //container.AddFacility<WcfFacility>();
        }

        /// <summary>
        /// The end application
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Application_End(object sender, EventArgs e)
        {
            if (container != null)
            {
                container.Dispose();
            }
        }
    }
}