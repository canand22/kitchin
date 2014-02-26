using System;
using System.Runtime.Serialization;

namespace KitchIn.WCF.Core.Models.CommonDataContract
{
    [DataContract]
    public class StatusResponse
    {
        [DataMember]
        public Boolean IsSuccessfully { get; set; }

        [DataMember]
        public String Message { get; set; }
    }
}
