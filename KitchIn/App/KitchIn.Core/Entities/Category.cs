using SmartArch.Core.Domain.Base;

namespace KitchIn.Core.Entities
{
    public class Category : BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }
    }
}
