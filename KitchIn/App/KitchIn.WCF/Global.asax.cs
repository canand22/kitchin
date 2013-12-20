using System;

using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using KitchIn.ServiceLocation;
using KitchIn.WCF.Contracts;
using KitchIn.WCF.Services;

namespace KitchIn.WCF
{
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
            container.Register(Component.For<IKitchInAppService>().ImplementedBy<KitchInAppService>());
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