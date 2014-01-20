using System.Collections.Generic;
using KitchIn.Core.Entities;
using KitchIn.Core.Models;

namespace KitchIn.Core.Interfaces
{
    public interface IManageProductProvider
    {
        void Save(Product product);

        Product GetProduct(long id);

        Product GetProduct(string shortName, long storeId);
        
        long CreateProduct(ProductiPhoneModel prod);

        IEnumerable<KeyValuePair<long, string>> GetAllProducts(string categoryId);

        IEnumerable<Product> GetProductsMatchesFirstLetters(int numberFirstLetters, string firstLetters);

        IEnumerable<Product> GetAllProducts();

        IEnumerable<Product> GetAllProductsByStore(long storeId);

        IEnumerable<Product> GetAllProductsByStoreAndCategory(long storeId, long categoryId);

        IEnumerable<Product> SearchProductsByFirstLetters(string letters, long categoryId, long storeId);

        bool IsFoodGroupe(long productId);
    }
}