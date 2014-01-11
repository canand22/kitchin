using System.Collections.Generic;
using KitchIn.Core.Entities;

namespace KitchIn.Core.Interfaces
{
    public interface IManageStoreProvider
    {
        void Save(Store product);

        Store GetStore(long id);

        IEnumerable<KeyValuePair<long, string>> GetAllStores();
    }
}