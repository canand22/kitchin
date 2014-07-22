using Iesi.Collections.Generic;
using Newtonsoft.Json;
using SmartArch.Core.Domain.Base;

namespace KitchIn.Core.Entities
{
    [JsonObject]
    public class Diet : BaseEntity
    {
        [JsonProperty("id")]
        public virtual string YummlyId { get; set; }

        [JsonProperty("shortDescription")]
        public virtual string ShortDescription { get; set; }

        [JsonProperty("longDescription")]
        public virtual string LongDescription { get; set; }

        [JsonProperty("searchValue")]
        public virtual string SearchValue { get; set; }

        [JsonProperty("type")]
        public virtual string Type { get; set; }

        [JsonProperty("localesAvailableIn")]
        public virtual string[] LocalesAvailableIn { get; set; }

        public virtual ISet<UserPreference> UserPreferences { get; set; }
    }
}
