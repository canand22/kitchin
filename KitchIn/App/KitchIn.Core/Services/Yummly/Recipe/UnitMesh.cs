using Newtonsoft.Json;

namespace KitchIn.Core.Services.Yummly.Recipe
{
    public class UnitMesh
    {
        public string id { get; set; }
        public string name { get; set; }
        public string abbreviation { get; set; }
        public string plural { get; set; }
        public string pluralAbbreviation { get; set; }
        
        [JsonProperty("decimal")]
        public bool Decimal { get; set; }
    }
}
