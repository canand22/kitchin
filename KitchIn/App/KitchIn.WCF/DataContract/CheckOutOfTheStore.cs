using System.Runtime.Serialization;

namespace KitchIn.WCF.DataContract
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