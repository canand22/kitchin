using System.Collections.Generic;
using KitchIn.Core.Models;
using KitchIn.Core.Interfaces;
using System.Linq;
using KitchIn.Core.Entities;
using KitchIn.Core.Services.Cache;
using KitchIn.Core.Services.TextMatching;

namespace KitchIn.BL.Implementation
{
    public class ManageMatchingTexts : IManageMatchingTexts
    {
        private string[] TextForRecognizer { get; set; }

        private Store Store { get; set; }

        private readonly IManageProductProvider productProvider;

        private readonly IManageStoreProvider manageStoreProvider;

        private readonly IManageCaches manageCaches;

        private const int PercCorrect = 70; // percentage correct for matching

        public ManageMatchingTexts(IManageProductProvider manageProductProvider, IManageStoreProvider manageStoreProvider, IManageCaches manageCaches)
        {
            this.productProvider = manageProductProvider;
            this.manageStoreProvider = manageStoreProvider;
            this.manageCaches = manageCaches;
            this.manageCaches.InitCaches();
        }

        public IList<ResultMatching> GetResultsOfTheMatching(string[] textForRecognizer, long storeId)
        {
            TextForRecognizer = textForRecognizer;
            Store = manageStoreProvider.GetStore(storeId);

            var result = new List<ResultMatching>();
            foreach (var item in TextForRecognizer)
            {
                var firstAttempt = productProvider.GetProduct(item, Store.Id);
                if (firstAttempt != null)
                {
                    result.Add(new ResultMatching() { Id = firstAttempt.Id, IsSuccessMatching = true, ItemName = firstAttempt.Name});
                    continue;
                }

                var secondAttempt = GetProductMatchesFirstLetters(item, Store.Name);
                if (secondAttempt != null)
                {
                    result.Add(new ResultMatching() { Id = secondAttempt.Id, IsSuccessMatching = true, ItemName = secondAttempt.Name });
                    continue;
                }
                else
                {
                    result.Add(new ResultMatching() { IsSuccessMatching = false, ItemName = item });
                }

            }
            return result;
        }

        private Product GetProductMatchesFirstLetters(string item, string storeName)
        {
            var result = new Product();
            var productsForMatching = KitchInCache.GetCachedItem(storeName) as List<KeyValuePair<string, long>>;
            var levenshteinDistance = new LevenshteinDistance(item, PercCorrect);
            var candidates = new List<KeyValuePair<string, long>>();
            if (productsForMatching != null)
            {
                foreach (var matchesItem in productsForMatching)
                {
                    if (levenshteinDistance.PercentageCheck(matchesItem.Key))
                    {
                        candidates.Add(matchesItem);
                    }
                }
            }
            if (!candidates.Any())
            {
                return null;
            }
            var productId = levenshteinDistance.GetStringWithMinimumDistanceLevenshtein(candidates);
            result = productProvider.GetProduct(productId);
            return result;
        }
    }
}
