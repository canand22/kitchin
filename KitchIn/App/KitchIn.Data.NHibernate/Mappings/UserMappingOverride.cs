using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using KitchIn.Core.Entities;

namespace KitchIn.Data.NHibernate.Mappings
{
    public class UserMappingOverride : IAutoMappingOverride<User>
    {
        public void Override(AutoMapping<User> mapping)
        {
            mapping.Table("Users");
            mapping.Id().GeneratedBy.Identity();
            mapping.Map(x => x.Email).Not.Nullable();
            mapping.Map(x => x.Password).Not.Nullable();
            mapping.Map(x => x.FirstName).Not.Nullable();
            mapping.Map(x => x.LastName).Not.Nullable();
            mapping.Map(x => x.Role).CustomType<int>().Not.Nullable();
            mapping.Map(x => x.SessionId).Nullable();
            mapping.HasMany(x => x.Products).KeyColumn("UserId").Cascade.AllDeleteOrphan().Inverse();

            mapping.HasManyToMany(x => x.FavoriteRecipes)
                .Table("FavoritesRecipes_Users")
                      .ParentKeyColumn("UserId")
                      .ChildKeyColumn("FavoriteRecipeId")
                      .Not.LazyLoad();
        }
    }
}