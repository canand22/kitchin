using System.Runtime.Serialization;

namespace KitchIn.WCF.Core.Models.CommonDataContract
{
    [DataContract]
    public class PropuctSimpleModel
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
