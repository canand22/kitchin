using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using KitchIn.Core.Enums;
using KitchIn.Core.Services.OCRRecognize.Models;

namespace KitchIn.Core.Services.OCRRecognize.Parsers
{
    public class JewelOscoParser : IParser
    {
        private const string PatternF1 = @"F(1|I)$";
        private const string Pattern = @"^=";
        private const string RegStar = @"^\*";
        private const string RegCount = @"^(\d+\.?\d*?){1}";
        private const string PatternNumberItems = @"(NUMBER|ITEM){1}";

        private readonly IList<ProductParserModel> products;

        private readonly string[] lines;

        public JewelOscoParser()
        {
            this.NumberOfItems = 0;
            this.products = new List<ProductParserModel>();
            this.lines = new ReceiptsData().GetLines(MarketTypes.JewelOsco);
        }

        private void Parse()
        {
            this.FirstLevelParse();
            this.SecondLevelParse();
        }

        private void FirstLevelParse()
        {
            var regF1 = new Regex(PatternF1);
            var reg = new Regex(Pattern);
            var star = new Regex(RegStar);
            var count = new Regex(RegCount);
            var numberItem = new Regex(PatternNumberItems);
            var flag = false;

            for (var i = 0; i < this.lines.Length; i++)
            {
                var str = this.lines[i];
                if (regF1.IsMatch(str))
                {
                    if (reg.IsMatch(str))
                    {
                        flag = true;
                        continue;
                    }

                    if (star.IsMatch(str))
                    {
                        str = str.Substring(1);
                    }

                    var product = new ProductParserModel { Title = str, Volume = str };
                    if (!flag)
                    {
                        var temp = this.lines[i - 1];
                        var tempCount = temp ?? string.Empty;
                        if (tempCount != string.Empty && count.IsMatch(tempCount))
                        {
                            product.Quantity = tempCount;
                        }
                        else
                        {
                            product.Quantity = "1";
                        }
                    }
                    else
                    {
                        product.Quantity = "1";
                    }

                    this.products.Add(product);
                    flag = true;
                }
                else
                {
                    if (numberItem.IsMatch(str))
                    {
                        var s = str.Split('\t');
                        this.NumberOfItems = Convert.ToInt32(s[s.Length - 1]);
                        break;
                    }

                    flag = false;
                }
            }
        }

        private void SecondLevelParse()
        {
            var patternDelimiter = new Regex(@"\d{4}$");

            foreach (var product in this.products)
            {
                product.Title = product.Title.Substring(0, product.Title.IndexOf('\t'));

                if (patternDelimiter.IsMatch(product.Title))
                {
                    product.Title = product.Title.Substring(0, product.Title.LastIndexOf(' '));
                }
            }
        }

        public IList<ProductParserModel> GetProducts()
        {
            this.Parse();
            this.GetQuantity();

            return this.products;
        }

        public int NumberOfItems { get; private set; }

        private void GetQuantity()
        {
            var patternFirstNumb = new Regex(@"[ ]");
            var patternUnit = new Regex(@"(l|1)b");

            foreach (var product in this.products)
            {
                if (patternFirstNumb.IsMatch(product.Quantity))
                {
                    var str = product.Quantity.Split(' ');
                    product.Quantity = str[0];
                    if (patternUnit.IsMatch(str[1]))
                    {
                        product.Unit = UnitType.LB;
                    }
                }
            }
        }
    }
}