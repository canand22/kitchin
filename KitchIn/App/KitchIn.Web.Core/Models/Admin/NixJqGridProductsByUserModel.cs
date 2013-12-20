using System.Collections.Generic;
using System.Web.Mvc;

using SmartArch.NixJqGridFramework.Core;
using SmartArch.NixJqGridFramework.Core.Extensions;
using SmartArch.NixJqGridFramework.Core.NixJqGridParts;
using SmartArch.NixJqGridFramework.Core.NixJqGridParts.Enums;

namespace KitchIn.Web.Core.Models.Admin
{
    public class NixJqGridProductsByUserModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NixJqGridProductsByUserModel"/> class. 
        /// </summary>
        /// <param name="categories">
        /// The list categories.
        /// </param>
        public NixJqGridProductsByUserModel(List<SelectListItem> categories)
        {
            this.AjaxGrid =
               new NixJqGrid<ProductViewModel>(
                   new ActionUrl("ProductsByUser/GetDataForAjaxGrid", "ProductsByUser/EditAjaxGrid", "ProductsByUser/DeleteAjaxGrid", null),
                   new List<NixJqGridColumn<ProductViewModel>>
                       {
                           new NixJqGridColumn<ProductViewModel>(x => x.Id).Hidden(),
                           new NixJqGridColumn<ProductViewModel>(x => x.Name).Set(p => p.Searchable, true),
                           new NixJqGridColumn<ProductViewModel>(x => x.Category).SetDropDownList(categories),
                           new NixJqGridColumn<ProductViewModel>(x => x.ExpirationDate),
                           new NixJqGridColumn<ProductViewModel>(x => x.ApproveRow).SetCustomFormatterFunction("approveProductLink"),
                           new NixJqGridColumn<ProductViewModel>(x => x.DeclineRow).SetCustomFormatterFunction("declineProductLink")
                       })
                        .Id(c => c.Id).ShowFilterToolBar();

            this.AjaxGrid.ScrollPagingEnabled = false;
            this.AjaxGrid.InlineEditNavSettings.InLineEditEnabled = true;
            this.AjaxGrid.SubGridOptions.SubGridEnaled = false;
            
            this.AjaxGrid.ToolBarSettings.ToolBarPosition = ToolBarPosition.topAndBottom;
            this.AjaxGrid.MultiSelectEnabled = true;

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
            
            this.AjaxGrid.ShowRowNumbers = true;

            this.AjaxGrid.PagerSettings.NoRowsMessage = string.Empty;
            this.AjaxGrid.PagerSettings.PageSizeOptions = new List<int> { 20, 30, 50 };
            this.AjaxGrid.PagerSettings.PageSize = 20;

            this.AjaxGrid[c => c.Id].Align = ColumnAlign.center;
            this.AjaxGrid[c => c.Id].ColumnName = string.Empty;
            this.AjaxGrid[c => c.Id].Sortable = false;
            this.AjaxGrid[c => c.Id].Width = 100;

            this.AjaxGrid[c => c.Name].Searchable = true;
            this.AjaxGrid[c => c.Name].Width = 250;

            this.AjaxGrid[c => c.Category].Searchable = true;
            this.AjaxGrid[c => c.Category].Width = 250;

            this.AjaxGrid[c => c.ExpirationDate].Sortable = false;
            this.AjaxGrid[c => c.ExpirationDate].Width = 250;

            this.AjaxGrid[c => c.ApproveRow].Align = ColumnAlign.center;
            this.AjaxGrid[c => c.ApproveRow].Editable = false;
            this.AjaxGrid[c => c.ApproveRow].ColumnName = string.Empty;
            this.AjaxGrid[c => c.ApproveRow].Searchable = false;
            this.AjaxGrid[c => c.ApproveRow].Sortable = false;
            this.AjaxGrid[c => c.ApproveRow].Width = 70;

            this.AjaxGrid[c => c.DeclineRow].Align = ColumnAlign.center;
            this.AjaxGrid[c => c.DeclineRow].Editable = false;
            this.AjaxGrid[c => c.DeclineRow].ColumnName = string.Empty;
            this.AjaxGrid[c => c.DeclineRow].Searchable = false;
            this.AjaxGrid[c => c.DeclineRow].Sortable = false;
            this.AjaxGrid[c => c.DeclineRow].Width = 70;
           
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