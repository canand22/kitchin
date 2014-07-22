namespace KitchIn.Core.Services.Yummly
{
    public class RecipeSearchJson
    {

        public Attribution attribution { get; set; }

        public int totalMatchCount { get; set; }

        public FacetCounts facetCounts { get; set; }

        public Match[] matches { get; set; }

        public Criteria criteria { get; set; }

    }
}
