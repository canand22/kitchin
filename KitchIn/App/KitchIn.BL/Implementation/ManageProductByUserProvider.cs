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

    public class ManageProductByUserProvider : IManageProductByUserProvider
    {
        private readonly IRepository<Product> productsRepo;

        private readonly IRepository<ProductByUser> productByUserRepo;

        private readonly IRepository<Category> categoriesRepo;

        private readonly IRepository<Store> storesRepo;

        private readonly IRepository<Ingredient> ingredientsRepo;

        public ManageProductByUserProvider()
        {
            
        }

        public ManageProductByUserProvider(IRepository<Product> productsRepo, IRepository<Category> categoriesRepo, IRepository<Store> storesRepo,
            IRepository<ProductByUser> productByUserRepo, IRepository<Ingredient> ingredientsRepo)
        {
            this.productsRepo = productsRepo;
            this.categoriesRepo = categoriesRepo;
            this.storesRepo = storesRepo;
            this.productByUserRepo = productByUserRepo;
            this.ingredientsRepo = ingredientsRepo;
        }

        public void Save(string upcCode, string shortName, string name, string ingredientName, long? categoryId, long? storeId, User user, int? expirationDate, long id = 0)
        {
            var isNewItem = id == 0;
            var ingredient = this.ingredientsRepo.FirstOrDefault(p => p.Term == ingredientName);
            if (isNewItem)
            {
                var product = new ProductByUser
                                  {
                                      Category = categoryId.HasValue? this.categoriesRepo.Get(categoryId.Value) : null,
                                      UpcCode = upcCode,
                                      Ingredient = ingredient,
                                      Date = DateTime.Now,
                                      Name = name,
                                      ShortName = shortName,
                                      Store = storeId.HasValue ?  this.storesRepo.Get(storeId.Value) : null,
                                      ExpirationDate = expirationDate,
                                      User = user
                                  };
                this.productByUserRepo.Save(product);
            }
            else
            {
                var product = this.productByUserRepo.Get(id);
                product.Category = categoryId.HasValue ? this.categoriesRepo.Get(categoryId.Value) : null;
                product.UpcCode = upcCode;
                product.Ingredient = ingredient;
                product.Date = DateTime.Now;
                product.Name = name;
                product.ShortName = shortName;
                product.Store = storeId.HasValue ? this.storesRepo.Get(storeId.Value) : null ;
                product.ExpirationDate = expirationDate;
                product.User = user;
                this.productByUserRepo.Save(product);
            }
            this.productByUserRepo.SaveChanges();
        }

        public IEnumerable<ProductByUser> GetAllProducts()
        {
            return this.productByUserRepo;
        }

        public void Remove(ProductByUser product)
        {
            this.productByUserRepo.Remove(product);
        }

        public ProductByUser GetProduct(long id)
        {
            return this.productByUserRepo.SingleOrDefault(x => x.Id == id);
        }

    }
}
