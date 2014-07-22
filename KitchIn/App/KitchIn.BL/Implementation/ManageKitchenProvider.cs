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
    public class ManageKitchenProvider : ManageProductProvider, IManageKitchenProvider
    {
        public IRepository<ProductsOnKitchen> ProductsOnKitchenRepo
        {
            get { return ServiceLocator.Current.GetInstance<IRepository<ProductsOnKitchen>>(); }
        }

        public void SaveProductOnKitchen(ProductsOnKitchen product)
        {
            this.ProductsOnKitchenRepo.Save(product);
        }

        public void SaveKitchen()
        {
            this.ProductsOnKitchenRepo.SaveChanges();
        }

        public IEnumerable<ProductiPhoneModel> GetProducts(Guid id, long categoryId)
        {
            return this.GetProductsOnKitchen(id, categoryId)
                .Select(x => new ProductiPhoneModel
                       {
                           Name = x.Name,
                           ProductId = x.Product.Id,
                           CategoryId = x.Category.Id,
                           Quantity = x.Quantity,
                           //HasExpired = this.HasExpiredProduct(x)
                       }).ToList();
        }

        public IEnumerable<CategoryiPhoneModel> GetCategories(Guid id)
        {
            var models = new List<CategoryiPhoneModel>();

            //foreach (var cat in this.CategoriesRepo)
            //{
            //    var categ = new CategoryiPhoneModel
            //                    {
            //                        CategoryId = cat.Id,
            //                        Name = cat.Name,
            //                        CountProducts = this.GetProductsOnKitchen(id, cat.Id).Count(),
            //                        //HasExpired = this.HasExpired(id, cat.Id)
            //                    };

            //    models.Add(categ);
            //}

            return models;
        }

        public bool AddProductOnKitchen(Guid id, ProductiPhoneModel model)
        {
            var user = this.GetUser(id);
            if (user == null)
            {
                return false;
            }

            if (!model.ProductId.HasValue)
            {
                model.ProductId = this.CreateProduct(model);
            }

            user.Products.Add(this.Add(user, model));
            this.UserRepo.SaveChanges();

            return true;
        }

        public bool AddProducts(Guid id, IEnumerable<ProductiPhoneModel> products)
        {
            return products.All(product => this.AddProductOnKitchen(id, product));
        }

        public void RemoveProductOnKitchen(string productId)
        {
            this.ProductsOnKitchenRepo.Remove(Convert.ToInt64(productId));
        }

        public void EditProductOnKitchen(string productId, string quantity)
        {
            var product = this.ProductsOnKitchenRepo.Get(Convert.ToInt64(productId));
            product.Quantity = Convert.ToDouble(quantity);
            this.SaveProductOnKitchen(product);
        }

        private ProductsOnKitchen Add(User user, ProductiPhoneModel model)
        {
            var productOnKitchen = new ProductsOnKitchen
            {
                User = user,
                Product = this.GetProduct(model.ProductId.Value),
                Name = model.Name,
                Quantity = model.Quantity,
                Category = this.GetCategory(model.CategoryId.Value)
            };

            this.SaveProductOnKitchen(productOnKitchen);

            return productOnKitchen;
        }

        private IQueryable<ProductsOnKitchen> GetProductsOnKitchen(Guid id, long categoryId)
        {
            return this.ProductsOnKitchenRepo
                .Where(x => x.User.SessionId == id && x.Category.Id == categoryId);
        }

        //private bool HasExpired(Guid id, long categoryId)
        //{
        //    return this.GetProductsOnKitchen(id, categoryId).Any(this.HasExpiredProduct);
        //}

        //private bool HasExpiredProduct(ProductsOnKitchen product)
        //{
        //    return product.Product != null
        //        && (!string.IsNullOrWhiteSpace(product.Product.ExpirationDate)
        //            && (DateTime.Now - product.DateOfPurchase).Days > Convert.ToInt32(product.Product.ExpirationDate));
        //}
    }
}