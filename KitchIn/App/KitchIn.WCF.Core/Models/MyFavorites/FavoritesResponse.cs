using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KitchIn.WCF.Core.Models.MyFavorites
{
    [DataContract]
    public class FavoritesResponse
    {
        public IEnumerable<string> BigOvenRecipes { get; set; }
    }
}