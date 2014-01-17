using System.Runtime.Serialization;

namespace KitchIn.WCF.Core.Models
{
    [DataContract]
    public class RegisterRequest
    {
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }
    }
}