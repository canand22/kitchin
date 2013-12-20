using FluentValidation.Attributes;
using SmartArch.Web.Membership;

namespace KitchIn.WCF.Core.Models
{
    /// <summary>
    /// The registration model
    /// </summary>
    [Validator(typeof(RegisterUserRequest))]
    public class RegisterUserRequest : IMembershipUser
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets Login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets ConfirmPassword.
        /// </summary>
        public string ConfirmPassword { get; set; }
    }
}
