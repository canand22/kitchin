using KitchIn.Core.Enums;

namespace KitchIn.Core.Services
{
    public class ProductParserModel
    {
        public string Title { get; set; }

        public string Volume { get; set; }

        public string Quantity { get; set; }

        public UnitType Unit { get; set; } 
    }
}