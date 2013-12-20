using System.Runtime.Serialization;

namespace KitchIn.WCF.Core.Models
{
    [DataContract]
    public class LoginRequest
    {
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}