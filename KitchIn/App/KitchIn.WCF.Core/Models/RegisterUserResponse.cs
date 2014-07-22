using System.Collections.Generic;

namespace KitchIn.WCF.Core.Models
{
    /// <summary>
    /// The error register user response model
    /// </summary>
    public class RegisterUserResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether user successfully registered.
        /// </summary>
        public bool IsUserRegistered { get; set; }

        /// <summary>
        /// Gets or sets ValidationErrors.
        /// </summary>
        public List<ErrorModel> ValidationErrors { get; set; }
    }
}