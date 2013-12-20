using System.Collections.Generic;
using FluentValidation.Attributes;
using KitchIn.Core.Enums;
using KitchIn.Web.Core.Validation;
using SmartArch.Web.Membership;

namespace KitchIn.Web.Core.Models.Admin
{
    /// <summary>
    /// The user edit model
    /// </summary>
    [Validator(typeof(UserEditModelValidation))]
    public class UserEditModel : IMembershipUser
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets Login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets Roles.
        /// </summary>
        public UserRoles Roles { get; set; }
    }
}