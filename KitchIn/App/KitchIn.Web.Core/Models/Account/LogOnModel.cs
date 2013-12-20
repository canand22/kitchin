using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using KitchIn.Web.Core.Validation;
using SmartArch.Web.Membership;

namespace KitchIn.Web.Core.Models.Account
{
    /// <summary>
    /// The logon model
    /// </summary>
    [Validator(typeof(LogOnModelValidator))]
    public class LogOnModel : IMembershipUser
    {
        /// <summary>
        /// Gets the user Id-code.
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
        /// Gets or sets Password.
        /// </summary>
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether RememberMe.
        /// </summary>
        public bool RememberMe { get; set; }
    }
}