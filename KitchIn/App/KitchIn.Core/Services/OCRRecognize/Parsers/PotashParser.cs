using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace KitchIn.Core.Services.OCRRecognize.Parsers
{
    public class PotashParser : IParser
    {
        private readonly IList<ProductParserModel> products;

        private readonly string[] lines;

        public int NumberOfItems { get; private set; }

        public PotashParser(string[] lines)
        {
            this.NumberOfItems = 0;
            this.products = new List<ProductParserModel>();
            this.lines = lines;
        }

        private void Parse()
        {
            this.FirstLevelParse();
        }

        private void FirstLevelParse()
        {
            var result = new List<string>();
            var patternLF = new Regex(@"((L)?(\s|\t)*F){1}$");
            var patternH = new Regex(@"((\s|\t)*(H|h)){1}$");
            var patternCanceled = new Regex(@"(-\d+(([ ]?)|(\.?))(\d+)?){1}$");
            var patternDouble = new Regex(@"(\d+(([ ]?)|(\.?)|(\,?))(\d+)?){1}$");
            var patternWeightingItem = new Regex(@"^\d");
            var patternBegin = new Regex(@"^(\W*)");

            for (var line = 0; line < lines.Length; line++)
            {
                var isCanceled = false;
                if (patternLF.IsMatch(lines[line]) || patternH.IsMatch(lines[line]))
                {
                    var withoutEnding = patternLF.IsMatch(lines[line])
                                  ? Regex.Replace(lines[line], patternLF.ToString(), "").Trim()
                                  : Regex.Replace(lines[line], patternH.ToString(), "").Trim();
                    var withoutBegining = patternBegin.IsMatch(withoutEnding)
                                              ? Regex.Replace(withoutEnding, patternBegin.ToString(), "")
                                              : withoutEnding;
                    string withoutPrice;
                    if (patternCanceled.IsMatch(withoutBegining))
                    {
                        isCanceled = true;
                        withoutPrice = Regex.Replace(withoutBegining, patternCanceled.ToString(), "").Trim();
                    }
                    else
                    {
                        withoutPrice = Regex.Replace(withoutBegining, patternDouble.ToString(), "").Trim();
                    }

                    string description;
                    if (patternWeightingItem.IsMatch(withoutPrice) && line != 0)
                    {
                        description = lines[line - 1].Trim();
                    }
                    else
                    {
                        description = withoutPrice;
                    }

                    if (isCanceled)
                    {
                        result.Remove(description);
                    }
                    else
                    {
                        result.Add(description);
                    }
                }
            }
            result.ForEach(x => products.Add(new ProductParserModel(){Title = x}));
        }

        private void GetQuantity()
        {
        }

        public IList<ProductParserModel> GetProducts()
        {
            this.Parse();
            this.GetQuantity();

            return this.products;
        }
    }
}