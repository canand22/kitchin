using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using KitchIn.Core.Entities;

namespace KitchIn.Data.NHibernate.Mappings
{
    public class StoreMappingOverride : IAutoMappingOverride<Store>
    {
        public void Override(AutoMapping<Store> mapping)
        {
            mapping.Table("Stores");
            mapping.Id().GeneratedBy.Identity();
            mapping.Map(x => x.Name).Nullable();
            mapping.Map(x => x.Latitude).Nullable();
            mapping.Map(x => x.Longitude).Nullable();
        }
    }
}