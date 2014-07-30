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
            mapping.HasMany(x => x.AllowedIngredients).KeyColumn("AllowedIngridientId");
            mapping.HasMany(x => x.ExcludedIngredients).KeyColumn("ExcludedIngridientId");
            mapping.Map(x => x.Meal).Not.Nullable();
            mapping.References(x => x.DishType, "DishTypeId");
            mapping.Map(x => x.Time);
            mapping.Map(x => x.OwnerPreference);
            mapping.HasManyToMany(x => x.Cuisines)
              .Table("UserPreferences_Cuisine")
                    .ParentKeyColumn("UserPreferencesId")
                    .ChildKeyColumn("CiusineId")
                    .Not.LazyLoad()
                    .Cascade.SaveUpdate();
            mapping.HasManyToMany(x => x.Diets)
              .Table("UserPreferences_Diet")
                    .ParentKeyColumn("UserPreferencesId")
                    .ChildKeyColumn("DietId")
                    .Not.LazyLoad()
                    .Cascade.SaveUpdate();
            mapping.HasManyToMany(x => x.Allergies)
              .Table("UserPreferences_Allergy")
                    .ParentKeyColumn("UserPreferencesId")
                    .ChildKeyColumn("AllergyId")
                    .Not.LazyLoad()
            .Cascade.SaveUpdate();
            mapping.HasManyToMany(x => x.Holidays)
                .Table("UserPreferences_Holiday")
                      .ParentKeyColumn("UserPreferencesId")
                      .ChildKeyColumn("HolidayId")
                      .Not.LazyLoad()
            .Cascade.SaveUpdate();
            mapping.References(x => x.User, "UserId").Nullable();
            mapping.HasManyToMany(x => x.AllowedIngredients)
               .Table("UserPreferences_AllowedIngredients")
                     .ParentKeyColumn("UserPreferencesId")
                     .ChildKeyColumn("AllowedIngredientsId")
                     .Not.LazyLoad()
            .Cascade.SaveUpdate();
            mapping.HasManyToMany(x => x.ExcludedIngredients)
               .Table("UserPreferences_ExcludedIngredients")
                     .ParentKeyColumn("UserPreferencesId")
                     .ChildKeyColumn("ExcludedIngredientsId")
                     .Not.LazyLoad()
            .Cascade.SaveUpdate();
        }
    }
}
