using SmartArch.Core.Domain.Base;

namespace KitchIn.Core.Entities
{
    public class CategoryInStore : BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual Store Store { get; set; }
    }
}
