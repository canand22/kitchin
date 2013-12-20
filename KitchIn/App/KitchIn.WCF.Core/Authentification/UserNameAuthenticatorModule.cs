using System;
using System.Text;
using System.Web;

using Microsoft.Practices.ServiceLocation;

using KitchIn.Core.Interfaces;

namespace KitchIn.WCF.Core.Authentification
{
    /// <summary>
    /// Http module to authenticate user.
    /// </summary>
    public class UserNameAuthenticatorModule : IHttpModule
    {
        /// <summary>
        /// The sync object for log authentication
        /// </summary>
        private static readonly object syncObject = new object();

        /// <summary>
        /// Service, which doesn't need authentification
        /// </summary>
        private const string AUTHORIZATION_SERVICE_NAME = "AccountService";

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Inits the specified application.
        /// </summary>
        /// <param name="application">The application.</param>
        public void Init(HttpApplication application)
        {
            application.AuthenticateRequest += this.OnAuthenticateRequest;
            application.EndRequest += this.OnEndRequest;
        }

        /// <summary>
        /// Called when authenticate request.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="eventArgs">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void OnAuthenticateRequest(object source, EventArgs eventArgs)
        {
            var app = (HttpApplication)source;
            string authHeader = app.Request.Headers[System.Net.HttpRequestHeader.Authorization.ToString()];

            if (app.Request.RawUrl.Contains(AUTHORIZATION_SERVICE_NAME))
            {
                return;
            }

            // check, if the correct authorization header is not present 
            if (string.IsNullOrEmpty(authHeader) || authHeader.Trim().IndexOf("Authorization", 0) != 0)
            {
                this.DenyAccess(app);
                return;
            }

            // extract credentials
            authHeader = authHeader.Trim();
            string encodedCredentials = authHeader.Substring(13);
            byte[] decodedBytes = Convert.FromBase64String(encodedCredentials);
            string s = new ASCIIEncoding().GetString(decodedBytes);
            string[] userPass = s.Split(new[] { ':' });
            string username = userPass[0];
            string password = userPass[1];

            if (!ServiceLocator.Current.GetInstance<IMembershipProvider>().ValidateUser(username, password))
            {
                this.DenyAccess(app);
            }
        }

        /// <summary>
        /// Called when end request.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="eventArgs">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void OnEndRequest(object source, EventArgs eventArgs)
        {
            if (HttpContext.Current.Response.StatusCode == 401)
            {
                //if the status is 401 the WWW-Authenticated is added to  
                //the response so client knows it needs to send credentials  
                HttpContext context = HttpContext.Current;
                context.Response.StatusCode = 401;
                context.Response.AddHeader("WWW-Authenticate", "Basic Realm");
            }
        }

        /// <summary>
        /// the status of response is set to 401 and it ended 
        /// the end request will check if it is 401 and add 
        /// the authentication header so the client knows 
        /// it needs to send credentials to authenticate .
        /// </summary>
        /// <param name="app">The http app.</param>
        private void DenyAccess(HttpApplication app)
        {
            app.Response.StatusCode = 401;
            app.Response.StatusDescription = "Access Denied";

            // error not authenticated 
            app.Response.Write("401 Access Denied");

            app.CompleteRequest();
        }
    }
}
