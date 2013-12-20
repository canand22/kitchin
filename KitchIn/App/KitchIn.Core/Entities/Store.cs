using SmartArch.Core.Domain.Base;

namespace KitchIn.Core.Entities
{
    public class Store : BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual double Latitude { get; set; }

        public virtual double Longitude { get; set; }
    }
}