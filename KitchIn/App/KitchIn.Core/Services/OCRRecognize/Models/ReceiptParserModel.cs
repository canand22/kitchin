using KitchIn.Core.Enums;

namespace KitchIn.Core.Services.OCRRecognize.Models
{
    public class ReceiptParserModel
    {
        public MarketTypes Market { get; set; }

        public string FilePath { get; set; }
    }
}