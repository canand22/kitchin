using System.Runtime.Serialization;

namespace KitchIn.WCF.Core.Models.CommonDataContract
{
    [DataContract]
    public class CheckOutOfTheStore
    {
        [DataMember]
        public long StoreId { get; set; }

        [DataMember]
        public string ImageAsBase64String { get; set; }
    }
}