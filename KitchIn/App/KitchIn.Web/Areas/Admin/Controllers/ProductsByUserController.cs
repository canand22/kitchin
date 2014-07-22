using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using KitchIn.Core.Entities;
using KitchIn.Web.Core.Models.Admin;

using SmartArch.Data;
using SmartArch.NixJqGridFramework.Helpers.ModelBinder;
using SmartArch.Web.Attributes;
using System.Web.UI.WebControls;
using KitchIn.Core.Interfaces;

namespace KitchIn.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class ProductsByUserController : Controller 
    {
        /// <summary>
        /// The manage Product Provider
        /// </summary>
        protected readonly IManageProductProvider manageProductProvider;

        /// <summary>
        /// The manage Category Provider
        /// </summary>
        protected readonly IManageCategoryProvider manageCategoryProvider;

        /// <summary>
        /// The manage Store Provider
        /// </summary>
        protected readonly IManageStoreProvider manageStoreProvider;

        /// <summary>
        /// The manage Store Provider
        /// </summary>
        protected readonly IManageProductByUserProvider manageProductByUserProvider;

        /// <summary>
        /// The manage User Provider
        /// </summary>
        protected readonly IManageUserProvider manageUserProvider;


        /// <summary>
        /// The manage Ingredient Provider
        /// </summary>
        protected readonly IManageIngredientProvider manageIngredientProvider;


        public ProductsByUserController(IManageProductProvider manageProductProvider, IManageCategoryProvider manageCategoryProvider,
            IManageStoreProvider manageStoreProvider, IManageProductByUserProvider manageProductByUserProvider, IManageUserProvider manageUserProvider, IManageIngredientProvider manageIngredientProvider)
        {
            this.manageCategoryProvider = manageCategoryProvider;
            this.manageProductByUserProvider = manageProductByUserProvider;
            this.manageProductProvider = manageProductProvider;
            this.manageStoreProvider = manageStoreProvider;
            this.manageUserProvider = manageUserProvider;
            this.manageIngredientProvider = manageIngredientProvider;
        }

        /// <summary>
        /// Call the index view
        /// </summary>
        /// <returns>
        /// The index view
        /// </returns>
        [HttpGet]
        public virtual ActionResult Index()
        {
            var categories = this.manageCategoryProvider.GetAllCategories().OrderBy(x => x.Value).Select(x => new SelectListItem
            {
                Text = x.Value,
                Value = x.Key.ToString()
            }).ToList();

            var stores = this.manageStoreProvider.GetAllStores().OrderBy(x => x.Value).Select(x => new SelectListItem
            {
                Text = x.Value,
                Value = x.Key.ToString()
            }).ToList();

            var ingredients = this.manageIngredientProvider.GetAllIngredients().OrderBy(x => x.Value).Select(x => new SelectListItem
            {
                Text = x.Value,
                Value = x.Key.ToString()
            }).ToList();
            var model = new NixJqGridProductsByUserModel(categories, stores,ingredients);
            return this.View(model);
        }


        #region AJAX actions

        /// <summary>
        /// Get data for AjaxJqGrid
        /// </summary>
        /// <param name="gridContext">The grid context</param>
        /// <returns>Returns the view</returns>
        public JsonResult GetDataForAjaxGrid(NixJqGridContext gridContext)
        {
            var products = this.manageProductByUserProvider.GetAllProducts().ToList();

            var viewModel = products.Select(p => new ProductByUserViewModel()
            {
                Category = p.Category!= null ? p.Category.Name : null,
                Date = p.Date.ToString("MM/dd/yyyy"),
                ExpirationDate = p.ExpirationDate.ToString(),
                Id = p.Id,
                Ingredient = p.Ingredient,
                Name = p.Name,
                Store = p.Store != null ? p.Store.Name : null,
                PosDescription = p.ShortName,
                UpcCode = p.UpcCode,
                UsersEmail = p.User.Email,
            });

            return gridContext.Response(viewModel, isAllDataSearch: true);
        }

        [Transaction]
        public virtual void EditAjaxGrid(ProductByUserViewModel product)
        {
            var id = Convert.ToInt64(product.Id);
            var categoryId = Convert.ToInt64(product.Category);
            var storeId = Convert.ToInt64(product.Store);
            var user = this.manageUserProvider.GetUser(product.UsersEmail);
            this.manageProductByUserProvider.Save(product.UpcCode, product.PosDescription, product.Name, product.Ingredient.Term, categoryId, 
                storeId, user, Convert.ToInt32(product.ExpirationDate), id);
        }


        [Transaction]
        public void Approve(long id)
        {
            var product = this.manageProductByUserProvider.GetProduct(id);

            if (product == null)
            {
                throw new HttpException(404, string.Format("ProductByuser with id [{0}] not found.", id));
            }
            this.manageProductProvider.Save(product.ShortName, product.Name, product.Ingredient.Term, product.Category.Id, product.Store.Id, upcCode:product.UpcCode);
            this.manageProductByUserProvider.Remove(product);
        }

        /// <summary>
        /// Delete the row from ajax grid
        /// </summary>
        /// <param name="id">The id-code.</param>
        [Transaction]
        public virtual void DeleteAjaxGrid(long id)
        {
            var product = this.manageProductByUserProvider.GetProduct(id);

            if (product == null)
            {
                throw new HttpException(404, string.Format("ProductByuser with id [{0}] not found.", id));
            }
            this.manageProductByUserProvider.Remove(product);
        }


        #endregion AJAX actions
    }
}
