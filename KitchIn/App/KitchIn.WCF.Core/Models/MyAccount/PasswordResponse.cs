using System;
using System.Runtime.Serialization;

namespace KitchIn.WCF.Core.Models.MyAccount
{
    [DataContract]
    public class PasswordResponse
    {
        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public bool PasswordUpdated { get; set; }
    }
}