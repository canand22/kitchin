﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using KitchIn.Core.Entities;

namespace KitchIn.Data.NHibernate.Mappings
{
    public class DietMappingOverride : IAutoMappingOverride<Diet>
    {
        public void Override(AutoMapping<Diet> mapping)
        {
            mapping.Table("Diets");
            mapping.Id().GeneratedBy.Identity();
            mapping.Map(x => x.YummlyId).Not.Nullable();
            mapping.Map(x => x.ShortDescription);
            mapping.Map(x => x.LongDescription);
            mapping.Map(x => x.SearchValue);
            mapping.Map(x => x.Type);
            mapping.Map(x => x.LocalesAvailableIn).Not.Nullable();
            mapping.HasManyToMany(x => x.UserPreferences)
             .Table("UserPreferences_Diet")
                   .ParentKeyColumn("DietId")
                   .ChildKeyColumn("UserPreferencesId")
                   .Not.LazyLoad();
        }
    }
}
