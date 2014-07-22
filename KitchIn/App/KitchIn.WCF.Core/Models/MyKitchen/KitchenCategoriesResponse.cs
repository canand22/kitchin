using System.Collections.Generic;
using System.Runtime.Serialization;
using KitchIn.Core.Models;

namespace KitchIn.WCF.Core.Models.MyKitchen
{
    [DataContract]
    [KnownType(typeof(CategoryiPhoneModel))]
    public class KitchenCategoriesResponse
    {
        [DataMember]
        public IEnumerable<CategoryiPhoneModel> Categories { get; set; }
    }
}