using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using KitchIn.Core.Enums;

namespace KitchIn.Core.Services.OCRRecognize.Models
{
    public class ReceiptsData
    {
        private const string PatternDominick = @"Dominick";
        private const string PatternWholeFoods = @"WholeFoodR";
        private const string PatternJewlOsco = @"JewelOscoR";
        private const string PatternTrader = @"Trader";

        public IList<ReceiptParserModel> Receipts { get; set; }

        public ReceiptsData()
        {
            this.Receipts = new List<ReceiptParserModel>();
            this.SetListPath();
        }

        private void SetListPath()
        {
            var dominick = new Regex(PatternDominick);
            var whole = new Regex(PatternWholeFoods);
            var jewel = new Regex(PatternJewlOsco);
            var trader = new Regex(PatternTrader);
            var files = Directory.GetFiles(HttpContext.Current.Server.MapPath(@"~\KitchIn.WCF\Content\temp"), "*.txt");
            foreach (var file in files)
            {
                this.Receipts.Add(new ReceiptParserModel
                {
                    FilePath = file,
                    Market = dominick.IsMatch(file)
                       ? MarketTypes.Dominicks
                       : whole.IsMatch(file)
                           ? MarketTypes.WholeFoods
                           : trader.IsMatch(file)
                              ? MarketTypes.TraderJoes
                              : jewel.IsMatch(file) 
                                ? MarketTypes.JewelOsco 
                                : MarketTypes.None
                });
            }
        }

        public string CurrentPath(MarketTypes market)
        {
            return this.Receipts.Single(x => x.Market == market).FilePath;
        }

        public string[] GetLines(MarketTypes market)
        {
            return File.ReadAllLines(this.CurrentPath(market));
        }
    }
}