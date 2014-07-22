using System.Collections.Generic;
using System.Runtime.Serialization;
using KitchIn.Core.Models;

namespace KitchIn.WCF.Core.Models.MyKitchen
{
    [DataContract]
    [KnownType(typeof(ProductiPhoneModel))]
    public class KitchenProductsResponse
    {
        [DataMember]
        public IEnumerable<ProductiPhoneModel> Products { get; set; }
    }
}