using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AutoMapper;
using KitchIn.Core.Enums;
using Microsoft.Practices.ServiceLocation;

using SmartArch.Data;
using SmartArch.NixJqGridFramework.Helpers.ModelBinder;
using KitchIn.Core.Entities;
using KitchIn.Core.Interfaces;
using KitchIn.Web.Core.Models.Admin;
using SmartArch.Web.Attributes;

namespace KitchIn.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The user management controller
    /// </summary>
    [Authorize]
    ////(Roles = "Administrator")
    public class UsersController : Controller
    {
        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IRepository<User> repositoryUser;

        /// <summary>
        /// The role repository
        /// </summary>
        private readonly IRepository<Role> repositoryRole;

        /// <summary>
        /// The membership provider
        /// </summary>
        private readonly IMembershipProvider membershipProvider;

        static UsersController()
        {
            ////Mapper.CreateMap<User, UserViewModel>().ForMember(d => d.Roles, o => o.MapFrom(src => src.Roles.Select(r => r.Name).ToList()));
            Mapper.CreateMap<User, UserEditModel>().ForMember(um => um.Roles, a => a.MapFrom(u => GetUserRoleModels(u)));
            Mapper.CreateMap<Role, EditRoleModel>();
        }

        private static List<EditRoleModel> GetUserRoleModels(User user)
        {
            return ServiceLocator.Current.GetInstance<IRepository<Role>>()
                .ToList()
                .Select(r => new EditRoleModel { Id = r.Id, Name = r.Name, IsChecked = true })
                .ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="repositoryUser">
        /// The repository User.
        /// </param>
        /// <param name="repositoryRole">
        /// The repository Role.
        /// </param>
        /// <param name="membershipProvider">
        /// The membership Provider.
        /// </param>
        public UsersController(IRepository<User> repositoryUser, IRepository<Role> repositoryRole, IMembershipProvider membershipProvider)
        {
            this.repositoryUser = repositoryUser;
            this.repositoryRole = repositoryRole;
            this.membershipProvider = membershipProvider;
        }

        /// <summary>
        /// Call the index view
        /// </summary>
        /// <returns>
        /// The index view
        /// </returns>
        [HttpGet]
        public ActionResult Index()
        {
            var roles = Enum.GetValues(typeof(UserRoles)).Cast<int>().Select(x => new SelectListItem
                                                                                       {
                                                                                           Value = x.ToString(),
                                                                                           Text = ((UserRoles)x).ToString()
                                                                                       }).ToList();
            roles.Single(x => x.Text == @"User").Selected = true;
            var model = new NixJqGridUserModel(roles);
            var role = this.repositoryRole.ToList();
            this.ViewBag.Roles = role;

            return this.View(model);
        }

        ///// <summary>
        ///// Call view AddUser
        ///// </summary>
        ///// <returns>The AddUser view</returns>
        //[HttpGet]
        //public ActionResult AddUser()
        //{
        //    var user = new User();

        //    UserEditModel model = Mapper.Map<User, UserEditModel>(user);

        //    return this.View("EditUser", model);
        //}

        ///// <summary>
        ///// Create new user
        ///// </summary>
        ///// <param name="model">The model.</param>
        ///// <returns>Login view</returns>
        //[HttpPost]
        //[Transaction]
        //public ActionResult AddUser(UserEditModel model)
        //{
        //    if (!this.ModelState.IsValid)
        //    {
        //        return this.View("EditUser", model);
        //    }

        //    this.membershipProvider.CreateUser(model);

        //    return this.View("EditUserSuccess");
        //}

        ///// <summary>
        ///// Call view EditUser
        ///// </summary>
        ///// <param name="id">The user id-code.</param>
        ///// <returns>The view UserEdit</returns>
        //[HttpGet]
        //public ActionResult EditUser(int id)
        //{
        //    var user = this.repositoryUser.First(x => x.Id.Equals(id));

        //    UserEditModel model = Mapper.Map<User, UserEditModel>(user);

        //    return this.View(model);
        //}

        ///// <summary>
        ///// Edit user data
        ///// </summary>
        ///// <param name="editUser">The edit user data</param>
        ///// <returns>View SuccessfullUpdata</returns>
        //[HttpPost]
        //[Transaction]
        //public ActionResult EditUser(UserEditModel editUser)
        //{
        //    if (!this.ModelState.IsValid)
        //    {
        //        return this.View(editUser);
        //    }

        //    this.membershipProvider.UpdateUser(editUser);

        //    return View("EditUserSuccess");
        //}

        #region AJAX actions

        /// <summary>
        /// Get data for AjaxJqGrid
        /// </summary>
        /// <param name="gridContext">The grid context</param>
        /// <returns>Returns the view</returns>
        public JsonResult GetDataForAjaxGrid(NixJqGridContext gridContext)
        {
            IList<User> us = this.repositoryUser.ToList();
            IList<UserViewModel> viewModel = us.Select(user => new UserViewModel
                                                                   {
                                                                       Id = user.Id,
                                                                       FirstName = user.FirstName,
                                                                       LastName = user.LastName,
                                                                       Email = user.Email,
                                                                       Password = user.Password,
                                                                       Role = user.Role.ToString()
                                                                   }).ToList();

            return gridContext.Response(viewModel);
        }

        [Transaction]
        public void EditAjaxGrid(UserViewModel model)
        {
            var user = this.repositoryUser.Get(model.Id);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Password = model.Password;
            user.Role = model.Role == "0" ? UserRoles.Admin : UserRoles.User;

            this.repositoryUser.SaveChanges();
        }

        /// <summary>
        /// Delete the row from ajax grid
        /// </summary>
        /// <param name="id">The id-code.</param>
        [Transaction]
        public void DeleteAjaxGrid(long id)
        {
            var user = this.repositoryUser.Get(id);

            if (user == null)
            {
                throw new HttpException(404, string.Format("User with id [{0}] not found.", id));
            }

            if (user.Email.Equals(User.Identity.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new HttpException(403, string.Format("You can't remove current user"));
            }

            this.repositoryUser.Remove(user);
        }

        #endregion AJAX actions
    }
}