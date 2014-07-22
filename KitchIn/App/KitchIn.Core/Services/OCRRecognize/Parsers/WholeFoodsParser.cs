using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using KitchIn.Core.Enums;
using KitchIn.Core.Services.OCRRecognize.Models;

namespace KitchIn.Core.Services.OCRRecognize.Parsers
{
    public class WholeFoodsParser : IParser
    {
        private const string PatternB = @" B$";
        private const string PatternLB = @"((LB|lb|It|Ib|1b){1}|(TARE|TARt|I ARE|'ARE|ARE){1}|(@){1}){1}";
        private const string PatternNumberItems = @"(NUMBER){1}";
        private const string PatternItem = @"(ITEM|IT|:tem){1}";
        private const string PatternDelimiter = @"^(WT|UT|ulT|UlT){1}";
        private const string PatternTax = @"(TAX|Tax){1}";
        private const string PatternTel = @"(\d{3}-\d{3}){1}";

        private readonly IList<ProductParserModel> products;
        private readonly string[] lines;

        public WholeFoodsParser()
        {
            this.NumberOfItems = 0;
            this.products = new List<ProductParserModel>();
            this.lines = new ReceiptsData().GetLines(MarketTypes.WholeFoods);
        }

        private void Parse()
        {
            this.FirstLevelParse();
            this.SecondLevelParse();
        }

        private void FirstLevelParse()
        {
            var regB = new Regex(PatternB);
            var count = new Regex(PatternLB);
            var numberItem = new Regex(PatternNumberItems);
            var regOZ = new Regex(@"Z$");
            var regLB = new Regex(@"LB$");
            var regWT = new Regex(PatternDelimiter);
            var regTax = new Regex(PatternTax);
            var regItem = new Regex(PatternItem);
            var regTel = new Regex(PatternTel);
            //var star = new Regex(regStar);
            //var count = new Regex(regCount);
            var flag = false;
            var index = 0;
            for (var i = 0; i < this.lines.Length; i++)
            {
                var str = this.lines[i];
                if (regTel.IsMatch(str))
                {
                    index = i + 1;
                    break;
                }   
            }

            for (var i = index; i < this.lines.Length; i++)
            {
                var str = this.lines[i];

                if (regB.IsMatch(str))
                {
                    var product = this.SetProduct(str, count, flag, i);
                        
                    this.products.Add(product);
                    flag = true;
                }
                else
                {
                    if (regLB.IsMatch(str) || regOZ.IsMatch(str) || regWT.IsMatch(str))
                    {
                        var product = this.SetProduct(str, count, flag, i);
                        product.Unit = regLB.IsMatch(str) ? UnitType.LB : regOZ.IsMatch(str) ? UnitType.OZ : UnitType.None;
                        this.products.Add(product);
                        flag = true;
                        continue;
                    }

                    if (regTax.IsMatch(str))
                    {
                        break;
                    }

                    if (!regItem.IsMatch(str) && !count.IsMatch(str))
                    { 
                        var product = this.SetProduct(str, count, flag, i);
                        product.Unit = UnitType.None;
                        this.products.Add(product);
                        flag = true;
                        continue;
                    }

                    flag = false;
                }
            }

            foreach (var s in this.lines.Where(str => numberItem.IsMatch(str)).Select(str => str.Split('\t')))
            {
                this.NumberOfItems = Convert.ToInt32(s[s.Length - 1]);
                break;
            }
        }

        private ProductParserModel SetProduct(string str, Regex count, bool flag, int i)
        {
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

            return product;
        }

        private void SecondLevelParse()
        {
            var patternDelimiter = new Regex(PatternDelimiter);
            var patternB = new Regex(PatternB);
            var patternT = new Regex(@"\t");

            foreach (var product in this.products)
            {
                if (patternT.IsMatch(product.Title))
                {
                    if (patternB.IsMatch(product.Title))
                    {
                        product.Title = product.Title.Substring(0, product.Title.LastIndexOf('\t'));
                    }
                }

                if (product.Title.ToCharArray()[product.Title.Length - 1] == '\t')
                {
                    product.Title = product.Title.Substring(0, product.Title.LastIndexOf('\t'));
                }

                if (patternDelimiter.IsMatch(product.Title))
                {
                    product.Title = product.Title.Substring(product.Title.IndexOf('\t') + 1);

                    if (product.Title.ToCharArray()[0] == '\t')
                    {
                        product.Title = product.Title.Substring(product.Title.IndexOf('\t') + 1);
                    }
                }
                else if (product.Title.ToCharArray()[0] == '\t')
                {
                    product.Title = product.Title.Substring(product.Title.IndexOf('\t') + 1);

                    if (product.Title.ToCharArray()[0] == '\t')
                    {
                        product.Title = product.Title.Substring(product.Title.IndexOf('\t') + 1);
                    }
                }

                if (patternT.IsMatch(product.Title))
                {
                    product.Title = product.Title.Replace('\t', ' ');
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
            var patternUnit = new Regex(@"(LB|lb|1b|Ib|L.B){1}");
            var patternD = new Regex(@"(@){1}"); 

            foreach (var product in this.products)
            {
                if (patternUnit.IsMatch(product.Quantity))
                {
                    product.Unit = UnitType.LB;
                    product.Quantity = product.Quantity.Substring(0, product.Quantity.ToUpper().IndexOf("B") - 2);
                    product.Quantity = product.Quantity.Replace("\t", string.Empty);
                    product.Quantity = product.Quantity.Replace(" ", string.Empty);
                }
                
                if (patternD.IsMatch(product.Quantity))
                {
                    product.Quantity = product.Quantity.Substring(0, product.Quantity.IndexOf("@"));
                    product.Quantity = product.Quantity.Replace("\t", string.Empty);
                    product.Quantity = product.Quantity.Replace(" ", string.Empty);
                }
            }
        }
    }
}