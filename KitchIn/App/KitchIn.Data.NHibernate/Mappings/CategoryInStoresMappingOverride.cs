using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace KitchIn.Data.NHibernate.Mappings
{
    using KitchIn.Core.Entities;

    public class CategoryInStoresMappingOverride : IAutoMappingOverride<CategoryInStore>
    {
        public void Override(AutoMapping<CategoryInStore> mapping)
        {
            mapping.Table("CategoryInStores");
            mapping.Id().GeneratedBy.Identity();
            mapping.Map(x => x.Name).Nullable();
            mapping.References(x => x.Store, "StoreId").Nullable();
        }
    }
}