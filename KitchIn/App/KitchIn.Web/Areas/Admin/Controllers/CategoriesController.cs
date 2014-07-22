using System;
using System.Collections.Generic;
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
    [Authorize]
    public class CategoriesController : Controller
    {
        /// <summary>
        /// The category repository
        /// </summary>
        private readonly IRepository<Category> repositoryCategory;
 
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="repositoryCategory">
        /// The repository Category.
        /// </param>
        /// <param name="repositoryProduct">
        /// The repository Product.
        /// </param>
        public CategoriesController(IRepository<Category> repositoryCategory)
        {
            this.repositoryCategory = repositoryCategory;
        }


        public ActionResult Index()
        {
            var model = new NixJqGridCategoryModel();
            return View(model);
        }

        #region AJAX actions

        /// <summary>
        /// Get data for AjaxJqGrid
        /// </summary>
        /// <param name="gridContext">The grid context</param>
        /// <returns>Returns the view</returns>
        public JsonResult GetDataForAjaxGrid(NixJqGridContext gridContext)
        {
            var categories = this.repositoryCategory.ToList();

            var viewModel = categories.Select(p => new CategoryViewModel
            {
                Id = p.Id,
                Name = p.Name, 
                Description = p.Description
            });

            return gridContext.Response(viewModel, isAllDataSearch: true);
        }

        [Transaction]
        public void EditAjaxGrid(CategoryViewModel model)
        {
            var category = model.Id != 0 ? this.repositoryCategory.Get(model.Id) : new Category();

            category.Name = model.Name;
            category.Description = model.Description;

            this.repositoryCategory.Save(category);
        }

        /// <summary>
        /// Delete the row from ajax grid
        /// </summary>
        /// <param name="id">The id-code.</param>
        [Transaction]
        public void DeleteAjaxGrid(string id)
        {
            var ids = id.Split(',');

            foreach (var i in ids)
            {
                var category = this.repositoryCategory.Get(Convert.ToInt64(i));

                if (category == null)
                {
                    throw new HttpException(404, string.Format("Product with id [{0}] not found.", id));
                }

                //if (user.Login.Equals(User.Identity.Name, StringComparison.InvariantCultureIgnoreCase))
                //{
                //    throw new HttpException(403, string.Format("You can't remove current user"));
                //}

                this.repositoryCategory.Remove(category);
            }
        }

        #endregion AJAX actions
    }
}
