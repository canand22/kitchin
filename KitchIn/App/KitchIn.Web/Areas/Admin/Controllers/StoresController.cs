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
    public class StoresController : Controller
    {
        private readonly IRepository<Store> storesRepository;

        public StoresController(IRepository<Store> storesRepository)
        {
            this.storesRepository = storesRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new NixJqGridStoreModel();

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
            var stores = this.storesRepository.ToList();

            var viewModel = stores.Select(p => new StoreViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Latitude = p.Latitude,
                Longitude = p.Longitude
            });

            return gridContext.Response(viewModel, isAllDataSearch: true);
        }

        [Transaction]
        public void EditAjaxGrid(StoreViewModel model)
        {
            if (ModelState.IsValid)
            {
                var store = model.Id.HasValue ? this.storesRepository.Get(model.Id.Value) : new Store();

                store.Name = model.Name;
                store.Latitude = model.Latitude.Value;
                store.Longitude = model.Longitude.Value;

                this.storesRepository.Save(store);
            }
        }

        /// <summary>
        /// Delete the row from ajax grid
        /// </summary>
        /// <param name="id">The id-code.</param>
        [Transaction]
        public void DeleteAjaxGrid(long id)
        {
            var store = this.storesRepository.Get(id);

            if (store == null)
            {
                throw new HttpException(404, string.Format("Store with id [{0}] not found.", id));
            }

            this.storesRepository.Remove(store);
        }

        #endregion AJAX actions
    }
}
