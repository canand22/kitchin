using Newtonsoft.Json;

namespace KitchIn.Core.Services.Yummly.Recipe
{
    public class RecipeJson
    {
        public string yield { get; set; }

        public NutritionEstimate[] nutritionEstimates { get; set; }

        public string totalTime { get; set; }

        public Images[] images { get; set; }

        public string name { get; set; }

        public Source source { get; set; }

        public string id { get; set; }

        public string[] ingredientLines { get; set; }

        public Attribution attribution { get; set; }

        public int? numberOfServings { get; set; }

        public int? totalTimeInSeconds { get; set; }

        public Attributes attributes { get; set; }

        public Flavors flavors { get; set; }

        public int? rating { get; set; }
    }

}
