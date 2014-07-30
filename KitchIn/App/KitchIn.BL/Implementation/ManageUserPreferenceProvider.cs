using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using KitchIn.Core.Entities;
using KitchIn.Core.Enums;
using KitchIn.Core.Interfaces;
using KitchIn.Core.Models;
using SmartArch.Data;
using Microsoft.Practices.ServiceLocation;
using Iesi.Collections.Generic;
using System.Collections.Generic;

namespace KitchIn.BL.Implementation
{
    public class ManageUserPreferenceProvider : BaseProvider, IManageUserPreferenceProvider
    {
        private readonly IRepository<UserPreference> userPreferenceRepo;

        private readonly IRepository<Allergy> allergyRepo;

        private readonly IRepository<Diet> dietRepo;

        private readonly IRepository<Ingredient> ingredientRepo;

        private readonly IRepository<Cuisine> cuisineRepo;

        private readonly IRepository<Holiday> holidayRepo;

        private readonly IRepository<Course> courseRepo;

        private const string MESSAGE_USER_FAIL = "User isn't found";
        private const string MESSAGE_USERPREFERENCE_FAIL = "DB doesn't contain current preference";

        public ManageUserPreferenceProvider(IRepository<UserPreference> userPreferenceRepo,
            IRepository<Allergy> allergyRepo, IRepository<Diet> dietRepo, IRepository<Ingredient> ingredientRepo,
            IRepository<Cuisine> cuisineRepo, IRepository<Holiday> holidayRepo, IRepository<Course> courseRepo)
        {
            this.userPreferenceRepo = userPreferenceRepo;
            this.allergyRepo = allergyRepo;
            this.dietRepo = dietRepo;
            this.ingredientRepo = ingredientRepo;
            this.cuisineRepo = cuisineRepo;
            this.holidayRepo = holidayRepo;
            this.courseRepo = courseRepo;
        }

        public string AddOrUpdateUserPreference(Guid id, UserPreferenceiPhoneModel model)
        {
            var user = this.GetUser(id);
            if (user == null)
            {
                return MESSAGE_USER_FAIL;
            }

            try
            {
                if (
                     userPreferenceRepo.Where(p => p.User == user)
                     .FirstOrDefault(p => p.OwnerPreference == (model.OwnerPreference == null ? user.FirstName : model.OwnerPreference)) != null)
                {
                    var previousUsPref = this.userPreferenceRepo.Where(p => p.User == user)
                     .FirstOrDefault(p => p.OwnerPreference == (model.OwnerPreference == null ? user.FirstName : model.OwnerPreference));
                    var updatedUsPref = UpdateUserPreference(model, previousUsPref);
                    this.userPreferenceRepo.SaveChanges();
                    return CommonConstants.MESSAGE_SUCCESS;
                }

                this.userPreferenceRepo.Save(CreateUserPreference(user, model));
                this.userPreferenceRepo.SaveChanges();
                return CommonConstants.MESSAGE_SUCCESS;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("Violation of PRIMARY KEY constraint"))
                {
                    return CommonConstants.MESSAGE_SUCCESS;
                }
                return ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

        }

        private UserPreference CreateUserPreference(User user, UserPreferenceiPhoneModel model)
        {
            UserPreference newUsPref = new UserPreference();
            try
            {
                if (model.Allergies != null)
                {
                    foreach (var val in model.Allergies)
                    {
                        newUsPref.AddAllergy(allergyRepo.FirstOrDefault(p => p.SearchValue == val));
                    }
                }
                if (model.AllowedIngridients != null)
                {
                    foreach (var val in model.AllowedIngridients)
                    {
                        newUsPref.AddAllowedIngredient(ingredientRepo.FirstOrDefault(p => p.Term == val));
                    }
                }
                if (model.ExcludedIngridients != null)
                {
                    foreach (var val in model.ExcludedIngridients)
                    {
                        newUsPref.AddExcludedIngredient(ingredientRepo.FirstOrDefault(p => p.Term == val));
                    }
                }
                if (model.Holidays != null)
                {
                    foreach (var val in model.Holidays)
                    {
                        newUsPref.AddHoliday(holidayRepo.FirstOrDefault(p => p.Name == val));
                    }
                }
                if (model.Diets != null)
                {
                    foreach (var val in model.Diets)
                    {
                        newUsPref.AddDiet(dietRepo.FirstOrDefault(p => p.ShortDescription == val));
                    }
                }
                if (model.Cuisines != null)
                {
                    foreach (var val in model.Cuisines)
                    {
                        newUsPref.AddCuisine(cuisineRepo.FirstOrDefault(p => p.Name == val));
                    }
                }
                if (model.DishType != null)
                {
                    newUsPref.DishType = courseRepo.FirstOrDefault(p => p.Name == model.DishType);
                }
                newUsPref.Meal = model.Meal;
                newUsPref.Time = model.Time.Max().ToString();
                newUsPref.OwnerPreference = model.OwnerPreference == null ? user.FirstName : model.OwnerPreference;
                newUsPref.User = user;
                return newUsPref;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private UserPreference UpdateUserPreference(UserPreferenceiPhoneModel model, UserPreference previousUsPref,bool isRemove=false)
        {
            #region UpdateAllergies
            model.Allergies = model.Allergies == null ? new System.Collections.Generic.List<string>() : model.Allergies;
            var removedAllergies = previousUsPref.Allergies.Where(p => model.Allergies.Count(t => t == p.SearchValue) == 0).ToList();
            foreach (var allergy in removedAllergies)
            {
                previousUsPref.RemoveAllergy(allergy);
            }
            List<string> addedAllergies = model.Allergies.Where(t => previousUsPref.Allergies.Count(p => p.SearchValue == t) == 0).ToList();
            foreach (var allergy in addedAllergies)
            {
                previousUsPref.AddAllergy(this.allergyRepo.FirstOrDefault(p => p.SearchValue == allergy));
            }
            #endregion

            #region UpdateDiets
            model.Diets = model.Diets == null ? new System.Collections.Generic.List<string>() : model.Diets;
            var removedDiets = previousUsPref.Diets.Where(p => model.Diets.Count(t => t == p.ShortDescription) == 0).ToList();
            foreach (var diet in removedDiets)
            {
                previousUsPref.RemoveDiet(diet);
            }
            var addedDiets = model.Diets.Where(t => previousUsPref.Diets.Count(p => p.ShortDescription == t) == 0);
            foreach (var diet in addedDiets)
            {
                previousUsPref.AddDiet(this.dietRepo.FirstOrDefault(p => p.ShortDescription == diet));
            }
            #endregion

            #region UpdateAllowedIngridients
            model.AllowedIngridients = model.AllowedIngridients == null ? new System.Collections.Generic.List<string>() : model.AllowedIngridients;
             var removedAllowedIngridients = previousUsPref.AllowedIngredients.Where(p => model.AllowedIngridients.Count(t => t == p.Term) == 0).ToList();
             foreach (var allowedIngridient in removedAllowedIngridients)
            {
                previousUsPref.RemoveAllowedIngredient(allowedIngridient);
            }
            var addedAllowedIngridients = model.AllowedIngridients.Where(t => previousUsPref.AllowedIngredients.Count(p => p.Term == t) == 0);
            foreach (var allowedIngridient in addedAllowedIngridients)
            {
                previousUsPref.AddAllowedIngredient(this.ingredientRepo.FirstOrDefault(p => p.Term == allowedIngridient));
            }
            #endregion

            #region UpdateExcludedIngridients
            model.ExcludedIngridients = model.ExcludedIngridients == null ? new System.Collections.Generic.List<string>() : model.ExcludedIngridients;
            var removedExcludedIngridients = previousUsPref.ExcludedIngredients.Where(p => model.ExcludedIngridients.Count(t => t == p.Term) == 0).ToList();
            foreach (var excludedIngridient in removedExcludedIngridients)
            {
                previousUsPref.RemoveExcludedIngredient(excludedIngridient);
            }
            var addedExcludedIngridients = model.ExcludedIngridients.Where(t => previousUsPref.ExcludedIngredients.Count(p => p.Term == t) == 0);
            foreach (var excludedIngridient in addedExcludedIngridients)
            {
                previousUsPref.AddExcludedIngredient(this.ingredientRepo.FirstOrDefault(p => p.Term == excludedIngridient));
            }
            #endregion

            #region UpdateHolidays
            model.Holidays = model.Holidays == null ? new System.Collections.Generic.List<string>() : model.Holidays;
            var removedHolidays=previousUsPref.Holidays.Where(p => model.Holidays.Count(t => t == p.Name) == 0).ToList();
            foreach (var holiday in removedHolidays)
            {
                previousUsPref.RemoveHoliday(holiday);
            }
            var addedHolidays = model.Holidays.Where(t => previousUsPref.Holidays.Count(p => p.Name == t) == 0);
            foreach (var holiday in addedHolidays)
            {
                previousUsPref.AddHoliday(this.holidayRepo.FirstOrDefault(p => p.Name == holiday));
            }
            #endregion

            #region UpdateCuisines
            model.Cuisines = model.Cuisines == null ? new System.Collections.Generic.List<string>() : model.Cuisines;
            var removedCuisines=previousUsPref.Cuisines.Where(p => model.Cuisines.Count(t => t == p.Name) == 0).ToList();
            foreach (var cuisine in removedCuisines)
            {
                previousUsPref.RemoveCuisine(cuisine);
            }
            var addedCuisines = model.Cuisines.Where(t => previousUsPref.Cuisines.Count(p => p.Name == t) == 0);
            foreach (var cuisine in addedCuisines)
            {
                previousUsPref.AddCuisine(this.cuisineRepo.FirstOrDefault(p => p.Name == cuisine));
            }
            #endregion
            if (!isRemove)
            {
                if (previousUsPref.DishType == null || previousUsPref.DishType.Name != model.DishType)
                {
                    previousUsPref.DishType = this.courseRepo.FirstOrDefault(p => p.Name == model.DishType);
                }
                previousUsPref.Meal = previousUsPref.Meal != model.Meal ? model.Meal : previousUsPref.Meal;
                if (model.Time == null)
                {
                    model.Time = new List<TimeType> { TimeType.Any };
                }
                previousUsPref.Time = previousUsPref.Time != model.Time.Max().ToString() ? model.Time.Max().ToString() : previousUsPref.Time;
            }
            return previousUsPref;
        }

        public string RemoveUserPreference(Guid id, UserPreferenceiPhoneModel model)
        {
            var user = this.GetUser(id);
            if (user == null)
            {
                return MESSAGE_USER_FAIL;
            }

            try
            {
                if (
                     userPreferenceRepo.Where(p => p.User == user)
                     .FirstOrDefault(p => p.OwnerPreference == (model.OwnerPreference == null ? user.FirstName : model.OwnerPreference)) != null)
                {
                    var previousUsPref = this.userPreferenceRepo.Where(p => p.User == user)
                     .FirstOrDefault(p => p.OwnerPreference == (model.OwnerPreference == null ? user.FirstName : model.OwnerPreference));

                    var updatedUsPref = UpdateUserPreference(new UserPreferenceiPhoneModel(), previousUsPref,true);
                    this.userPreferenceRepo.SaveChanges();
                    this.userPreferenceRepo.Remove(previousUsPref.Id);
                    this.userPreferenceRepo.SaveChanges();
                    return CommonConstants.MESSAGE_SUCCESS;
                }
                else
                    return MESSAGE_USERPREFERENCE_FAIL;
            }
            catch (Exception ex)
            {
                return ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

        }
    }

}
