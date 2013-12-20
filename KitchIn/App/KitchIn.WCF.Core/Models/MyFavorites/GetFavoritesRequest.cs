using System;
using System.Runtime.Serialization;

namespace KitchIn.WCF.Core.Models.MyFavorites
{
    [DataContract]
    public class GetFavoritesRequest
    {
        [DataMember]
        public Guid Guid { get; set; }
    }
}