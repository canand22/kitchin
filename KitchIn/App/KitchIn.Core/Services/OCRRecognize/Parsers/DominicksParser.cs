using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using KitchIn.Core.Enums;
using KitchIn.Core.Services.OCRRecognize.Models;

namespace KitchIn.Core.Services.OCRRecognize.Parsers
{
    public class DominicksParser : IParser
    {
        private const string PatternLB = @"(LB|lb|It|Ib|1b|ib|[@$]){1}";
        private const string PatternNumberItems = @"(NUMBER|ITEM){1}";
        private const string PattGrocery = @"(GROCERY){1}";
        private const string PattGenMerchandise = @"(MERCHAN){1}";
        private const string PattMeat = @"(MEAT){1}";
        private const string PattProduce = @"(PRODUCE){1}";
        private const string PattFloral = @"(FLORAL){1}";
        private const string PattDeli = @"(DELI){1}";
        private const string PattLiquor = @"(LIQUOR){1}";

        private readonly IList<ProductParserModel> products;

        private readonly string[] lines;

        public DominicksParser()
        {
            this.NumberOfItems = 0;
            this.products = new List<ProductParserModel>();
            this.lines = new ReceiptsData().GetLines(MarketTypes.Dominicks);
        }

        private void Parse()
        {
            this.FirstLevelParse();
            this.SecondLevelParse();
        }

        private void FirstLevelParse()
        {
            var numberItem = new Regex(PatternNumberItems);
            var grocery = new Regex(PattGrocery);
            var meat = new Regex(PattMeat);
            var produce = new Regex(PattProduce);
            var deli = new Regex(PattDeli);
            var liquor = new Regex(PattLiquor);

            bool flag = false;
            var i = 0;

            while (true)
            {
                var str = this.lines[i];

                if (grocery.IsMatch(str) || meat.IsMatch(str) || deli.IsMatch(str))
                {
                    i = this.AddLines(i + 1, null);
                }
                else if (produce.IsMatch(str))
                {
                    i = this.AddLines(i + 1, flag);

                    flag = true;
                }
                else if (liquor.IsMatch(str))
                {
                    i++;
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
                    i++;
                }
            }
        }

        private int AddLines(int index, bool? flag)
        {
            var regPrice = new Regex(@"(ReaPrice|RegPrice){1}");
            var savings = new Regex(@"(Savinas|Savings){1}");
            var grocery = new Regex(PattGrocery);
            var meat = new Regex(PattMeat);
            var produce = new Regex(PattProduce);
            var deli = new Regex(PattDeli);
            var liquor = new Regex(PattLiquor);
            var merchant = new Regex(PattGenMerchandise);
            var floral = new Regex(PattFloral);
            var count = new Regex(PatternLB);
            var exc = new Regex(@"(WT|UT|wt|ut){1}");
            var except = new Regex(@"^(WT|UT|wt|ut)$");
            var qty = new Regex(@"(QTY|qty){1}$");
            var exceptMinus = new Regex(@"-$");
            var exceptOnlyNumbers = new Regex(@"(\d+?\.?\d+?)$");

            for (var i = index; i < this.lines.Length; i++)
            {
                var str = this.lines[i];
                if (grocery.IsMatch(str)
                    || meat.IsMatch(str)
                    || deli.IsMatch(str)
                    || produce.IsMatch(str)
                    || liquor.IsMatch(str)
                    || merchant.IsMatch(str)
                    || floral.IsMatch(str))
                {
                    index = i;
                    break;
                }

                if (regPrice.IsMatch(str) || savings.IsMatch(str) || count.IsMatch(str))
                {
                    if (!exc.IsMatch(str))
                    {
                        continue;
                    }
                }

                if (except.IsMatch(str) || exceptMinus.IsMatch(str) || exceptOnlyNumbers.IsMatch(str))
                {
                    continue;
                }

                if (qty.IsMatch(str))
                {
                    this.products[this.products.Count - 1].Quantity = str.Substring(0, str.ToLower().IndexOf("qty"));
                    continue;
                }

                var product = new ProductParserModel { Title = str, Volume = str };
                if (flag.HasValue)
                {
                    if (!flag.Value)
                    {
                        var temp = this.lines[i - 1];
                        var tempCount = temp ?? string.Empty;
                        if (tempCount != string.Empty && count.IsMatch(tempCount))
                        {
                            product.Quantity = tempCount.Trim();
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
                }

                this.products.Add(product);
            }

            return index;
        }

        private void SecondLevelParse()
        {
            var patternDelimiter = new Regex(@"((O|0|F)?Z){1}$");
            var patternZ = new Regex(@"Z{1}$");
            var patternOZ = new Regex(@"((0|O)Z){1}$");
            var patternFZ = new Regex(@"(FZ){1}$");
            var patternDouble = new Regex(@"(\d+(([ ]?)|(\.?))(\d+)?){1}$");
            var patternWT = new Regex(@"^((W|U|LI)T\t){1}");

            foreach (var product in this.products)
            {
                if (patternDelimiter.IsMatch(product.Title))
                {
                    if (patternOZ.IsMatch(product.Title) || patternFZ.IsMatch(product.Title))
                    {
                        product.Unit = patternOZ.IsMatch(product.Title) ? UnitType.OZ : UnitType.FZ;
                        product.Title = product.Title.Substring(0, product.Title.Length - 2).TrimEnd();
                        product.Volume = product.Volume.Substring(0, product.Volume.Length - 2).TrimEnd();
                    }

                    if (patternZ.IsMatch(product.Title))
                    {
                        product.Title = product.Title.Substring(0, product.Title.Length - 1);
                        product.Unit = UnitType.OZ;
                    }

                    if (patternDouble.IsMatch(product.Title))
                    {
                        var m = patternDouble.Match(product.Title);
                        if (m.Success)
                        {
                            product.Title = product.Title.Substring(0, m.Index).TrimEnd();
                            product.Volume = product.Volume.Substring(m.Index).Replace(' ', '.');
                        }
                    }
                }
                else if (patternWT.IsMatch(product.Title))
                {
                    product.Title = product.Title.Substring(product.Title.IndexOf('\t')).TrimStart(new[] { '\t', ' ' });
                    product.Title = product.Title.Replace("\t", string.Empty);
                }
            }
        }

        private void GetQuantity()
        {
            var qty = new Regex(@"(QTY|qty){1}");
            var patternUnit = new Regex(@"(LB|lb|1b|Ib){1}");

            foreach (var product in this.products)
            {
                if (product.Quantity != null)
                {
                    if (patternUnit.IsMatch(product.Quantity))
                    {
                        product.Unit = UnitType.LB;
                        product.Quantity = product.Quantity.Substring(0, product.Quantity.IndexOf(" ")).Trim();
                    }
                }
                else
                {
                    product.Quantity = "1";
                    if (qty.IsMatch(product.Title))
                    {
                        product.Quantity = product.Title.Substring(0, product.Title.IndexOf(" ")).Trim();
                    }
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
    }
}