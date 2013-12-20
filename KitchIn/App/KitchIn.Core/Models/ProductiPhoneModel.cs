namespace KitchIn.Core.Models
{
    public class ProductiPhoneModel
    {
        public string Name { get; set; }

        public long? ProductId { get; set; }

        public long? CategoryId { get; set; }

        public double Quantity { get; set; }

        public bool HasExpired { get; set; } 
    }
}