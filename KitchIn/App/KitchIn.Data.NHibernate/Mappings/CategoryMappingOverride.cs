using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using KitchIn.Core.Entities;

namespace KitchIn.Data.NHibernate.Mappings
{
    public class CategoryMappingOverride : IAutoMappingOverride<Category>
    {
        public void Override(AutoMapping<Category> mapping)
        {
            mapping.Table("Categories");
            mapping.Id().GeneratedBy.Identity();
            mapping.Map(x => x.Name).Not.Nullable();
            mapping.Map(x => x.Description).Nullable();
        }
    }
}