using KitchIn.Core.Enums;

namespace KitchIn.Core.Models
{
    public class ProductModel
    {
        public string Name { get; set; }

        public long? ProductId { get; set; }

        public string Description { get; set; }

        public double Quantity { get; set; }

        public UnitType UnitType { get; set; }

        public long? CategoryId { get; set; }

        public int? ExpirateDate { get; set; }
    }
}
