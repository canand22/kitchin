using System;
using SmartArch.Web.Membership;

namespace KitchIn.Web.Core.Models.Admin
{
    /// <summary>
    /// The user view model
    /// </summary>
    public class UserViewModel : IMembershipUser
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets EditRow.
        /// </summary>
        public string EditRow { get; set; }

        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets Login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the First Name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Last Name.
        /// </summary>
        public string LastName { get; set; }

        public Guid? SessionId { get; set; }

        public string Role { get; set; }
    }
}