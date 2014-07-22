using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using KitchIn.Core.Entities;

namespace KitchIn.Data.NHibernate.Mappings
{
    public class RecipeMappingOverride: IAutoMappingOverride<Recipe>
    {
        public void Override(AutoMapping<Recipe> mapping)
        {
            mapping.Table("Recipes");
            mapping.Id().GeneratedBy.Identity();
            mapping.Map(x => x.Title).Not.Nullable();
            mapping.Map(x => x.NumberOfStars).Not.Nullable();
            mapping.Map(x => x.NumberOfReviews);
            mapping.Map(x => x.PreparationTimeInSeconds);
            mapping.Map(x => x.CookTimeInSeconds).Nullable();
            mapping.Map(x => x.PhotoUrl).Nullable();
            mapping.Map(x => x.Instruction).Nullable();
            mapping.HasManyToMany(x => x.Ingredients)
              .Table("Recipes_Ingredients")
                    .ParentKeyColumn("RecipeId")
                    .ChildKeyColumn("IngredientId")
                    .Not.LazyLoad();

        }
    }
}
