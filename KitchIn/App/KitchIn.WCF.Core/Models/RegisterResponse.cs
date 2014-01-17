using System;

namespace KitchIn.WCF.Core.Models
{
    public class RegisterResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether user successfully registered.
        /// </summary>
        public bool IsUserRegistered { get; set; }

        public string Message { get; set; }

        public Guid? SessionId { get; set; }
    }
}