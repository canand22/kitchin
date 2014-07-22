using System;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using SmartArch.Core.Domain.Base;

namespace KitchIn.Core.Entities
{
    public class Recipe : BaseEntity
    {
        public Recipe()
        {
            this.Ingredients = new HashedSet<Ingredient>();
        }
        public virtual string Title { get; set; }

        public virtual int NumberOfStars { get; set; }

        public virtual long? NumberOfReviews { get; set; }

        public virtual long PreparationTimeInSeconds { get; set; }

        public virtual long CookTimeInSeconds { get; set; }

        public virtual string Instruction { get; set; }

        public virtual ISet<Ingredient> Ingredients { get; set; }

        public virtual string PhotoUrl { get; set; }
    }
}
