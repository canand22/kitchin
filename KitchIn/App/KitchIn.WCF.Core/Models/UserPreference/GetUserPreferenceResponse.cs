using KitchIn.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KitchIn.WCF.Core.Models.UserPreference
{
    [DataContract]
    public class GetUserPreferenceResponse
    {
        [DataMember]
        public List<UserPreferenceiPhoneModel> UserPreference { get; set; }
    }
}
