using SmartArch.Core.Domain.Base;
using System;

namespace KitchIn.Core.Entities
{
    public class ProductByUser : BaseEntity
    {
        public virtual string UpcCode { get; set; }

        public virtual string ShortName { get; set; }

        public virtual string Name { get; set; }

        public virtual string IngredientName { get; set; }

        public virtual Category Category { get; set; }

        public virtual Store Store { get; set; }

        public virtual User User { get; set; }

        public virtual int? ExpirationDate { get; set; }

        public virtual DateTime Date { get; set; }
    }
}
