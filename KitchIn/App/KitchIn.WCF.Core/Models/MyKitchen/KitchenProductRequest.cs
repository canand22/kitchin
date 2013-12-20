using System;
using System.Runtime.Serialization;
using KitchIn.Core.Models;

namespace KitchIn.WCF.Core.Models.MyKitchen
{
    [DataContract]
    [KnownType(typeof(ProductiPhoneModel))]
    public class KitchenProductRequest
    {
        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public ProductiPhoneModel Product { get; set; }
    }
}