using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using KitchIn.Core.Entities;

namespace KitchIn.Data.NHibernate.Mappings
{
    public class IngredientMappingOverride : IAutoMappingOverride<Ingredient>
    {
        public void Override(AutoMapping<Ingredient> mapping)
        {
            mapping.Table("Ingredients");
            mapping.Id().GeneratedBy.Identity();
            mapping.Map(x => x.YummlyId).Not.Nullable();
            mapping.Map(x => x.SearchValue);
            mapping.Map(x => x.Description);
            mapping.Map(x => x.Term);

            mapping.HasMany(x => x.Product).KeyColumn("IngredientId");
            mapping.HasManyToMany(x => x.Recipe)
  .Table("Recipes_Ingredients")
        .ParentKeyColumn("IngredientId")
        .ChildKeyColumn("RecipeId")
        .Not.LazyLoad();
            mapping.HasManyToMany(x => x.UserPreferences)
               .Table("UserPreferences_AllowedIngredients")
                     .ParentKeyColumn("AllowedIngredientsId")
                     .ChildKeyColumn("UserPreferencesId")
                     .Not.LazyLoad()
                     .Cascade.SaveUpdate();
            mapping.HasManyToMany(x => x.UserPreferences)
               .Table("UserPreferences_ExcludedIngredients")
                     .ParentKeyColumn("ExcludedIngredientsId")
                     .ChildKeyColumn("UserPreferencesId")
                     .Not.LazyLoad()
                    .Cascade.SaveUpdate();
        }
    }
       
}
