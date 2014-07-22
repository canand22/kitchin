using System.Collections.Generic;
using SmartArch.NixJqGridFramework.Core;
using SmartArch.NixJqGridFramework.Core.Extensions;
using SmartArch.NixJqGridFramework.Core.NixJqGridParts;
using SmartArch.NixJqGridFramework.Core.NixJqGridParts.Enums;

namespace KitchIn.Web.Core.Models.Admin
{
    public class NixJqGridCategoryModel
    {
        /// <summary>
        /// Gets or sets AjaxGrid.
        /// </summary>
        public NixJqGrid<CategoryViewModel> AjaxGrid { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NixJqGridUserModel"/> class.
        /// </summary>
        public NixJqGridCategoryModel()
        {
            this.AjaxGrid =
               new NixJqGrid<CategoryViewModel>(
                   new ActionUrl("Categories/GetDataForAjaxGrid", "Categories/EditAjaxGrid", "Categories/DeleteAjaxGrid", null),
                   new List<NixJqGridColumn<CategoryViewModel>>
                       {
                           new NixJqGridColumn<CategoryViewModel>(x => x.Id).Hidden(),
                           new NixJqGridColumn<CategoryViewModel>(x => x.Name).Set(p => p.Searchable, true),
                           new NixJqGridColumn<CategoryViewModel>(x => x.Description)
                       })
                        .Id(c => c.Id).ShowFilterToolBar();

            this.AjaxGrid.ScrollPagingEnabled = false;
            this.AjaxGrid.InlineEditNavSettings.InLineEditEnabled = true;
            this.AjaxGrid.InlineEditNavSettings.ShowAddButton = true;
            this.AjaxGrid.InlineEditNavSettings.ShowEditButton = true;
            this.AjaxGrid.InlineEditNavSettings.ShowSaveButton = true;
            this.AjaxGrid.InlineEditNavSettings.ShowCancelButton = true;
            this.AjaxGrid.SubGridOptions.SubGridEnaled = false;

            this.AjaxGrid.MultiSelectEnabled = true;
            this.AjaxGrid.ShowToolBar = true;
            this.AjaxGrid.ToolBarSettings.ToolBarPosition = ToolBarPosition.topAndBottom;
            this.AjaxGrid.ToolBarSettings.ShowRefreshButton = true;
            this.AjaxGrid.ToolBarSettings.ShowEditButton = false;
            this.AjaxGrid.ToolBarSettings.ShowAddButton = false;
            this.AjaxGrid.ToolBarSettings.ShowDeleteButton = false;
            this.AjaxGrid.ToolBarSettings.ShowSearchButton = false;

            this.AjaxGrid.ShowRowNumbers = true;

            this.AjaxGrid.PagerSettings.NoRowsMessage = string.Empty;
            this.AjaxGrid.PagerSettings.PageSizeOptions = new List<int> { 20, 30, 50 };
            this.AjaxGrid.PagerSettings.PageSize = 20;

            this.AjaxGrid.ToolBarSettings.CustomButtons.Add(new NixJqGridToolBarCustomButton
            {
                Id = "#del_" + this.AjaxGrid.Id,
                Caption = string.Empty,
                Title = "Delete",
                ButtonIcon = "ui-icon ui-icon-trash",
                OnClickFunction = "DeleteSelectedRow",
                Position = ToolBarButtonPosition.last
            });

            this.AjaxGrid[c => c.Name].ColumnName = "Category title";
            this.AjaxGrid[c => c.Name].Searchable = true;
            this.AjaxGrid[c => c.Name].Width = 250;

            this.AjaxGrid[c => c.Description].ColumnName = "Description";
            this.AjaxGrid[c => c.Description].Sortable = false;
            this.AjaxGrid[c => c.Description].Width = 450;

            this.AjaxGrid.CanMultipleSearch();
            this.AjaxGrid.AltRows = false;
            this.AjaxGrid.AutoEncode = true;
            this.AjaxGrid.Caption = string.Empty;
        }
    } 
}