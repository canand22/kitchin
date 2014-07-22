using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using KitchIn.Core.Entities;

namespace KitchIn.Data.NHibernate.Mappings
{
    public class UserPreferenceMappingOverride : IAutoMappingOverride<UserPreference>
    {
        public void Override(AutoMapping<UserPreference> mapping)
        {
            mapping.Table("UserPreferences");
            mapping.Id().GeneratedBy.Identity();
            mapping.HasMany(x => x.AllowedIngridients).KeyColumn("AllowedIngridientId");
            mapping.HasMany(x => x.ExcludedIngridients).KeyColumn("ExcludedIngridientId");
            mapping.Map(x => x.Meal).Not.Nullable();
            mapping.Map(x => x.DishType).Not.Nullable();
            mapping.Map(x => x.Time);
            mapping.HasManyToMany(x => x.Cuisines)
              .Table("UserPreferences_Cuisine")
                    .ParentKeyColumn("UserPreferencesId")
                    .ChildKeyColumn("CiusineId")
                    .Not.LazyLoad();
            mapping.HasManyToMany(x => x.Diets)
              .Table("UserPreferences_Diet")
                    .ParentKeyColumn("UserPreferencesId")
                    .ChildKeyColumn("DietId")
                    .Not.LazyLoad();
            mapping.HasManyToMany(x => x.Allergies)
              .Table("UserPreferences_Allergy")
                    .ParentKeyColumn("UserPreferencesId")
                    .ChildKeyColumn("AllergyId")
                    .Not.LazyLoad();
            mapping.HasManyToMany(x => x.Holidays)
                .Table("UserPreferences_Holiday")
                      .ParentKeyColumn("UserPreferencesId")
                      .ChildKeyColumn("HolidayId")
                      .Not.LazyLoad();
            mapping.HasManyToMany(x => x.User)
                .Table("UserPreferences_Users")
                      .ParentKeyColumn("UserPreferencesId")
                      .ChildKeyColumn("UserId")
                      .Not.LazyLoad();
        }
    }
}
