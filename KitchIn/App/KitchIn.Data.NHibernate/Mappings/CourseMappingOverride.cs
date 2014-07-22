using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using KitchIn.Core.Entities;

namespace KitchIn.Data.NHibernate.Mappings
{
    public class CourseMappingOverride : IAutoMappingOverride<Course>
    {
        public void Override(AutoMapping<Course> mapping)
        {
            mapping.Table("Courses");
            mapping.Id().GeneratedBy.Identity();
            mapping.Map(x => x.YummlyId).Not.Nullable();
            mapping.Map(x => x.Name).Not.Nullable();
            mapping.Map(x => x.Type);
            mapping.Map(x => x.Description);
            mapping.Map(x => x.SearchValue);
            mapping.Map(x => x.LocalesAvailableIn).Not.Nullable();
        }
    }
}
