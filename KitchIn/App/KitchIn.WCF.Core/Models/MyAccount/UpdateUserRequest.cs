using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KitchIn.WCF.Core.Models.MyAccount
{
    [DataContract]
    public class UpdateUserRequest
    {
        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public string OldEmail { get; set; }

        [DataMember]
        public string NewEmail { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

    }
}
