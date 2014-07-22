using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Configuration;
using Iesi.Collections;
using Iesi.Collections.Generic;
using KitchIn.Core.Entities;
using Newtonsoft.Json;
using SmartArch.Data;

namespace KitchIn.Core.Services.Yummly
{
    public class YummlyManager : IYummly
    {
        private IDictionary<string, string> endpoints;

        private static IDictionary<string, string> metadata = new Dictionary<string, string>()
            {
                {"Ingredients", String.Empty},
                {"Allergies", String.Empty},
                {"Diets", String.Empty},
                {"Cuisine", String.Empty},
                {"Course", String.Empty},
                {"Holiday", String.Empty}
            };

        private IRepository<Ingredient> ingredientsRepository;
        private IRepository<Course> coursesRepository;
        private IRepository<Holiday> holidaysRepository;
        private IRepository<Diet> dietsRepository;
        private IRepository<Cuisine> cuisinesRepository;
        private IRepository<Allergy> allergiesRepository;

        public YummlyManager(IRepository<Ingredient> ingredientsRepository,
                             IRepository<Course> coursesRepository,
                             IRepository<Holiday> holidaysRepository,
                             IRepository<Diet> dietsRepository,
                             IRepository<Cuisine> cuisinesRepository,
                             IRepository<Allergy> allergiesRepository)
        {
            this.ingredientsRepository = ingredientsRepository;
            this.coursesRepository = coursesRepository;
            this.holidaysRepository = holidaysRepository;
            this.dietsRepository = dietsRepository;
            this.cuisinesRepository = cuisinesRepository;
            this.allergiesRepository = allergiesRepository;

            endpoints = new Dictionary<string, string>
            {
                {"Ingredient", "http://api.yummly.com/v1/api/metadata/ingredient?_app_id={0}&_app_key={1}{2}"},
                {"Allergy", "http://api.yummly.com/v1/api/metadata/allergy?_app_id={0}&_app_key={1}{2}"},
                {"Diet", "http://api.yummly.com/v1/api/metadata/diet?_app_id={0}&_app_key={1}{2}"},
                {"Cuisine", "http://api.yummly.com/v1/api/metadata/cuisine?_app_id={0}&_app_key={1}{2}"},
                {"Course", "http://api.yummly.com/v1/api/metadata/course?_app_id={0}&_app_key={1}{2}"},
                {"Holiday", "http://api.yummly.com/v1/api/metadata/holiday?_app_id={0}&_app_key={1}{2}"},
                {"Recipes", "http://api.yummly.com/v1/api/recipes?_app_id={0}&_app_key={1}{2}"},
                {"GetRecipe", "http://api.yummly.com/v1/api/recipe/{2}?_app_id={0}&_app_key={1}"}
            };
        }

        private string GetData(string url, string query = "", string param = "")
        {
            var appId = WebConfigurationManager.AppSettings["ApplicationId"];
            var appKey = WebConfigurationManager.AppSettings["Password"];
            var result = String.Empty;

            try
            {
                using (var client = new WebClient { Encoding = Encoding.UTF8 })
                {
                    result = client.DownloadString(String.Format(url + query, appId, appKey, param)).Replace("\\", String.Empty);
                }

                return result;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        public void UpdateMetadata()
        {
            foreach (var item in endpoints)
            {
                var result = GetData(item.Value, String.Empty);
                var itemList = String.Empty;

                if (!String.IsNullOrWhiteSpace(result))
                {
                    switch (item.Key)
                    {
                        case "Ingredient":
                            var ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(result);

                            foreach (var ingredient in ingredients)
                            {
                                var currIngredient =
                                    this.ingredientsRepository.FirstOrDefault(
                                        i => i.SearchValue.Equals(ingredient.SearchValue));

                                if (currIngredient == null)
                                {
                                    var newIngr = new Ingredient()
                                    {
                                        SearchValue = ingredient.SearchValue,
                                        Description = ingredient.Description,
                                        Term = ingredient.Term
                                    };

                                    ingredientsRepository.Save(newIngr);
                                }
                            }

                            if (ingredients.Any())
                            {
                                itemList = String.Empty;
                                ingredients.ForEach(x => itemList += x.Term + " ");
                                metadata["Ingredients"] = itemList;
                            }

                            break;

                        case "Allergy":
                            var allergies = JsonConvert.DeserializeObject<List<Allergy>>(result);

                            foreach (var allergy in allergies)
                            {
                                var currAllergy = allergiesRepository.FirstOrDefault(a => a.SearchValue.Equals(allergy.SearchValue));

                                if (currAllergy == null)
                                {
                                    var newAllergy = new Allergy()
                                    {
                                        SearchValue = currAllergy.SearchValue,
                                        ShortDescription = currAllergy.ShortDescription,
                                        LongDescription = currAllergy.LongDescription,
                                        Type = currAllergy.Type,
                                        LocalesAvailableIn = currAllergy.LocalesAvailableIn
                                    };

                                    allergiesRepository.Save(newAllergy);
                                }
                            }

                            if (allergies.Any())
                            {
                                itemList = String.Empty;                                
                                allergies.ForEach(x => itemList += x.SearchValue + " ");
                                metadata["Allergies"] = itemList;
                            }

                            break;

                        case "Diet":
                            var diets = JsonConvert.DeserializeObject<List<Diet>>(result);

                            foreach (var diet in diets)
                            {
                                var currDiet = dietsRepository.FirstOrDefault(d => d.SearchValue.Equals(diet.SearchValue));

                                if (currDiet == null)
                                {
                                    var newDied = new Diet()
                                    {
                                        SearchValue = diet.SearchValue,
                                        ShortDescription = diet.ShortDescription,
                                        LongDescription = diet.LongDescription,
                                        Type = diet.Type,
                                        LocalesAvailableIn = diet.LocalesAvailableIn
                                    };

                                    dietsRepository.Save(newDied);
                                }
                            }

                            if (diets.Any())
                            {
                                itemList = String.Empty;
                                diets.ForEach(x => itemList += x.SearchValue + " ");
                                metadata["Diets"] = itemList;
                            }

                            break;

                        case "Cuisine":
                            var cuisines = JsonConvert.DeserializeObject<List<Cuisine>>(result);

                            foreach (var cuisin in cuisines)
                            {
                                var currCuisine = cuisinesRepository.FirstOrDefault(c => c.SearchValue.Equals(cuisin.SearchValue));

                                if (currCuisine == null)
                                {
                                    var newCuisine = new Cuisine()
                                    {
                                        SearchValue = cuisin.SearchValue,
                                        Name = cuisin.Name,
                                        Description = cuisin.Description,
                                        Type = cuisin.Type,
                                        LocalesAvailableIn = cuisin.LocalesAvailableIn
                                    };

                                    cuisinesRepository.Save(newCuisine);
                                }
                            }

                            if (cuisines.Any())
                            {
                                itemList = String.Empty;
                                cuisines.ForEach(x => itemList += x.SearchValue + " ");
                                metadata["Cuisine"] = itemList;
                            }

                            break;

                        case "Course":
                            var courses = JsonConvert.DeserializeObject<List<Course>>(result);

                            foreach (var course in courses)
                            {
                                var currCourse = coursesRepository.FirstOrDefault(c => c.SearchValue.Equals(course.SearchValue));

                                if (currCourse == null)
                                {
                                    var newCourse = new Course()
                                    {
                                        SearchValue = course.SearchValue,
                                        Name = course.Name,
                                        Description = course.Description,
                                        Type = course.Type,
                                        LocalesAvailableIn = course.LocalesAvailableIn
                                    };

                                    coursesRepository.Save(newCourse);
                                }
                            }

                            if (courses.Any())
                            {
                                itemList = String.Empty;
                                courses.ForEach(x => itemList += x.SearchValue + " ");
                                metadata["Courses"] = itemList;
                            }

                            break;

                        case "Holiday":
                            var holidays = JsonConvert.DeserializeObject<List<Holiday>>(result);

                            foreach (var holiday in holidays)
                            {
                                var currHoliday = holidaysRepository.FirstOrDefault(h => h.SearchValue.Equals(holiday.SearchValue));

                                if (currHoliday == null)
                                {
                                    var newHoliday = new Holiday()
                                    {
                                        SearchValue = holiday.SearchValue,
                                        Name = holiday.Name,
                                        Description = holiday.Description,
                                        Type = holiday.Type,
                                        LocalesAvailableIn = holiday.LocalesAvailableIn
                                    };

                                    holidaysRepository.Save(newHoliday);
                                }
                            }

                            if (holidays.Any())
                            {
                                itemList = String.Empty;
                                holidays.ForEach(x => itemList += x.SearchValue + " ");
                                metadata["Holidays"] = itemList;
                            }

                            break;
                    }
                }
            }
        }

        public RecipeSearchJson SearchRecipes(string request)
        {
            try
            {
                var entity = JsonConvert.DeserializeObject<YummlyReqEntity>(request);
                var query = GenerateRequest(entity);
                var response = GetData(endpoints["Recipes"], query, String.Empty);
                var recipies = JsonConvert.DeserializeObject<RecipeSearchJson>(response);

                return recipies;
            }
            catch
            {
                return null;
            }
        }



        private string GenerateRequest(YummlyReqEntity entity)
        {
            var query = "&requirePictures=true";

            if (entity.CookWith.Any())
            {
                query = entity.CookWith.Where(field => metadata["Ingredients"].Contains(field)).Aggregate(query, (current, field) => current + ("&allowedIngredient[]=" + field));
            }

            if (entity.CookWithout.Any())
            {
                query = entity.CookWith.Where(field => metadata["Ingredients"].Contains(field)).Aggregate(query, (current, field) => current + ("&excludedIngredient[]=" + field));
            }

            if (entity.Allergies.Any())
            {
                query = entity.Allergies.Where(field => metadata["Allergies"].Contains(field)).Aggregate(query, (current, field) => current + ("&allowedAllergy[]=" + field));
            }

            if (entity.Meal.Any())
            {
                foreach (var field in entity.Meal)
                {
                    switch (field)
                    {
                        case "Breakfast&Brunch":
                            query += "&allowedCourse [ ] = course^course- Breakfast + course^course- Brunch";
                            break;
                        case "Dinner":
                            query += "&allowedCourse [ ] = course^course-Main Dishes+course^course-Side Dishes";
                            break;
                        case "Lunch& Snack":
                            query += "&allowedCourse [ ] = course^course- Lunch and Snacks";
                            break;
                    }
                }
            }

            if (entity.Cuisine.Any())
            {
                query = entity.Cuisine.Where(field => metadata["Cuisine"].Contains(field)).Aggregate(query, (current, field) => current + ("&allowedCuisine[]=" + field));
            }

            if (entity.Holiday.Any())
            {
                query = entity.Holiday.Where(field => metadata["Holiday"].Contains(field)).Aggregate(query, (current, field) => current + ("&allowedHoliday[]=" + field));
            }

            if (entity.DishType.Any())
            {
                query = entity.DishType.Where(field => metadata["Course"].Contains(field)).Aggregate(query, (current, field) => current + ("&allowedCourse[]=" + field));
            }

            if (entity.Diets.Any())
            {
                foreach (var field in entity.Diets)
                {
                    switch (field)
                    {
                        case "Vegan":
                            query += "&allowedDiet[]=Vegan";
                            break;
                        case "Vegetarian":
                            query += "&allowedDiet[]=Vegetarian";
                            break;
                        case "Lacto Vegetarian":
                            query += "&allowedDiet[]=Lacto%20Vegetarian";
                            break;
                        case "Ovo Vegetarian ":
                            query += "&allowedDiet[]=Ovo%20Vegetarian";
                            break;
                        case "Pescetarian ":
                            query += "&allowedDiet[]=Pescetarian";
                            break;
                    }
                }
            }

            if (!String.IsNullOrWhiteSpace(entity.Time))
            {
                switch (entity.Time)
                {
                    case "Less than 15 mins":
                        query += "&maxTotalTimeInSeconds[]=900";
                        break;
                    case "Less than 30 mins":
                        query += "&maxTotalTimeInSeconds[]=1800";
                        break;
                    case "Less than 45 mins":
                        query += "&maxTotalTimeInSeconds[]=2700";
                        break;
                    case "Less than 1 hour":
                        query += "&maxTotalTimeInSeconds[]=3600";
                        break;
                    case "Less than 2 hours":
                        query += "&maxTotalTimeInSeconds[]=7200";
                        break;
                    case "Less than 3 hours":
                        query += "&maxTotalTimeInSeconds[]=10800";
                        break;
                    case "Low-Calorie":
                        query += "&nutrition.Energ_kcal.min=100&nutrition.Energ_kcal.max=250";
                        break;
                    case "Low-Fat":
                        query += "&nutrition.fatl.min=1&nutrition.fat.max=15";
                        break;
                    case "Low-Carbohydrate":
                        query += "&nutrition.CHOCDF.min=10&nutrition.CHOCDF.max=30";
                        break;
                    case "Low-Sodium":
                        query += "&nutrition.NA.min=10&nutrition.NA.max=30";
                        break;
                    case "High-Fiber":
                        query += "&nutrition.NA.min=20&nutrition.NA.max=50";
                        break;
                }
            }

            return query;
        }

        public Recipe GetRecipe(string id)
        {
            var response = GetData(endpoints["GetRecipe"], String.Empty, id);
            var recipies = JsonConvert.DeserializeObject<RecipeJson>(response);

            return new Recipe();
        };

        public IDictionary<string, string> GetMetadata()
        {
            return metadata;
        }
    }
}
