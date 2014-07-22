using Iesi.Collections;
using Iesi.Collections.Generic;
using Newtonsoft.Json;
using SmartArch.Core.Domain.Base;

namespace KitchIn.Core.Entities
{
    [JsonObject]
    public class Ingredient : BaseEntity
    {
        [JsonIgnore]
        public virtual string YummlyId { get; set; }

        [JsonProperty("searchValue")]
        public virtual string SearchValue { get; set; }

        [JsonProperty("description")]
        public virtual string Description { get; set; }

        [JsonProperty("term")]
        public virtual string Term { get; set; }

        public virtual ISet<Product> Product { get; set; }

        public virtual ISet<Recipe> Recipe { get; set; }
    }
}
