using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace KitchIn.WCF.Core.Authentification
{
    public class WrapSecurityAuthentications : IDisposable
    {
        private OperationContextScope scope;

        public WrapSecurityAuthentications()
        {
        }

        public WrapSecurityAuthentications(string username, string password, IClientChannel clientChannel)
        {
            this.scope = new OperationContextScope(clientChannel);
            HttpRequestMessageProperty request = new HttpRequestMessageProperty();
            request.Headers[System.Net.HttpRequestHeader.Authorization] = "Authorization" + this.EncodeBasicAuthenticationCredentials(username, password);
            OperationContext.Current.OutgoingMessageProperties.Add(HttpRequestMessageProperty.Name, request);
        }

        private string EncodeBasicAuthenticationCredentials(string username, string password)
        {
            string credentials = username + ":" + password;

            //Http uses ascii character encoding, WP7 doesn’t include
            // support for ascii encoding but it is easy enough to convert
            // since the first 128 characters of unicode are equivalent to ascii.
            // Any characters over 128 can’t be expressed in ascii so are replaced
            // by ?
            var asciiCredentials = (from c in credentials
                                    select c <= 0x7f ? (byte)c : (byte)'?').ToArray();

            //finally Base64 encode the result
            return Convert.ToBase64String(asciiCredentials);
        }

        public void Dispose()
        {
            this.scope.Dispose();
        }
    }
}