using KitchIn.Core.Entities;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace KitchIn.Core.Services.OCRRecognize.Parsers
{
    using System.Configuration;
    using System.Linq;

    public class PotashParser : IParser
    {
        private readonly IList<ProductParserModel> products;

        private readonly string[] lines;

        public int NumberOfItems { get; private set; }

        private List<string> FirstResult { get; set; }

        private List<string> SecondaryVerification { get; set; }

        private const int MinLengthShortname = 3;
        private static readonly Regex PatternLf = new Regex(@"((\s|\t)*(L|I)(\s|\t)*(F)?){1}$");
        private static readonly Regex PatternH = new Regex(@"((\s|\t)*(H|h)(B)?(F)?){1}$");
        private static readonly Regex PatternCanceledOrSubtracted = new Regex(@"(-\d+(([ ]?)|(\.?))(\d+)?){1}$");
        private static readonly Regex PatternDouble = new Regex(@"(\d+(([ ]?)|(\.?)|(\,?))(\d+)?){1}$");
        private static readonly Regex PatternBegining = new Regex(@"^(\W*(\s|\t)*)");
        private static readonly Regex PatternEnding = new Regex(@"((((\s|\t)+((\d)+((\d+(([ ]?)|(\.?)|(\,?))(\d+)?))+)(\s|\t)*)$)|(((\t)+(.)*)*$))");
        private static readonly Regex PatternTwoFor = new Regex(@"((\d)*(\s|\t)*(@|8|6|§|\(2)?(\s|\t)+\d(.)*FOR(.)*(\d)+(\.|\,)(\d){2})");
        private static readonly Regex PatternTwoForLight = new Regex(@"(\d(.)*FOR(.)*(\d)+(\.|\,)(\d){2})");
        private static readonly Regex PatternFewPieces = new Regex(@"((\s|\t)*\d(\s|\t)*(@|8|6|§|\(2)(\s|\t)*(\d)+(\.|\,)(\d){2}(\s|\t)*(EA)?)");
        private static readonly Regex PatternWeighting = new Regex(@"(\d(.)*(lb|1b|Ib)+(.)*\d(.)*(lb|1b|Ib)+)");
        private static readonly Regex PatternDoubleWithGarbage = new Regex(@"^((\W)*(\d)+((\.)|(\,))(\d+)+(\W)*){1}");

        public PotashParser(string[] lines)
        {
            this.NumberOfItems = 0;
            this.products = new List<ProductParserModel>();
            this.lines = lines;
            this.FirstResult = new List<string>();
            this.SecondaryVerification = new List<string>();
        }

        private void Parse()
        {
            this.FirstLevelParse();
            this.SecondLevelParse();
            var result = FirstResult.Distinct().ToList();
            result.ForEach(x => products.Add(new ProductParserModel() { Title = x }));
        }

        private void FirstLevelParse()
        {
            for (var line = 0; line < lines.Length; line++)
            {
                if (IsEndOfReceipt(lines[line]))
                {
                    break;
                }

                if (IsBeginOfReceipt(lines[line]))
                {
                    continue;
                }

                var isCanceled = false;
                if (PatternLf.IsMatch(lines[line]) || PatternH.IsMatch(lines[line]))
                {
                    var withoutEnding = PatternLf.IsMatch(lines[line])
                                            ? Regex.Replace(lines[line], PatternLf.ToString(), "").Trim()
                                            : Regex.Replace(lines[line], PatternH.ToString(), "").Trim();
                    var withoutBegining = PatternBegining.IsMatch(withoutEnding)
                                              ? Regex.Replace(withoutEnding, PatternBegining.ToString(), "")
                                              : withoutEnding;
                    string withoutPrice;
                    if (PatternCanceledOrSubtracted.IsMatch(withoutBegining))
                    {
                        isCanceled = true;
                        withoutPrice = Regex.Replace(withoutBegining, PatternCanceledOrSubtracted.ToString(), "").Trim();
                    }
                    else
                    {
                        withoutPrice = Regex.Replace(withoutBegining, PatternDouble.ToString(), "").Trim();
                    }

                    //without 2FOR
                    if (PatternTwoFor.IsMatch(withoutPrice))
                    {
                        withoutPrice = Regex.Replace(withoutPrice, PatternTwoFor.ToString(), "").Trim();
                    }

                    if (PatternTwoForLight.IsMatch(withoutPrice))
                    {
                        continue;
                    }

                    //without Few Pieces
                    if (PatternFewPieces.IsMatch(withoutPrice))
                    {
                        withoutPrice = Regex.Replace(withoutPrice, PatternFewPieces.ToString(), "").Trim();
                    }

                    string description;
                    if (PatternWeighting.IsMatch(withoutPrice) && line != 0)
                    {
                        description = lines[line - 1].Trim();
                    }
                    else
                    {
                        description = withoutPrice;
                    }

                    var withoutEndingSecondParse = Regex.Replace(description, PatternEnding.ToString(), "").Trim();
                    description = withoutEndingSecondParse;

                    if (isCanceled)
                    {
                        FirstResult.Remove(description);
                    }
                    else
                    {
                        if (description != string.Empty)
                        {
                            FirstResult.Add(description);
                        }
                    }
                }
                else
                {
                    SecondaryVerification.Add(lines[line]);
                }
            }
        }

        private bool IsEndOfReceipt(string line)
        {
            var endingRecaeipts = ConfigurationManager.AppSettings["PossibleOptionsEndOfReceiptsPotash"].Split(',').ToList();
            return endingRecaeipts.Any(item => line.ToLower().Contains(item.ToLower()));
        }

        private bool IsBeginOfReceipt(string line)
        {
            var begingRecaeipts = ConfigurationManager.AppSettings["PossibleOptionsBeginOfReceiptsPotash"].Split(',').ToList();
            return begingRecaeipts.Any(item => line.ToLower().Contains(item.ToLower()));
        }

        private bool IsSubtracted(string line)
        {
            var subtracted = ConfigurationManager.AppSettings["ItemSubtracted"].Split(',').ToList();
            return subtracted.Any(item => line.ToLower().Contains(item.ToLower()));
        }

        private bool IsCancelled(string line)
        {
            var subtracted = ConfigurationManager.AppSettings["ItemCancelled"].Split(',').ToList();
            return subtracted.Any(item => line.ToLower().Contains(item.ToLower()));
        }

        private void SecondLevelParse()
        {
            for (int i = 0; i < SecondaryVerification.Count; i++)
            {
                if (IsEndOfReceipt(SecondaryVerification[i]))
                {
                    break;
                }

                if (PatternWeighting.IsMatch(SecondaryVerification[i]))
                {
                    continue;
                }

                if (PatternTwoFor.IsMatch(SecondaryVerification[i]))
                {
                    continue;
                }

                if (PatternTwoForLight.IsMatch(SecondaryVerification[i]))
                {
                    continue;
                }

                var wittoutBegining = Regex.Replace(SecondaryVerification[i], PatternBegining.ToString(), "").Trim();
                var wittoutEndining = Regex.Replace(wittoutBegining, PatternEnding.ToString(), "").Trim();

                if (IsSubtracted(SecondaryVerification[i]) || IsCancelled(SecondaryVerification[i]))
                {
                    continue;
                }

                //to weed out waste - assuming that the length of the short name can not be less than or equal to 3
                if (wittoutEndining.Length <= MinLengthShortname)
                {
                    continue;
                }

                //sifting garbage
                wittoutEndining = SiftingGarbage(wittoutEndining);

                if (wittoutEndining != string.Empty)
                {
                    FirstResult.Add(wittoutEndining);
                }
            }
        }

        private string SiftingGarbage(string text)
        {
            var result = string.Empty;

            if (PatternDoubleWithGarbage.IsMatch(text))
            {
                return result;
            }
            result = text;
            return result;
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