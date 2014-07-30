using System;
using System.Linq;
using System.Text;
using KitchIn.Core.Enums;
using SmartArch.Core.Domain.Base;
using Iesi.Collections.Generic;
using System.Collections.ObjectModel;

namespace KitchIn.Core.Entities
{
    public class UserPreference : BaseEntity
    {
        public virtual Iesi.Collections.Generic.ISet<Allergy> Allergies{get; set;}
        public virtual Iesi.Collections.Generic.ISet<Ingredient> AllowedIngredients { get; set; }
        public virtual Iesi.Collections.Generic.ISet<Ingredient> ExcludedIngredients { get; set; }
        public virtual Iesi.Collections.Generic.ISet<Holiday> Holidays { get; set; }
        public virtual Iesi.Collections.Generic.ISet<Diet> Diets { get; set; }
        public virtual Iesi.Collections.Generic.ISet<Cuisine> Cuisines { get; set; }

        public UserPreference()
        {
            Allergies = new HashedSet<Allergy>();
            AllowedIngredients = new HashedSet<Ingredient>();
            ExcludedIngredients = new HashedSet<Ingredient>();
            Holidays = new HashedSet<Holiday>();
            Diets = new HashedSet<Diet>();
            Cuisines = new HashedSet<Cuisine>();
        }
        public virtual string Meal { get; set; }

        public virtual Course DishType { get; set; }

        public virtual string Time { get; set; }

        public virtual User User { get; set; }

        public virtual string OwnerPreference { get; set; }
        #region 'Add' methods
        public virtual void AddAllergy(Allergy allergy)
        {
            if (allergy == null)
                throw new ArgumentNullException("allergy");
            if (!Allergies.Contains(allergy))
            {
                Allergies.Add(allergy);
                allergy.AddUserPreference(this);
            }
        }
        public virtual void AddDiet(Diet diet)
        {
            if (diet == null)
                throw new ArgumentNullException("diet");
            if (!Diets.Contains(diet))
            {
                Diets.Add(diet);
                diet.AddUserPreference(this);
            }
        }
        public virtual void AddAllowedIngredient(Ingredient allowedIngredient)
        {
            if (allowedIngredient == null)
                throw new ArgumentNullException("allowedIngredient");
            if (!AllowedIngredients.Contains(allowedIngredient))
            {
                AllowedIngredients.Add(allowedIngredient);
                allowedIngredient.AddUserPreference(this, true);
            }
        }
        public virtual void AddExcludedIngredient(Ingredient excludedIngredient)
        {
            if (excludedIngredient == null)
                throw new ArgumentNullException("excludedIngredient");
            if (!ExcludedIngredients.Contains(excludedIngredient))
            {
                ExcludedIngredients.Add(excludedIngredient);
                excludedIngredient.AddUserPreference(this, false);
            }
        }
        public virtual void AddHoliday(Holiday holiday)
        {
            if (holiday == null)
                throw new ArgumentNullException("holiday");
            if (!Holidays.Contains(holiday))
            {
                Holidays.Add(holiday);
                holiday.AddUserPreference(this);
            }
        }
        public virtual void AddCuisine(Cuisine cuisine)
        {
            if (cuisine == null)
                throw new ArgumentNullException("cuisine");
            if (!Cuisines.Contains(cuisine))
            {
                Cuisines.Add(cuisine);
                cuisine.AddUserPreference(this);
            }
        }
        
#endregion
        #region 'Remove' methods
        public virtual void RemoveAllergy(Allergy allergy)
        {
            if (allergy == null)
                throw new ArgumentNullException("allergy");
            if (Allergies.Contains(allergy))
            {
                Allergies.Remove(allergy);
                allergy.RemoveUserPreference(this);
            }
        }
       public virtual void RemoveDiet(Diet diet)
        {
            if (diet == null)
                throw new ArgumentNullException("diet");
            if (Diets.Contains(diet))
            {
                Diets.Remove(diet);
                diet.RemoveUserPreference(this);
            }
        }
       public virtual void RemoveAllowedIngredient(Ingredient allowedIngredient)
        {
            if (allowedIngredient == null)
                throw new ArgumentNullException("allowedIngredient");
            if (AllowedIngredients.Contains(allowedIngredient))
            {
                AllowedIngredients.Remove(allowedIngredient);
                allowedIngredient.RemoveUserPreference(this, true);
            }
        }
        public virtual void RemoveExcludedIngredient(Ingredient excludedIngredient)
        {
            if (excludedIngredient == null)
                throw new ArgumentNullException("excludedIngredient");
            if (ExcludedIngredients.Contains(excludedIngredient))
            {
                ExcludedIngredients.Remove(excludedIngredient);
                excludedIngredient.RemoveUserPreference(this, false);
            }
        }
        public virtual void RemoveHoliday(Holiday holiday)
        {
            if (holiday == null)
                throw new ArgumentNullException("holiday");
            if (Holidays.Contains(holiday))
            {
                Holidays.Remove(holiday);
                holiday.RemoveUserPreference(this);
            }
        }
        public virtual void RemoveCuisine(Cuisine cuisine)
        {
            if (cuisine == null)
                throw new ArgumentNullException("cuisine");
            if (Cuisines.Contains(cuisine))
            {
                Cuisines.Remove(cuisine);
                cuisine.RemoveUserPreference(this);
            }
        }

        #endregion
    }
}
