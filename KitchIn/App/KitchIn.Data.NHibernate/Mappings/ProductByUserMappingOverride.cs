using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using KitchIn.Core.Entities;

namespace KitchIn.Data.NHibernate.Mappings
{
    public class ProductByUserMappingOverride : IAutoMappingOverride<ProductByUser>
    {
        public void Override(AutoMapping<ProductByUser> mapping)
         {
             mapping.Table("ProductsFromUsers");
             mapping.Id().GeneratedBy.Identity();
             mapping.Map(x => x.Name).Nullable();
             mapping.Map(x => x.ShortName).Nullable();
             mapping.Map(x => x.UpcCode).Nullable();
             mapping.Map(x => x.IngredientName).Nullable();
             mapping.Map(x => x.Date).Not.Nullable();
             mapping.Map(x => x.ExpirationDate).Nullable();

             mapping.References(x => x.Store, "StoreId").Nullable();
             mapping.References(x => x.Category, "CategoryId").Nullable();
             mapping.References(x => x.User, "UserId").Not.Nullable();
         }
    }
}