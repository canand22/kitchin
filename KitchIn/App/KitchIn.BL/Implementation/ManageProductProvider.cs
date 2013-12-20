using System;
using System.Collections.Generic;
using System.Linq;
using KitchIn.Core.Entities;
using KitchIn.Core.Interfaces;
using KitchIn.Core.Models;
using Microsoft.Practices.ServiceLocation;
using SmartArch.Data;

namespace KitchIn.BL.Implementation
{
    public class ManageProductProvider : BaseProvider, IManageProductProvider
    {
        public IRepository<Product> ProductsRepo
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IRepository<Product>>();
            }
        }

        public IRepository<Category> CategoriesRepo
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IRepository<Category>>();
            }
        }

        public void Save(Product product)
        {
            this.ProductsRepo.Save(product);
        }

        public Product GetProduct(long id)
        {
            return this.ProductsRepo.SingleOrDefault(x => x.Id == id);
        }
        
        public long CreateProduct(ProductiPhoneModel prod)
        {
            var product = new Product
                       {
                           Category = this.GetCategory(prod.CategoryId.Value),
                           ExpirationDate = string.Empty,
                           Name = prod.Name,
                           IsAddedByUser = true
                       };

            this.Save(product);
            return product.Id;
        }
        
        public IEnumerable<KeyValuePair<long, string>> GetAllProducts(string categoryId)
        {
            return
                this.ProductsRepo
                .Where(x => x.Category.Id == Convert.ToInt64(categoryId))
                .Select(x => new KeyValuePair<long, string>(x.Id, x.Name));
        }

        protected Category GetCategory(long categoryId)
        {
            return this.CategoriesRepo.Get(categoryId);
        }
    }
}
