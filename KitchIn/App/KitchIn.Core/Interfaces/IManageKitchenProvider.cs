using System;
using System.Collections.Generic;
using KitchIn.Core.Entities;
using KitchIn.Core.Models;
using SmartArch.Data;

namespace KitchIn.Core.Interfaces
{
    public interface IManageKitchenProvider : IManageProductProvider
    {
        IRepository<ProductsOnKitchen> ProductsOnKitchenRepo { get; }

        void SaveProductOnKitchen(ProductsOnKitchen product);

        void SaveKitchen();

        IEnumerable<ProductiPhoneModel> GetProducts(Guid id, long categoryId);

        IEnumerable<CategoryiPhoneModel> GetCategories(Guid id);

        bool AddProductOnKitchen(Guid id, ProductiPhoneModel model);

        bool AddProducts(Guid id, IEnumerable<ProductiPhoneModel> products);

        void RemoveProductOnKitchen(string productId);

        void EditProductOnKitchen(string productId, string quantity);
    }
}