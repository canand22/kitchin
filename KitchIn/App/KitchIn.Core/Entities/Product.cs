using Iesi.Collections;
using Iesi.Collections.Generic;
using SmartArch.Core.Domain.Base;
using System;
using KitchIn.Core.Enums;

namespace KitchIn.Core.Entities
{
    public class Product : BaseEntity
    {
        public virtual string UpcCode { get; set; }

        public virtual string ShortName { get; set; }

        public virtual string Name { get; set; }

        public virtual Ingredient Ingredient { get; set; }

        public virtual Category Category { get; set; }

        public virtual Store Store { get; set; }

        public virtual TypeAdd TypeAdd { get; set; }

        public virtual DateTime ModificationDate { get; set; }
    }
}
