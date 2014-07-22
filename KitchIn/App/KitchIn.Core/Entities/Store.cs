using SmartArch.Core.Domain.Base;

namespace KitchIn.Core.Entities
{
    using System.Collections.Generic;

    public class Store : BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual double? Latitude { get; set; }

        public virtual double? Longitude { get; set; }

        public virtual IList<CategoryInStore> Categories { get; set; }
    }
}