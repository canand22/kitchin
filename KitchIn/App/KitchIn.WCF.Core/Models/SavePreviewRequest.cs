using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KitchIn.Core.Models;

namespace KitchIn.WCF.Core.Models
{
    [DataContract]
    [KnownType(typeof(ProductiPhoneModel))]
    public class SavePreviewRequest
    {
        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public IEnumerable<ProductiPhoneModel> Products { get; set; }
    }
}