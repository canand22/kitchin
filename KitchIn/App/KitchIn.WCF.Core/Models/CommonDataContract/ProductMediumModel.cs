using System.Runtime.Serialization;
using KitchIn.Core.Models;

namespace KitchIn.WCF.Core.Models.CommonDataContract
{
    using KitchIn.Core.Entities;

    [DataContract]
    public class ProductMediumModel
    {
        [DataMember]
        public string ItemName { get; set; }

        [DataMember]
        public long? Id { get; set; }

        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public bool IsSuccessMatching { get; set; }

        public static implicit operator ProductMediumModel(ResultMatching resultMatching)
        {
            var model = new ProductMediumModel()
            {
                Id = resultMatching.Id,
                IsSuccessMatching = resultMatching.IsSuccessMatching,
                ItemName = resultMatching.ItemName,
                Category = resultMatching.Category
            };
            return model;
        }

        public static implicit operator ProductMediumModel(Product product)
        {
            var model = new ProductMediumModel()
            {
                Id = product.Id,
                IsSuccessMatching = true,
                ItemName = product.Name,
                Category = product.Category.Name
            };
            return model;
        }

    }
}