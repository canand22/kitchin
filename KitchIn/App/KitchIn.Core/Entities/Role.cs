using Iesi.Collections.Generic;
using SmartArch.Core.Domain.Base;

namespace KitchIn.Core.Entities
{
    /// <summary>
    /// Describes Role entity
    /// </summary>
    public class Role : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public virtual string Name { get; set; }
    }
}
