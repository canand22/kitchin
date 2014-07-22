using System;
using System.Runtime.Serialization;

namespace KitchIn.WCF.Core.Models.MyKitchen
{
    [DataContract]
    public class KitchenCategoriesRequest
    {
        [DataMember]
        public Guid Guid { get; set; }
    }
}