using Newtonsoft.Json;

namespace KitchIn.Core.Services.Yummly.Recipe
{
    public class ImageUrlsBySize
    {
        [JsonProperty("90")]
        public string Image90 { get; set; }

        [JsonProperty("360")]
        public string Image360 { get; set; }
    }
}
