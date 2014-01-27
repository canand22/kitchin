using System.Runtime.Serialization;

namespace KitchIn.WCF.Core.Models.CommonDataContract
{
    using System;

    [DataContract]
    public class ProductByUserModel
    {
        [DataMember]
        public string UpcCode { get; set; }

        [DataMember]
        public string ShortName { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string IngredientName { get; set; }

        [DataMember]
        public long CategoryId { get; set; }

        [DataMember]
        public long StoreId { get; set; }

        [DataMember]
        public string ExpirationDate { get; set; }

        [DataMember]
        public Guid SessionId { get; set; }
    }
}
