using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using KitchIn.Core.Entities;

namespace KitchIn.Data.NHibernate.Mappings
{
    public class ProductsOnKitchenMappingOverride : IAutoMappingOverride<ProductsOnKitchen>
    {
        public void Override(AutoMapping<ProductsOnKitchen> mapping)
        {
            mapping.Table("ProductsOnKitchens");
            mapping.Id().GeneratedBy.Identity();
            mapping.Map(x => x.Name).Not.Nullable();
            mapping.Map(x => x.DateOfPurchase).Nullable();
            mapping.Map(x => x.UnitType).CustomType<int>().Nullable();
            mapping.Map(x => x.Quantity).Nullable();
            mapping.References(x => x.User, "UserId").Nullable();
            mapping.References(x => x.Product, "ProductId").Cascade.All().Nullable();
            mapping.References(x => x.Category, "CategoryId").Nullable();
        }
    }
}