using System.Runtime.Serialization;
using KitchIn.Core.Models;

namespace KitchIn.WCF.DataContract
{
    [DataContract]
    public class ListProducts
    {
        [DataMember]
        public string ItemName { get; set; }

        [DataMember]
        public long? Id { get; set; }

        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public bool IsSuccessMatching { get; set; }

        public static implicit operator ListProducts(ResultMatching resultMatching)
        {
            var model = new ListProducts()
            {
                Id = resultMatching.Id,
                IsSuccessMatching = resultMatching.IsSuccessMatching,
                ItemName = resultMatching.ItemName,
                Category = resultMatching.Category
            };
            return model;
        }
    
    }
}