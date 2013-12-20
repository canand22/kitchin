﻿using FluentNHibernate.Automapping;
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
             mapping.Map(x => x.ExpirationDate).Nullable();
             mapping.Map(x => x.IsAddedByUser).Nullable();

             mapping.References(x => x.Category, "CategoryId").Nullable();
         }
    }
}