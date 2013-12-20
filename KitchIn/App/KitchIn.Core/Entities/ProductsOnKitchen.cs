using System;
using KitchIn.Core.Enums;
using SmartArch.Core.Domain.Base;

namespace KitchIn.Core.Entities
{
    public class ProductsOnKitchen : BaseEntity
    {
        public ProductsOnKitchen()
        {
            this.UnitType = UnitType.None;
            this.DateOfPurchase = DateTime.Now;
        }

        public virtual Product Product { get; set; }

        public virtual Category Category { get; set; }

        public virtual string Name { get; set; }

        public virtual DateTime DateOfPurchase { get; set; }

        public virtual User User { get; set; }

        public virtual double Quantity { get; set; }

        public virtual UnitType UnitType { get; set; }
    }
}
