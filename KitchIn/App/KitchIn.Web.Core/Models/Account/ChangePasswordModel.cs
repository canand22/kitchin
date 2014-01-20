using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using KitchIn.Web.Core.Validation;

namespace KitchIn.Web.Core.Models.Account
{
    /// <summary>
    /// The change password model
    /// </summary>
    [Validator(typeof(ChangePasswordModelValidator))]
    public class ChangePasswordModel
    {
        /// <summary>
        /// Gets or sets OldPassword.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Old password")]
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets or sets NewPassword.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets ConfirmPassword.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        public string ConfirmPassword { get; set; }
    }
}
