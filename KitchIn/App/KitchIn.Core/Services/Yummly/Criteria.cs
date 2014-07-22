namespace KitchIn.Core.Services.Yummly
{
    public class Criteria
    {
        public string[] excludedIngredients { get; set; }

        public string[] allowedIngredients { get; set; }

        public string[] terms { get; set; }

        public string requirePictures { get; set; }
    }
}
