using Iesi.Collections;
using Iesi.Collections.Generic;
using Newtonsoft.Json;
using SmartArch.Core.Domain.Base;
using System;
using System.Collections.ObjectModel;

namespace KitchIn.Core.Entities
{
    [JsonObject]
    public class Ingredient : BaseEntity
    {

        [JsonProperty("id")]
        public virtual ISet<UserPreference> UserPreferences { get; set; }
        public Ingredient()
        {
            UserPreferences = new HashedSet<UserPreference>();
        }
        public virtual string YummlyId { get; set; }

        [JsonProperty("searchValue")]
        public virtual string SearchValue { get; set; }

        [JsonProperty("description")]
        public virtual string Description { get; set; }

        [JsonProperty("term")]
        public virtual string Term { get; set; }

        public virtual ISet<Product> Product { get; set; }

        public virtual ISet<Recipe> Recipe { get; set; }

        public virtual void AddUserPreference(UserPreference userPreference,bool isAllowed)
        {
            if (userPreference == null)
                throw new ArgumentNullException("userPreference");
            if (!UserPreferences.Contains(userPreference))
            {
                UserPreferences.Add(userPreference);
                if(isAllowed)
                    userPreference.AddAllowedIngredient(this);
                else
                    userPreference.AddAllowedIngredient(this);
            }
        }
        public virtual void RemoveUserPreference(UserPreference userPreference, bool isAllowed)
        {
            if (userPreference == null)
                throw new ArgumentNullException("userPreference");
            if (UserPreferences.Contains(userPreference))
            {
                UserPreferences.Remove(userPreference);
                if (isAllowed)
                    userPreference.RemoveAllowedIngredient(this);
                else
                    userPreference.RemoveAllowedIngredient(this);
            }
        }
  }
}
