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
    [Authorize]
    ////(Roles = "Administrator")
    public class ProductsController : Controller
    {
        /// <summary>
        /// The category repository
        /// </summary>
        protected readonly IRepository<Category> repositoryCategory;
        
        /// <summary>
        /// The product repository
        /// </summary>
        protected readonly IRepository<Product> repositoryProduct;


        private readonly IRepository<ProductsOnKitchen> repositoryProductsOnKitchens;
 
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="repositoryCategory">
        /// The repository Category.
        /// </param>
        /// <param name="repositoryProduct">
        /// The repository Product.
        /// </param>
        public ProductsController(
            IRepository<Category> repositoryCategory,
            IRepository<Product> repositoryProduct,
            IRepository<ProductsOnKitchen> repositoryProductsOnKitchens)
        {
            this.repositoryCategory = repositoryCategory;
            this.repositoryProduct = repositoryProduct;
            this.repositoryProductsOnKitchens = repositoryProductsOnKitchens;
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
            var categories = this.repositoryCategory.OrderBy(x => x.Name).Select(x => new SelectListItem
                                                    {
                                                        Text = x.Name,
                                                        Value = x.Id.ToString()
                                                    }).ToList();
            var model = new NixJqGridProductModel(categories);

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
            var products = this.repositoryProduct.ToList();

            var viewModel = products.Where(x => !x.IsAddedByUser).Select(p => new ProductViewModel
                                                     {
                                                         Id = p.Id,
                                                         Category = p.Category.Name,
                                                         ExpirationDate = p.ExpirationDate,
                                                         Name = p.Name
                                                     });

            return gridContext.Response(viewModel, isAllDataSearch: true);
        }

        [Transaction]
        public virtual void EditAjaxGrid(ProductViewModel model)
        {
            var product = model.Id != 0 ? this.repositoryProduct.Get(model.Id) : new Product();

            product.Category = this.repositoryCategory.Get(Convert.ToInt64(model.Category));
            product.Name = model.Name;
            product.ExpirationDate = model.ExpirationDate;
        
            this.repositoryProduct.Save(product);
        }

        /// <summary>
        /// Delete the row from ajax grid
        /// </summary>
        /// <param name="id">The id-code.</param>
        [Transaction]
        public virtual void DeleteAjaxGrid(long id)
        {
            var product = this.repositoryProduct.Get(id);

            if (product == null)
            {
                throw new HttpException(404, string.Format("Product with id [{0}] not found.", id));
            }

            foreach (var prod in this.repositoryProductsOnKitchens.Where(x => x.Product.Id == id))
            {
                prod.Product = null;
            }
            
            this.repositoryProductsOnKitchens.SaveChanges();

            this.repositoryProduct.Remove(product);
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
