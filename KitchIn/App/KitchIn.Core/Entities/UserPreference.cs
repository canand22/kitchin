using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartArch.Core.Domain.Base;

namespace KitchIn.Core.Entities
{
    public class UserPreference:BaseEntity
    {
        public virtual ISet<Ingredient> AllowedIngridients { get; set; }

        public virtual ISet<Ingredient> ExcludedIngridients { get; set; }

        public virtual string Meal { get; set; }

        public virtual string DishType { get; set; }

        public virtual List<string> Time { get; set; }

        public virtual ISet<Holiday> Holidays { get; set; }

        public virtual ISet<Allergy> Allergies { get; set; }

        public virtual ISet<Diet> Diets { get; set; }

        public virtual ISet<Cuisine> Cuisines { get; set; }

        public virtual ISet<User> User { get; set; }


    }
}
