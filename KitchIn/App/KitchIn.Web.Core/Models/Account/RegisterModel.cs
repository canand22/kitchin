using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

using KitchIn.Web.Core.Models.Admin;
using KitchIn.Web.Core.Validation;
using SmartArch.Web.Membership;

namespace KitchIn.Web.Core.Models.Account
{
    /// <summary>
    /// The registration model
    /// </summary>
    [Validator(typeof(RegisterModelValidator))]
    public class RegisterModel : IMembershipUser
    {
        /// <summary>
        /// Gets or sets Login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets the user Id-code.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets ConfirmPassword.
        /// </summary>
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets Roles.
        /// </summary>
        public IList<EditRoleModel> ListRole { get; set; }
    }
}