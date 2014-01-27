using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using KitchIn.Core.Entities;

namespace KitchIn.Data.NHibernate.Mappings
{
    public class ProductMappingOverride : IAutoMappingOverride<Product>
    {
         public void Override(AutoMapping<Product> mapping)
         {
             mapping.Table("Products");
             mapping.Id().GeneratedBy.Identity();
             mapping.Map(x => x.Name).Not.Nullable();
             mapping.Map(x => x.ShortName).Not.Nullable();
             mapping.Map(x => x.UpcCode).Nullable();
             mapping.Map(x => x.TypeAdd).Not.Nullable();
             mapping.Map(x => x.ModificationDate).Not.Nullable();
             mapping.Map(x => x.IngredientName).Nullable();

             mapping.References(x => x.Store, "StoreId").Not.Nullable();
             mapping.References(x => x.Category, "CategoryId").Nullable();
         }
    }
}