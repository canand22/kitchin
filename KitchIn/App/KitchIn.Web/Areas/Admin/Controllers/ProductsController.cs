using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using KitchIn.Core.Entities;
using KitchIn.Core.Services.CSVParser;
using KitchIn.Core.Services.Uploader;
using KitchIn.Web.Core.Models.Admin;
using SmartArch.Data;
using SmartArch.NixJqGridFramework.Helpers.ModelBinder;
using SmartArch.Web.Attributes;

namespace KitchIn.Web.Areas.Admin.Controllers
{
    using KitchIn.Core.Enums;
    using KitchIn.Core.Interfaces;

    [Authorize]
    ////(Roles = "Administrator")
    public class ProductsController : Controller
    {
        /// <summary>
        /// The manage Product Provider
        /// </summary>
        protected readonly IManageProductProvider manageProductProvider;

        /// <summary>
        /// The manage Product Provider
        /// </summary>
        protected readonly IManageCategoryProvider manageCategoryProvider;

        /// <summary>
        /// The manage Product Provider
        /// </summary>
        protected readonly IManageStoreProvider manageStoreProvider;

        /// <summary>
        /// The manage Kitchen Provider
        /// </summary>
        protected readonly IManageKitchenProvider manageKitchenProvider;

        /// <summary>
        /// The manage Ingredient Provider
        /// </summary>
        protected readonly IManageIngredientProvider manageIngredientProvider;


        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="manageProductProvider">
        /// The manage Product Provider.
        /// </param>
        /// <param name="manageCategoryProvider">
        /// The manage Category Provider.
        /// </param>
        /// <param name="manageStoreProvider">
        /// The manage Store Provider.
        /// </param>
        /// <param name="manageKitchenProvider">
        /// The manage Kitchen Provider.
        /// </param>
        /// /// <param name="manageIngredientProvider">
        /// The manage Ingredient Provider.
        /// </param>
        public ProductsController(IManageProductProvider manageProductProvider, IManageCategoryProvider manageCategoryProvider, 
            IManageStoreProvider manageStoreProvider, IManageKitchenProvider manageKitchenProvider, IManageIngredientProvider manageIngredientProvider)
        {
            this.manageProductProvider = manageProductProvider;
            this.manageCategoryProvider = manageCategoryProvider;
            this.manageStoreProvider = manageStoreProvider;
            this.manageKitchenProvider = manageKitchenProvider;
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
            var model = new NixJqGridProductModel(categories, stores,ingredients);
            return this.View(model);
        }

        ///// <summary>
        ///// Call view AddUser
        ///// </summary>
        ///// <returns>The AddUser view</returns>
        //[HttpGet]
        //public ActionResult AddProduct()
        //{
        //    var product = new Product();

        //    var model = Mapper.Map<Product, ProductViewModel>(product);

        //    return this.View("EditProduct", model);
        //}

        ///// <summary>
        ///// Create new user
        ///// </summary>
        ///// <param name="model">The model.</param>
        ///// <returns>Login view</returns>
        //[HttpPost]
        //[Transaction]
        //public ActionResult AddProduct(ProductViewModel model)
        //{
        //    if (!this.ModelState.IsValid)
        //    {
        //        return this.View("EditUser", model);
        //    }

        //    var product = Mapper.Map<ProductViewModel, Product>(model);
        //    this.repositoryProduct.Save(product);

        //    return this.View("EditUserSuccess");
        //}

        ///// <summary>
        ///// Call view EditUser
        ///// </summary>
        ///// <param name="id">The product id-code.</param>
        ///// <returns>The view ProductEdit</returns>
        //[HttpGet]
        //public ActionResult EditProduct(int id)
        //{
        //    var product = this.repositoryProduct.First(x => x.Id.Equals(id));

        //    var model = Mapper.Map<Product, ProductViewModel>(product);

        //    return this.View(model);
        //}

        ///// <summary>
        ///// Edit user data
        ///// </summary>
        ///// <param name="model">The edit product data</param>
        ///// <returns>View SuccessfullUpdata</returns>
        //[HttpPost]
        //[Transaction]
        //public ActionResult EditUser(ProductViewModel model)
        //{
        //    if (!this.ModelState.IsValid)
        //    {
        //        return this.View(model);
        //    }

        //    var product = this.repositoryProduct.First(x => x.Id.Equals(model.Id));
        //    product = Mapper.Map(model, product);
        //    this.repositoryProduct.Save(product);

        //    return View("EditUserSuccess");
        //}

        #region AJAX actions

        /// <summary>
        /// Get data for AjaxJqGrid
        /// </summary>
        /// <param name="gridContext">The grid context</param>
        /// <returns>Returns the view</returns>
        public virtual JsonResult GetDataForAjaxGrid(NixJqGridContext gridContext)
        {
            var products = this.manageProductProvider.GetAllProducts().ToList();
            var viewModel = products.Select(p => new ProductViewModel
                                                     {
                                                         Id = p.Id,
                                                         Category = p.Category.Name,
                                                         Name = p.Name,
                                                         PosDescription = p.ShortName,
                                                         TypeAdd = p.TypeAdd.ToString(),
                                                         IngredientName = p.IngredientName,
                                                         Ingredient = p.Ingredient,
                                                         ModificationDate = p.ModificationDate.ToString("MM/dd/yyyy HH:mm"),
                                                         Store = p.Store.Name
                                                     });
            return gridContext.Response(viewModel, isAllDataSearch: true);
        }

        [Transaction]
        public virtual void EditAjaxGrid(ProductViewModel model)
        {
            var id = Convert.ToInt64(model.Id);
            var categoryId = Convert.ToInt64(model.Category);
            var storeId = Convert.ToInt64(model.Store);
            this.manageProductProvider.Save(model.PosDescription, model.Name, model.Ingredient.Term, categoryId, storeId, id);
        }

        /// <summary>
        /// Delete the row from ajax grid
        /// </summary>
        /// <param name="id">The id-code.</param>
        [Transaction]
        public virtual void DeleteAjaxGrid(long id)
        {
            var product = this.manageProductProvider.GetProduct(id);

            if (product == null)
            {
                throw new HttpException(404, string.Format("Product with id [{0}] not found.", id));
            }

            //foreach (var prod in this.manageKitchenProvider.GetAllProducts().Where(x => x.Product.Id == id))
            //{
            //    prod.Product = null;
            //}
            
            //this.repositoryProductsOnKitchens.SaveChanges();

            this.manageProductProvider.Remove(product);
        }

        [HttpPost]
        public FileUploaderResult UploadFiles()
        {
            var request = HttpContext.Request;
            var formUpload = request.Files.Count > 0;

            // find filename
            var xFileName = request.Headers["X-File-Name"];
            var qqFile = request["qqfile"];
            var formFilename = formUpload ? request.Files[0].FileName : null;
            var upload = new FileUploader
            {
                Filename = xFileName ?? qqFile ?? formFilename,
                InputStream = formUpload ? request.Files[0].InputStream : request.InputStream
            };
            try
            {
                //var parser = new CSVParser(upload.InputStream);
            }

            catch (Exception ex)
            {
                return new FileUploaderResult(false, error: ex.Message);
            }

            // the anonymous object in the result below will be convert to json and set back to the browser
            return new FileUploaderResult(true, new { extraInformation = 12345 });
        }

        #endregion AJAX actions
    }
}
