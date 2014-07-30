using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KitchIn.Core.Entities;
using KitchIn.Core.Enums;

namespace KitchIn.Core.Models
{
    public class UserPreferenceiPhoneModel
    {

        public List<string> AllowedIngridients { get; set; }

        public List<string> ExcludedIngridients { get; set; }

        public string Meal { get; set; }

        public string DishType { get; set; }

        public List<TimeType> Time { get; set; }

        public List<string> Holidays { get; set; }

        public List<string> Allergies { get; set; }

        public List<string> Diets { get; set; }

        public List<string> Cuisines { get; set; }

        public string OwnerPreference { get; set; }

   }
}
