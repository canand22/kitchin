using System.Collections.Generic;
using System.Collections.ObjectModel;
using Iesi.Collections.Generic;
using Newtonsoft.Json;
using SmartArch.Core.Domain.Base;
using System;

namespace KitchIn.Core.Entities
{
    [JsonObject]
    public class Diet : BaseEntity
    {
        public virtual Iesi.Collections.Generic.ISet<UserPreference> UserPreferences { get; set; }
        public Diet()
        {
            UserPreferences = new HashedSet<UserPreference>();
        }
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

        public virtual void AddUserPreference(UserPreference userPreference)
        {
            if (userPreference == null)
                throw new ArgumentNullException("userPreference");
            if (!UserPreferences.Contains(userPreference))
            {
                UserPreferences.Add(userPreference);
                userPreference.AddDiet(this);
            }
        }
        public virtual void RemoveUserPreference(UserPreference userPreference)
        {
            if (userPreference == null)
                throw new ArgumentNullException("userPreference");
            if (UserPreferences.Contains(userPreference))
            {
                UserPreferences.Remove(userPreference);
                userPreference.RemoveDiet(this);
            }
        }
    }
}
