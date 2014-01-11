using System.Collections.Generic;
using System.Linq;
using KitchIn.Core.Interfaces;

namespace KitchIn.Core.Services.Cache
{
    public class ManageCaches : IManageCaches
    {
        private readonly IManageProductProvider productProvider;

        private readonly IManageStoreProvider storeProvider;

        public ManageCaches(IManageProductProvider productProvider, IManageStoreProvider storeProvider)
        {
            this.productProvider = productProvider;
            this.storeProvider = storeProvider;
        }
        
        public void InitCaches()
        {
            var storesName = storeProvider.GetAllStores();
            foreach (var store in storesName)
            {
                var cacheName = store.Value;
                var cacheItems = KitchInCache.GetCachedItem(cacheName);
                if (cacheItems == null)
                {
                    var allProducts = productProvider.GetAllProductsByStore(store.Key);
                    var items = allProducts.Select(x => new KeyValuePair<string, long>(x.ShortName, x.Id)).ToList();
                    KitchInCache.AddToCache(cacheName, items, CachePriority.NotRemovable);
                }
            }
        }

    }


}
