using Newtonsoft.Json;
using SmartArch.Core.Domain.Base;

namespace KitchIn.Core.Entities
{
    [JsonObject]
    public class Course : BaseEntity
    {
        [JsonProperty("id")]
        public virtual string YummlyId { get; set; }

        [JsonProperty("name")]
        public virtual string Name { get; set; }

        [JsonProperty("type")]
        public virtual string Type { get; set; }

        [JsonProperty("description")]
        public virtual string Description { get; set; }

        [JsonProperty("searchValue")]
        public virtual string SearchValue { get; set; }

        [JsonProperty("localesAvailableIn")]
        public virtual string[] LocalesAvailableIn { get; set; }
    }
}
