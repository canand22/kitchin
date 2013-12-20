using System;
using System.Runtime.Serialization;

namespace KitchIn.WCF.Core.Models.MyKitchen
{
    [DataContract]
    public class KitchenProductsRequest
    {
        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public long CategoryId { get; set; }
    }
}