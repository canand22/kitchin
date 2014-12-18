using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading;
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
        public string ItemShortName { get; set; }

        [DataMember]
        public long? Id { get; set; }

        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public bool IsSuccessMatching { get; set; }

        [DataMember]
        public string YummlyName { get; set; }
        
        public static implicit operator ProductMediumModel(ResultMatching resultMatching)
        {
            var model = new ProductMediumModel()
            {
                Id = resultMatching.Id,
                IsSuccessMatching = resultMatching.IsSuccessMatching,
                ItemName = resultMatching.ItemName,
                Category = resultMatching.Category,
                ItemShortName = resultMatching.ItemShortName.ToLower(),
                YummlyName = resultMatching.IngredientName
            };
            return model;
        }

        public static implicit operator ProductMediumModel(Product product)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            var model = new ProductMediumModel()
            {
                Id = product.Id,
                IsSuccessMatching = true,
                ItemName = product.Name,
                Category = product.Category.Name,
                ItemShortName = product.ShortName,
                YummlyName = !String.IsNullOrWhiteSpace(product.IngredientName) ? textInfo.ToTitleCase(product.IngredientName) : product.IngredientName
            };
            return model;
        }
    }
}