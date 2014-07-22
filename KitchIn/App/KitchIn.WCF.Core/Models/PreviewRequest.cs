using System;
using System.Runtime.Serialization;

namespace KitchIn.WCF.Core.Models
{
    [DataContract]
    public class PreviewRequest
    {
        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public string Img { get; set; }
    }
}