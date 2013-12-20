using SmartArch.Core.Domain.Base;

namespace KitchIn.Core.Entities
{
    public class Product : BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual Category Category { get; set; }

        public virtual string ExpirationDate { get; set; }

        public virtual bool IsAddedByUser { get; set; }

        public virtual string ProductBigOven { get; set; }
    }
}
