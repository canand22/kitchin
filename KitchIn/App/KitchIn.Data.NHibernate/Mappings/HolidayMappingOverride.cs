using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using KitchIn.Core.Entities;

namespace KitchIn.Data.NHibernate.Mappings
{
    public class HolidayMappingOverride : IAutoMappingOverride<Holiday>
    {
        public void Override(AutoMapping<Holiday> mapping)
        {
            mapping.Table("Holidays");
            mapping.Id().GeneratedBy.Identity();
            mapping.Map(x => x.YummlyId).Not.Nullable();
            mapping.Map(x => x.Name).Not.Nullable();
            mapping.Map(x => x.Type);
            mapping.Map(x => x.Description);
            mapping.Map(x => x.SearchValue);
            mapping.Map(x => x.LocalesAvailableIn).Not.Nullable();
            mapping.HasManyToMany(x => x.UserPreferences)
              .Table("UserPreferences_Holiday")
                    .ParentKeyColumn("HolidayId")
                    .ChildKeyColumn("UserPreferencesId")
                    .Not.LazyLoad();
        }
    }
}
