using System.Collections.Generic;
using System.Linq;
using KitchIn.Core.Entities;
using SmartArch.Data;

namespace KitchIn.BL.Implementation
{
    using KitchIn.Core.Interfaces;

    public class ManageStoreProvider : BaseProvider, IManageStoreProvider
    {
        private readonly IRepository<Store> storesRepo;

        public ManageStoreProvider(IRepository<Store> storesRepository)
        {
            this.storesRepo = storesRepository;
        }

        public void Save(Store store)
        {
            this.storesRepo.Save(store);
        }

        public Store GetStore(long id)
        {
            return this.storesRepo.SingleOrDefault(x => x.Id == id);
        }

       public IEnumerable<KeyValuePair<long, string>> GetAllStores()
        {
            return this.storesRepo.Select(x => new KeyValuePair<long, string>(x.Id, x.Name));
        }

        public IEnumerable<CategoryInStore> GetAllCategoriesInStore(long id)
        {
            var store = this.storesRepo.SingleOrDefault(x => x.Id == id);
            if (store != null)
            {
                return store.Categories;
            }
            return null;
        }
    }
}
