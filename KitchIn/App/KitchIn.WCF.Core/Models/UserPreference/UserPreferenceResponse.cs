using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KitchIn.WCF.Core.Models.UserPreference
{
    [DataContract]
    public class UserPreferenceResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string Message { get; set; }


    }
}
