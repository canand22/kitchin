using System;
using System.Runtime.Serialization;

namespace KitchIn.WCF.Core.Models.MyFavorites
{
    [DataContract]
    public class SetFavoritesRequest
    {
        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public string RecipesBigOven { get; set; }

        [DataMember]
        public bool HasFavorites { get; set; }
    }
}