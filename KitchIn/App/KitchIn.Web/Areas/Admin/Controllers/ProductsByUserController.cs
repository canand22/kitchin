using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using KitchIn.Core.Entities;
using KitchIn.Web.Core.Models.Admin;

using SmartArch.Data;
using SmartArch.NixJqGridFramework.Helpers.ModelBinder;
using SmartArch.Web.Attributes;

namespace KitchIn.Web.Areas.Admin.Controllers
{
    public class ProductsByUserController : ProductsController
    {
        public ProductsByUserController(
            IRepository<Category> repositoryCategory,
            IRepository<Product> repositoryProduct,
            IRepository<ProductsOnKitchen> repositoryProductsOnKitchens)
            : base(repositoryCategory, repositoryProduct, repositoryProductsOnKitchens)
        {
        }
       
        public override ActionResult Index()
        {
            var categories = this.repositoryCategory.Select(x => new SelectListItem
            {
                Text = x.Name ?? string.Empty,
                Value = x.Id.ToString()
            }).ToList();
            var model = new NixJqGridProductsByUserModel(categories);
            return View(model);
        }

        #region AJAX actions

        /// <summary>
        /// Get data for AjaxJqGrid
        /// </summary>
        /// <param name="gridContext">The grid context</param>
        /// <returns>Returns the view</returns>
        public override JsonResult GetDataForAjaxGrid(NixJqGridContext gridContext)
        {
            var products = this.repositoryProduct.ToList();

            //var viewModel = products.Where(x => x.IsAddedByUser).Select(p => new ProductViewModel
            var viewModel = products.Select(p => new ProductViewModel

            {
                Id = p.Id,
                Category = p.Category != null ? p.Category.Name : string.Empty,
                //ExpirationDate = p.ExpirationDate,
                Name = p.Name
                //IsAddedByUser = p.IsAddedByUser
            });

            return gridContext.Response(viewModel, isAllDataSearch: true);
        }

        [Transaction]
        public void Approve(long id)
        {
            var product = this.repositoryProduct.Get(id);
            //product.IsAddedByUser = false;

            this.repositoryProduct.Save(product);
        }

        #endregion AJAX actions
    }
}
