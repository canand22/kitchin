using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using KitchIn.Core.Models;

namespace KitchIn.WCF.Core.Models.UserPreference
{
    [DataContract]
    public class UserPreferenceRequest
    {
        [DataMember]
        public Guid Guid { get; set; }
        

        [DataMember]
        public UserPreferenceiPhoneModel UserPreference { get; set; }
    }
}
