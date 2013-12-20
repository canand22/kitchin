using System.Collections.Generic;
using KitchIn.Core.Enums;
using KitchIn.Core.Services.OCRRecognize.Models;

namespace KitchIn.Core.Services.OCRRecognize.Parsers
{
    public class TraderJoesParser : IParser
    {
        private readonly IList<ProductParserModel> products;
        private readonly string[] lines;

        public TraderJoesParser()
        {
            this.NumberOfItems = 0;
            this.products = new List<ProductParserModel>();
            this.lines = new ReceiptsData().GetLines(MarketTypes.TraderJoes);
        }

        private void Parse()
        {
            this.FirstLevelParse();
            this.SecondLevelParse();
        }

        private void FirstLevelParse()
        {
        }

        private void SecondLevelParse()
        {
        }

        private void SetQuantity()
        {
        }

        public IList<ProductParserModel> GetProducts()
        {
            this.Parse();
            this.SetQuantity();

            return this.products;
        }

        public int NumberOfItems { get; private set; }
    }
}