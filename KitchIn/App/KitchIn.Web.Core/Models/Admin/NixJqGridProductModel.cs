using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KitchIn.Core.Entities;
using SmartArch.NixJqGridFramework.Core;
using SmartArch.NixJqGridFramework.Core.Extensions;
using SmartArch.NixJqGridFramework.Core.NixJqGridParts;
using SmartArch.NixJqGridFramework.Core.NixJqGridParts.Enums;

namespace KitchIn.Web.Core.Models.Admin
{
    public class NixJqGridProductModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NixJqGridProductModel"/> class.
        /// </summary>
        /// <param name="categories">
        /// The list categories.
        /// </param>
        public NixJqGridProductModel(List<SelectListItem> categories, List<SelectListItem> stores)
        {
            this.AjaxGrid =
               new NixJqGrid<ProductViewModel>(
                   new ActionUrl("Products/GetDataForAjaxGrid", "Products/EditAjaxGrid", "Products/DeleteAjaxGrid", null),
                   new List<NixJqGridColumn<ProductViewModel>>
                       {
                           new NixJqGridColumn<ProductViewModel>(x => x.Id).Hidden(),
                           new NixJqGridColumn<ProductViewModel>(x => x.PosDescription).Set(p => p.Searchable, true),
                           new NixJqGridColumn<ProductViewModel>(x => x.Name).Set(p => p.Searchable, true),
                           new NixJqGridColumn<ProductViewModel>(x => x.IngredientName).Set(p => p.Searchable, true),
                           new NixJqGridColumn<ProductViewModel>(x => x.Category).SetDropDownList(categories),
                           new NixJqGridColumn<ProductViewModel>(x => x.TypeAdd).Set(p => p.Searchable, true),
                           new NixJqGridColumn<ProductViewModel>(x => x.Store).SetDropDownList(stores),
                           new NixJqGridColumn<ProductViewModel>(x => x.ModificationDate).Set(p => p.Searchable, true),
                           new NixJqGridColumn<ProductViewModel>(x => x.ExpirationDate)
                       })
                        .Id(c => c.Id).ShowFilterToolBar();

            this.AjaxGrid.ScrollPagingEnabled = false;
            this.AjaxGrid.InlineEditNavSettings.InLineEditEnabled = true;
            this.AjaxGrid.SubGridOptions.SubGridEnaled = false;

            this.AjaxGrid.MultiSelectEnabled = true;

            this.AjaxGrid.ToolBarSettings.ToolBarPosition = ToolBarPosition.topAndBottom;

            this.AjaxGrid.ShowToolBar = true;
            this.AjaxGrid.InlineEditNavSettings.ShowAddButton = true;
            this.AjaxGrid.InlineEditNavSettings.ShowEditButton = true;
            this.AjaxGrid.InlineEditNavSettings.ShowSaveButton = true;
            this.AjaxGrid.InlineEditNavSettings.ShowCancelButton = true;
            
            this.AjaxGrid.ToolBarSettings.ShowRefreshButton = true;
            this.AjaxGrid.ToolBarSettings.ShowEditButton = false;
            this.AjaxGrid.ToolBarSettings.ShowAddButton = false;
            this.AjaxGrid.ToolBarSettings.ShowDeleteButton = false;
            this.AjaxGrid.ToolBarSettings.ShowSearchButton = false;


            this.AjaxGrid.ToolBarSettings.CustomButtons.Add(new NixJqGridToolBarCustomButton
            {
                Id = "#del_" + this.AjaxGrid.Id,
                Caption = string.Empty,
                Title = "Delete",
                ButtonIcon = "ui-icon ui-icon-trash",
                OnClickFunction = "DeleteSelectedRow",
                Position = ToolBarButtonPosition.last
            });

            this.AjaxGrid.ToolBarSettings.CustomButtons.Add(new NixJqGridToolBarCustomButton
                                                            {
                                                                ButtonIcon = "ui-icon ui-icon-transferthick-e-w",
                                                                Caption = string.Empty,
                                                                Title = "Upload file",
                                                                OnClickFunction = "uploadFiles"
                                                            });
            this.AjaxGrid.ToolBarSettings.CustomButtons.Add(new NixJqGridToolBarCustomButton
                                                                {
                                                                    ButtonIcon = "ui-icon ui-icon-folder-open",
                                                                    Caption = string.Empty,
                                                                    Title = "Select csv file",
                                                                    Position = ToolBarButtonPosition.first,
                                                                    OnClickFunction = "selectFiles"
                                                                });
            this.AjaxGrid.ShowRowNumbers = true;

            this.AjaxGrid.PagerSettings.NoRowsMessage = string.Empty;
            this.AjaxGrid.PagerSettings.PageSizeOptions = new List<int> { 20, 30, 50 };
            this.AjaxGrid.PagerSettings.PageSize = 20;


            this.AjaxGrid[c => c.Id].Align = ColumnAlign.center;
            this.AjaxGrid[c => c.Id].ColumnName = string.Empty;
            this.AjaxGrid[c => c.Id].Sortable = false;
            this.AjaxGrid[c => c.Id].Width = 100;

            this.AjaxGrid[c => c.PosDescription].Searchable = true;
            this.AjaxGrid[c => c.PosDescription].Width = 150;

            this.AjaxGrid[c => c.Name].Searchable = true;
            this.AjaxGrid[c => c.Name].Width = 250;

            this.AjaxGrid[c => c.IngredientName].Searchable = true;
            this.AjaxGrid[c => c.IngredientName].Width = 250;

            this.AjaxGrid[c => c.Category].Searchable = true;
            this.AjaxGrid[c => c.Category].Width = 220;

            this.AjaxGrid[c => c.TypeAdd].Searchable = true;
            this.AjaxGrid[c => c.TypeAdd].Width = 70;

            this.AjaxGrid[c => c.Store].Searchable = true;
            this.AjaxGrid[c => c.Store].Width = 70;

            this.AjaxGrid[c => c.ModificationDate].Searchable = true;
            this.AjaxGrid[c => c.ModificationDate].Width = 120;
            //this.AjaxGrid[c => c.ModificationDate].Formatters = Formatter.date;
            this.AjaxGrid[c => c.ExpirationDate].Sortable = false;
            this.AjaxGrid[c => c.ExpirationDate].Width = 100;
            
            this.AjaxGrid.CanMultipleSearch();
            this.AjaxGrid.AltRows = false;
            this.AjaxGrid.AutoEncode = true;
            this.AjaxGrid.Caption = string.Empty;
        }

        /// <summary>
        /// Gets or sets AjaxGrid.
        /// </summary>
        public NixJqGrid<ProductViewModel> AjaxGrid { get; set; }
    }
}