using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KitchIn.WCF.Core.Models.MyKitchen
{
    [DataContract]
    public class ProductsResponse
    {
        [DataMember]
        public IEnumerable<KeyValuePair<long, string>> Products { get; set; }
    }
}