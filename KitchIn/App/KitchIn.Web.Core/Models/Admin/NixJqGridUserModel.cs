using System.Collections.Generic;
using System.Web.Mvc;
using SmartArch.NixJqGridFramework.Core;
using SmartArch.NixJqGridFramework.Core.Extensions;
using SmartArch.NixJqGridFramework.Core.NixJqGridParts;
using SmartArch.NixJqGridFramework.Core.NixJqGridParts.Enums;

namespace KitchIn.Web.Core.Models.Admin
{
    /// <summary>
    /// The NixJqGridEdit user
    /// </summary>
    public class NixJqGridUserModel
    {
        /// <summary>
        /// Gets or sets AjaxGrid.
        /// </summary>
        public NixJqGrid<UserViewModel> AjaxGrid { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NixJqGridUserModel"/> class.
        /// </summary>
        /// <param name="roles">
        /// The list roles.
        /// </param>
        public NixJqGridUserModel(List<SelectListItem> roles)
        {
            this.AjaxGrid =
               new NixJqGrid<UserViewModel>(
                   new ActionUrl("Users/GetDataForAjaxGrid", "Users/EditAjaxGrid", "Users/DeleteAjaxGrid", null),
                   new List<NixJqGridColumn<UserViewModel>>
                       {
                           new NixJqGridColumn<UserViewModel>(x => x.Id).Hidden(),
                           new NixJqGridColumn<UserViewModel>(x => x.FirstName).Set(p => p.Searchable, true),
                           new NixJqGridColumn<UserViewModel>(x => x.LastName).Set(p => p.Searchable, true),
                           new NixJqGridColumn<UserViewModel>(x => x.Email).Set(p => p.Searchable, true),
                           new NixJqGridColumn<UserViewModel>(x => x.Role).SetDropDownList(roles)
                       })
                        .Id(c => c.Id).ShowFilterToolBar();

            this.AjaxGrid.ScrollPagingEnabled = false;

            this.AjaxGrid.MultiSelectEnabled = true;

            this.AjaxGrid.InlineEditNavSettings.InLineEditEnabled = true;
            this.AjaxGrid.InlineEditNavSettings.ShowEditButton = true;
            this.AjaxGrid.InlineEditNavSettings.ShowSaveButton = true;
            this.AjaxGrid.InlineEditNavSettings.ShowCancelButton = true;
            this.AjaxGrid.SubGridOptions.SubGridEnaled = false;

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

            this.AjaxGrid[c => c.FirstName].ColumnName = "First Name";
            this.AjaxGrid[c => c.FirstName].Searchable = true;
            this.AjaxGrid[c => c.FirstName].Width = 200;

            this.AjaxGrid[c => c.LastName].ColumnName = "Last Name";
            this.AjaxGrid[c => c.LastName].Searchable = true;
            this.AjaxGrid[c => c.LastName].Width = 200;


            this.AjaxGrid[c => c.Email].ColumnName = "Login/Email";
            this.AjaxGrid[c => c.Email].Searchable = true;
            this.AjaxGrid[c => c.Email].Width = 300;

            this.AjaxGrid[c => c.Role].ColumnName = "Role";
            this.AjaxGrid[c => c.Role].Searchable = true;
            this.AjaxGrid[c => c.Role].Width = 100;

            this.AjaxGrid.CanMultipleSearch();
            this.AjaxGrid.AltRows = false;
            this.AjaxGrid.AutoEncode = true;
            this.AjaxGrid.Caption = string.Empty;
        }
    }
}