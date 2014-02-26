using System;
using System.Collections.Generic;
using System.Linq;
using KitchIn.Core.Entities;
using KitchIn.Core.Interfaces;
using KitchIn.Core.Models;

using SmartArch.Data;

namespace KitchIn.BL.Implementation
{
    using KitchIn.Core.Enums;

    public class ManageProductProvider : BaseProvider, IManageProductProvider
    {
        private readonly IRepository<Product> productsRepo;

        private readonly IRepository<Category> categoriesRepo;

        private readonly IRepository<Store> storesRepo;

        private const string NonFoodCategory = "NON-FOOD";

        public ManageProductProvider()
        {
            
        }

        public ManageProductProvider(IRepository<Product> productsRepo, IRepository<Category> categoriesRepo, IRepository<Store> storesRepo)
        {
            this.productsRepo = productsRepo;
            this.categoriesRepo = categoriesRepo;
            this.storesRepo = storesRepo;
        }

        public void Save(Product product)
        {
            this.productsRepo.Save(product);
        }

        public void Save(string shortName, string name, string ingredientName, long categoryId, long storeId, long id = 0, string upcCode = null)
        {
            var isNewItem = id == 0;
            if (isNewItem)
            {
                var product = new Product()
                                  {
                                      Category = this.categoriesRepo.Get(categoryId),
                                      IngredientName = ingredientName,
                                      ModificationDate = DateTime.Now,
                                      Name = name,
                                      ShortName = shortName,
                                      Store = this.storesRepo.Get(storeId),
                                      TypeAdd = TypeAdd.Manually,
                                      UpcCode = upcCode
                                  };
                this.productsRepo.Save(product);
            }
            else
            {
                var product = this.productsRepo.Get(id);
                product.Category = this.categoriesRepo.Get(categoryId);
                product.IngredientName = ingredientName;
                product.ModificationDate = DateTime.Now;
                product.Name = name;
                product.ShortName = shortName;
                product.Store = this.storesRepo.Get(storeId);
                product.TypeAdd = TypeAdd.Manually;
                product.UpcCode = upcCode;
            }
        }

        public Product GetProduct(long id)
        {
            return this.productsRepo.SingleOrDefault(x => x.Id == id);
        }

        public Product GetProduct(string shortName, long storeId)
        {
            return this.productsRepo.FirstOrDefault(x => x.ShortName == shortName && x.Store.Id == storeId);
        }
        
        public long CreateProduct(ProductiPhoneModel prod)
        {
            var product = new Product
                       {
                           Category = this.GetCategory(prod.CategoryId.Value),
                           //ExpirationDate = string.Empty,
                           Name = prod.Name,
                           //IsAddedByUser = true
                       };

            this.Save(product);
            return product.Id;
        }
        
        public IEnumerable<KeyValuePair<long, string>> GetAllProducts(string categoryId)
        {
            return
                this.productsRepo
                .Where(x => x.Category.Id == Convert.ToInt64(categoryId))
                .Select(x => new KeyValuePair<long, string>(x.Id, x.Name));
        }

        public IEnumerable<Product> GetProductsMatchesFirstLetters(int numberFirstLetters, string firstLetters)
        {
            return this.productsRepo.Where(x => x.ShortName.Substring(0, numberFirstLetters) == firstLetters);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return this.productsRepo;
        }

        public IEnumerable<Product> GetAllProductsByStore(long storeId)
        {
            return this.productsRepo.Where(x => x.Store.Id == storeId);
        }

        public IEnumerable<Product> GetAllProductsByStoreAndCategory(long storeId, long categoryId)
        {
            return this.productsRepo.Where(x => x.Store.Id == storeId && x.Category.Id == categoryId);
        }

        public IEnumerable<Product> SearchProductsByFirstLetters(string letters, long categoryId, long storeId)
        {
            var products = new List<Product>();
            products = this.productsRepo.Where(x => x.Store.Id == storeId && x.Category.Id == categoryId && 
                x.ShortName.Substring(0, letters.Length).ToLower().Equals(letters.ToLower())).ToList();
            return products;
        } 

        protected Category GetCategory(long categoryId)
        {
            return this.categoriesRepo.Get(categoryId);
        }

        public bool IsFoodGroupe(long productId)
        {
            var singleOrDefault = this.productsRepo.SingleOrDefault(x => x.Id == productId);
            return singleOrDefault != null && singleOrDefault.Category.Name != NonFoodCategory;
        }

        public void Remove(Product product)
        {
            this.productsRepo.Remove(product);
        }
    }
}
