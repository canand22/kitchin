using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using KitchIn.Core.Entities;

namespace KitchIn.Data.NHibernate.Mappings
{
    public class FavoriteRecipeMappingOverride : IAutoMappingOverride<FavoriteRecipe>
    {
        public void Override(AutoMapping<FavoriteRecipe> mapping)
        {
            mapping.Table("FavoritesRecipes");
            mapping.Id().GeneratedBy.Identity();
            mapping.Map(x => x.Name).Nullable();
            mapping.Map(x => x.RecipeBigOven).Nullable();

            mapping.HasManyToMany(x => x.Users)
                .Table("FavoritesRecipes_Users")
                .ParentKeyColumn("FavoriteRecipeId")
                .ChildKeyColumn("UserId")
                .Inverse()
                .Not.LazyLoad()
                .Cascade.None();
        }
    }
}