using System.Collections.Generic;
using KitchIn.Core.Entities;
using System;

namespace KitchIn.Core.Interfaces
{
    public interface IManageProductByUserProvider
    {
        void Save(string upcCode, string shortName, string name, string ingredientName, long categoryId, long storeId, User user, int? expirationDate, long id = 0);

        IEnumerable<ProductByUser> GetAllProducts();

        void Remove(ProductByUser product);

        ProductByUser GetProduct(long id);
    }
}