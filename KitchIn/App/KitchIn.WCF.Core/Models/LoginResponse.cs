using System;
using System.Runtime.Serialization;

namespace KitchIn.WCF.Core.Models
{
    [DataContract]
    public class LoginResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public Guid? SessionId { get; set; }
    }
}