namespace KitchIn.Core.Services.Yummly.Recipe
{
    public class NutritionEstimate
    {
        public string attribute { get; set; }
        public string description { get; set; }
        public double? value { get; set; }
        public UnitMesh unit { get; set; }
    }
}
