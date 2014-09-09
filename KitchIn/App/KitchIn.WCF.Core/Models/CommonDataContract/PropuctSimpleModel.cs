using System.Runtime.Serialization;

namespace KitchIn.WCF.Core.Models.CommonDataContract
{
    [DataContract]
    public class PropuctSimpleModel
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Category { set; get; }

        [DataMember]
        public string ShortName { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string IngredientName { get; set; }
    }
}
