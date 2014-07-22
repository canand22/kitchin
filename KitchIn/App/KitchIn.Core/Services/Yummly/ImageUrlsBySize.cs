using Newtonsoft.Json;

namespace KitchIn.Core.Services.Yummly
{
    public class ImageUrlsBySize
    {
        [JsonProperty("90")]
        public string Size90 { get; set; }
    }
}
