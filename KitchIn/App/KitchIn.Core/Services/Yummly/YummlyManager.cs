using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Configuration;
using KitchIn.Core.Entities;
using KitchIn.Core.Services.Yummly.Recipe;
using KitchIn.Core.Services.Yummly.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

            if (String.IsNullOrWhiteSpace(metadata["Ingredients"]) || String.IsNullOrWhiteSpace(metadata["Allergies"]) ||
                String.IsNullOrWhiteSpace(metadata["Diets"]) || String.IsNullOrWhiteSpace(metadata["Cuisine"]) ||
                String.IsNullOrWhiteSpace(metadata["Course"]) || String.IsNullOrWhiteSpace(metadata["Holiday"]))
            {
                UpdateMetadata();
            }
        }

        private string GetData(string url, string query = "", string param = "")
        {
            var appId = WebConfigurationManager.AppSettings["ApplicationId"];
            var appKey = WebConfigurationManager.AppSettings["Password"];
            var result = String.Empty;

            using (var client = new WebClient { Encoding = Encoding.UTF8 })
            {
                var str = String.Format(url + query, appId, appKey, param);
                result = client.DownloadString(str);
            }

            return result;
        }

        public void UpdateMetadata()
        {
            foreach (var item in endpoints)
            {
                try
                {
                    var result = GetData(item.Value, String.Empty);
                    var itemList = String.Empty;

                    if (!String.IsNullOrWhiteSpace(result))
                    {
                        switch (item.Key)
                        {
                            case "Ingredient":
                                result =
                                    result.Replace("set_metadata('ingredient',", String.Empty)
                                        .Replace(");", String.Empty)
                                        .Trim();

                                var ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(result);

                                if (ingredients.Any())
                                {
                                    itemList = String.Empty;
                                    ingredients.ForEach(x => itemList += x.Term.ToLower() + ",");
                                    metadata["Ingredients"] = itemList;
                                }

                                foreach (var ingredient in ingredients)
                                {
                                    var currIngredient =
                                        this.ingredientsRepository.FirstOrDefault(
                                            i => i.SearchValue.Equals(ingredient.Term.ToLower()));

                                    if (currIngredient == null)
                                    {
                                        var newIngr = new Ingredient()
                                        {
                                            SearchValue = ingredient.SearchValue,
                                            Description = ingredient.Description,
                                            Term = ingredient.Term
                                        };

                                        this.ingredientsRepository.Save(newIngr);
                                    }
                                }

                                var tmpIngr = metadata["Ingredients"];

                                var ingredientsToRemove = this.ingredientsRepository.Where(x => !tmpIngr.Contains(x.Term)).ToList();
                                foreach (var ingr in ingredientsToRemove)
                                {
                                    this.ingredientsRepository.Remove(ingr);
                                }

                                this.ingredientsRepository.SaveChanges();

                                break;

                            case "Allergy":
                                result =
                                    result.Replace("set_metadata('allergy',", String.Empty)
                                        .Replace(");", String.Empty)
                                        .Trim();

                                var allergies = JsonConvert.DeserializeObject<List<Allergy>>(result);

                                if (allergies.Any())
                                {
                                    itemList = String.Empty;
                                    allergies.ForEach(x => itemList += x.ShortDescription.ToLower() + ",");
                                    metadata["Allergies"] = itemList;
                                }

                                foreach (var allergy in allergies)
                                {
                                    var currAllergy =
                                        this.allergiesRepository.FirstOrDefault(
                                            a => a.SearchValue.Equals(allergy.ShortDescription.ToLower()));

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

                                        this.allergiesRepository.Save(newAllergy);
                                    }
                                }

                                var tmpAllergy = metadata["Allergies"];

                                var allergiesToRemove = this.allergiesRepository.Where(x => !tmpAllergy.Contains(x.ShortDescription)).ToList();
                                foreach (var allergy in allergiesToRemove)
                                {
                                    this.allergiesRepository.Remove(allergy);
                                }

                                this.allergiesRepository.SaveChanges();

                                break;

                            case "Diet":
                                result =
                                    result.Replace("set_metadata('diet',", String.Empty)
                                        .Replace(");", String.Empty)
                                        .Trim();

                                var diets = JsonConvert.DeserializeObject<List<Diet>>(result);

                                if (diets.Any())
                                {
                                    itemList = String.Empty;
                                    diets.ForEach(x => itemList += x.ShortDescription.ToLower() + ",");
                                    metadata["Diets"] = itemList;
                                }

                                foreach (var diet in diets)
                                {
                                    var currDiet =
                                        this.dietsRepository.FirstOrDefault(d => d.ShortDescription.Equals(diet.ShortDescription.ToLower()));

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

                                        this.dietsRepository.Save(newDied);
                                    }
                                }

                                var tmpDiets = metadata["Diets"];

                                var dietsToRemove = this.dietsRepository.Where(x => !tmpDiets.Contains(x.ShortDescription)).ToList();
                                foreach (var diet in dietsToRemove)
                                {
                                    this.dietsRepository.Remove(diet);
                                }

                                this.dietsRepository.SaveChanges();

                                break;

                            case "Cuisine":
                                result =
                                    result.Replace("set_metadata('cuisine',", String.Empty)
                                        .Replace(");", String.Empty)
                                        .Trim();

                                var cuisines = JsonConvert.DeserializeObject<List<Cuisine>>(result);

                                if (cuisines.Any())
                                {
                                    itemList = String.Empty;
                                    cuisines.ForEach(x => itemList += x.Name.ToLower() + ",");
                                    metadata["Cuisine"] = itemList;
                                }

                                foreach (var cuisin in cuisines)
                                {
                                    var currCuisine =
                                        this.cuisinesRepository.FirstOrDefault(c => c.Name.Equals(cuisin.Name.ToLower()));

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

                                        this.cuisinesRepository.Save(newCuisine);
                                    }
                                }

                                var tmpCuisines = metadata["Cuisine"];

                                var cuisinesToRemove = this.cuisinesRepository.Where(x => !tmpCuisines.Contains(x.Name)).ToList();
                                foreach (var cuisine in cuisinesToRemove)
                                {
                                    this.cuisinesRepository.Remove(cuisine);
                                }

                                this.cuisinesRepository.SaveChanges();

                                break;

                            case "Course":
                                result =
                                    result.Replace("set_metadata('course',", String.Empty)
                                        .Replace(");", String.Empty)
                                        .Trim();

                                var courses = JsonConvert.DeserializeObject<List<Course>>(result);

                                if (courses.Any())
                                {
                                    itemList = String.Empty;
                                    courses.ForEach(x => itemList += x.Name.ToLower() + ",");
                                    metadata["Course"] = itemList;
                                }

                                foreach (var course in courses)
                                {
                                    var currCourse =
                                        this.coursesRepository.FirstOrDefault(c => c.Name.Equals(course.Name.ToLower()));

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

                                        this.coursesRepository.Save(newCourse);
                                    }
                                }

                                var tmpCourses = metadata["Course"];

                                var coursesToRemove = this.coursesRepository.Where(x => !tmpCourses.Contains(x.Name)).ToList();
                                foreach (var course in coursesToRemove)
                                {
                                    this.coursesRepository.Remove(course);
                                }

                                this.coursesRepository.SaveChanges();

                                break;

                            case "Holiday":
                                result =
                                    result.Replace("set_metadata('holiday',", String.Empty)
                                        .Replace(");", String.Empty)
                                        .Trim();

                                var holidays = JsonConvert.DeserializeObject<List<Holiday>>(result);

                                if (holidays.Any())
                                {
                                    itemList = String.Empty;
                                    holidays.ForEach(x => itemList += x.Name.ToLower() + ",");
                                    metadata["Holiday"] = itemList;
                                }

                                foreach (var holiday in holidays)
                                {
                                    var currHoliday =
                                        this.holidaysRepository.FirstOrDefault(h => h.Name.Equals(holiday.Name.ToLower()));

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

                                        this.holidaysRepository.Save(newHoliday);
                                    }
                                }

                                var tmpHolidays = metadata["Holiday"];

                                var holidaysToRemove = this.holidaysRepository.Where(x => !tmpHolidays.Contains(x.Name)).ToList();
                                foreach (var holiday in holidaysToRemove)
                                {
                                    this.holidaysRepository.Remove(holiday);
                                }

                                this.holidaysRepository.SaveChanges();

                                break;
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }
        }

        public IEnumerable<RecipeSearchRes> Search(YummlyReqEntity entity)
        {
            var query = GenerateRequest(entity);

            var response = GetData(endpoints["Recipes"], query, String.Empty);
            var recipies = JsonConvert.DeserializeObject<RecipeSearchJson>(response);

            var rearchRes = new List<RecipeSearchRes>();

            foreach (var recipe in recipies.matches)
            {
                var tmp = JObject.Parse(recipe.ToString());

                var recIngr = tmp["ingredients"].ToArray();
                var ingredients = new string[recIngr.Length];

                for (int i = 0; i < recIngr.Length; i++)
                {
                    ingredients[i] = recIngr[i].ToString();
                }

                var recImg = tmp["smallImageUrls"].ToArray();
                var images = new string[recImg.Length];

                for (int i = 0; i < recImg.Length; i++)
                {
                    images[i] = recImg[i].ToString();
                }
                
                //Uncomment if you need calories in recipes list

                //try
                //{
                //var currec = GetRecipe(tmp["id"].ToString()); 

                double totalTime = 0;
                double rating = 0;
                Double.TryParse(tmp["totalTimeInSeconds"].ToString(),out totalTime);
                Double.TryParse(tmp["rating"].ToString(), out rating);
                
                
                var item = new RecipeSearchRes()
                {
                    Id = tmp["id"] == null ? String.Empty : tmp["id"].ToString(),
                    Ingredients = tmp["ingredients"] == null ? new string[] {} : ingredients,
                    //Kalories = currec.Nutritions["FAT_KCAL"].Item2,
                    Kalories = 0,
                    PhotoUrl = tmp["smallImageUrls"] == null ? new string[] {} : images,
                    Title = tmp["recipeName"].ToString(),
                    TotalTime = totalTime,
                    Rating = rating
                };

                rearchRes.Add(item);
                //}
                //catch
                //{
                //    var item = new RecipeSearhRes()
                //    {
                //        Id = tmp["id"],
                //        Ingredients = tmp["ingredients"] == null ? new JToken[] { } : tmp["ingredients"].ToArray(),
                //        Kalories = 0,
                //        PhotoUrl = tmp["smallImageUrls"].ToString(),
                //        Title = tmp["recipeName"].ToString(),
                //        TotalTime = tmp["totalTimeInSeconds"]
                //    };

                //    rearchRes.Add(item);
                //}
            }

            return rearchRes;
        }

        public RecipeRes GetRecipe(string id)
        {
            try
            {
                var response = GetData(endpoints["GetRecipe"], String.Empty, id);
                var recipeYummly = JsonConvert.DeserializeObject<RecipeJson>(response);

                var nutritions = recipeYummly.nutritionEstimates;
                var nutritionDic = nutritions.ToDictionary(item => item.attribute,
                    item => new Tuple<string, double?, string>(item.description, item.value, item.unit.name));

                var recipe = new RecipeRes()
                {
                    RecipeName = recipeYummly.name,
                    Ingredients = recipeYummly.ingredientLines,
                    Picture = recipeYummly.images[0].hostedMediumUrl,
                    Rating = recipeYummly.rating == null ? 0 : recipeYummly.rating.Value,
                    Served = recipeYummly.numberOfServings.ToString(),
                    RecipeUrl = recipeYummly.attribution.url,
                    Time = recipeYummly.totalTimeInSeconds == null ? 0 : recipeYummly.totalTimeInSeconds.Value,
                    Nutritions = nutritionDic
                };

                return recipe;
            }
            catch
            {
                return new RecipeRes();
            }
        }

        public static IDictionary<string, string> GetMetadata(string key = "All")
        {
            return key.ToLower().Equals("all")
                ? metadata
                : (metadata.ContainsKey(key)
                    ? new Dictionary<string, string> { { key, metadata[key] } }
                    : new Dictionary<string, string> { { "Error", "Unknown key" } });
        }

        private string GenerateRequest(YummlyReqEntity entity)
        {
            var query = "&requirePictures=true";

            if (entity.CookWith != null)
            {
                query = entity.CookWith.Where(field => metadata["Ingredients"].Contains(field)).Aggregate(query, (current, field) => current + ("&allowedIngredient[]=" + field));
            }

            if (entity.CookWithout != null)
            {
                query = entity.CookWithout.Where(field => metadata["Ingredients"].Contains(field)).Aggregate(query, (current, field) => current + ("&excludedIngredient[]=" + field));
            }

            if (entity.Allergies != null)
            {
                query = entity.Allergies.Where(field => metadata["Allergies"].Contains(field)).Aggregate(query, (current, field) => current + ("&allowedAllergy[]=" + field));
            }

            if (entity.Meal != null)
            {
                foreach (var field in entity.Meal)
                {
                    switch (field)
                    {
                        case "Breakfast&Brunch":
                            query += "&allowedCourse[]=course^course-Breakfast + course^course-Brunch";
                            break;
                        case "Dinner":
                            query += "&allowedCourse[]=course^course-Main Dishes+course^course-Side%20Dishes";
                            break;
                        case "Lunch& Snack":
                            query += "&allowedCourse[]=course^course-Lunch%20and%20Snacks";
                            break;
                    }
                }
            }

            if (entity.Cuisine != null)
            {
                query = entity.Cuisine.Where(field => metadata["Cuisine"].Contains(field))
                    .Aggregate(query, (current, field) => current + ("&allowedCuisine[]=" + field));
            }

            if (entity.Holiday != null)
            {
                query = entity.Holiday.Where(field => metadata["Holiday"].Contains(field))
                    .Aggregate(query, (current, field) => current + ("&allowedHoliday[]=" + field));
            }

            if (entity.DishType != null)
            {
                query = entity.DishType.Where(field => metadata["Course"].Contains(field))
                    .Aggregate(query, (current, field) => current + ("&allowedCourse[]=" + field));
            }

            if (entity.Diets != null)
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
                    case "900":
                        query += "&maxTotalTimeInSeconds=900";
                        break;
                    case "Less than 30 mins":
                    case "1800":
                        query += "&maxTotalTimeInSeconds=1800";
                        break;
                    case "Less than 45 mins":
                    case "2700":
                        query += "&maxTotalTimeInSeconds=2700";
                        break;
                    case "Less than 1 hour":
                    case "3600":
                        query += "&maxTotalTimeInSeconds=3600";
                        break;
                    case "Less than 2 hours":
                    case "7200":
                        query += "&maxTotalTimeInSeconds=7200";
                        break;
                    case "Less than 3 hours":
                    case "1080":
                        query += "&maxTotalTimeInSeconds=10800";
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
    }
}
