namespace KitchIn.Core.Services.Yummly
{
    public class Match
    {
        public ImageUrlsBySize[] imageUrlsBySize { get; set; }

        public string sourceDisplayName { get; set; }

        public string[] ingredients { get; set; }

        public string id { get; set; }

        public string[] smallImageUrls { get; set; }

        public string recipeName { get; set; }

        public long totalTimeInSeconds { get; set; }

        public Attributes attributes { get; set; }

        public Flavors flavors { get; set; }

        public int rating { get; set; }
    }
}
