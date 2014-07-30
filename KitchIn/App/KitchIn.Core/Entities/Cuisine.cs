using System.Collections.Generic;
using System.Collections.ObjectModel;
using Iesi.Collections;
using Iesi.Collections.Generic;
using Newtonsoft.Json;
using SmartArch.Core.Domain.Base;
using System;

namespace KitchIn.Core.Entities
{
    [JsonObject]
    public class Cuisine : BaseEntity
    {
        public virtual Iesi.Collections.Generic.ISet<UserPreference> UserPreferences { get; set; }
        public Cuisine()
        {
            UserPreferences = new HashedSet<UserPreference>();
        }
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

        public virtual void AddUserPreference(UserPreference userPreference)
        {
            if (userPreference == null)
                throw new ArgumentNullException("userPreference");
            if (!UserPreferences.Contains(userPreference))
            {
                UserPreferences.Add(userPreference);
                userPreference.AddCuisine(this);
            }
        }
        public virtual void RemoveUserPreference(UserPreference userPreference)
        {
            if (userPreference == null)
                throw new ArgumentNullException("userPreference");
            if (UserPreferences.Contains(userPreference))
            {
                UserPreferences.Remove(userPreference);
                userPreference.RemoveCuisine(this);
            }
        }
    }
}
