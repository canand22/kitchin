namespace KitchIn.Core.Models
{
    public class CategoryiPhoneModel
    {
        public long CategoryId { get; set; }

        public string Name { get; set; }

        public int CountProducts { get; set; }

        public bool HasExpired { get; set; }
    }
}