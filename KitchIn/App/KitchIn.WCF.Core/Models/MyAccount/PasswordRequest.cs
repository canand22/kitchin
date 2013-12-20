using System;
using System.Runtime.Serialization;

namespace KitchIn.WCF.Core.Models.MyAccount
{
    [DataContract]
    public class PasswordRequest
    {
        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public string OldPassword { get; set; }

        [DataMember]
        public string NewPassword { get; set; }
    }
}