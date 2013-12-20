using System.Collections.Generic;
using KitchIn.Core.Entities;
using KitchIn.Core.Models;
using SmartArch.Data;

namespace KitchIn.Core.Interfaces
{
    public interface IManageProductProvider
    {
        IRepository<Product> ProductsRepo { get; }

        IRepository<Category> CategoriesRepo { get; }

        void Save(Product product);

        Product GetProduct(long id);

        long CreateProduct(ProductiPhoneModel prod);

        IEnumerable<KeyValuePair<long, string>> GetAllProducts(string categoryId);
    }
}