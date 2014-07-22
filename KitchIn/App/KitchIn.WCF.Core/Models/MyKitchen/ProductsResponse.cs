using System.Collections.Generic;
using System.Runtime.Serialization;
using KitchIn.WCF.Core.Models.CommonDataContract;

namespace KitchIn.WCF.Core.Models.MyKitchen
{
    [DataContract]
    public class ProductsResponse
    {
        [DataMember]
        public IEnumerable<PropuctSimpleModel> Products { get; set; }
    }
}