using System.Linq;

using KitchIn.Core.Interfaces;
using KitchIn.WCF.Contracts;
using KitchIn.WCF.Core.Models;
using KitchIn.WCF.Core.Validation;

namespace KitchIn.WCF.Services
{
    /// <summary>
    /// Implementation the IAccountService interface
    /// </summary>
    public class AccountService : IAccountService
    {
        /// <summary>
        /// The membership provider
        /// </summary>
        private readonly IMembershipProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        public AccountService(IMembershipProvider provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// Registration user in system
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// The RegisterUserResponse:
        ///		IsUserRegistered: true - user is registered; false - user doesn't registered in system;
        ///		ValidationErrors - the list of the form : name exception / exception description
        /// </returns>
        public RegisterUserResponse Register(RegisterUserRequest model)
        {
            var registerUserResponse = new RegisterUserResponse();
            var validationResult = RegisterUserRequestValidation.Instance.Validate(model);

            if (validationResult.IsValid)
            {
                registerUserResponse.IsUserRegistered = true;
                this.provider.CreateUser(model);
            }
            else
            {
                registerUserResponse.IsUserRegistered = false;
                registerUserResponse.ValidationErrors = validationResult.Errors
                    .Select(validationFailure => new ErrorModel { FieldName = validationFailure.PropertyName, ErrorDescription = validationFailure.ErrorMessage })
                    .ToList();
            }

            return registerUserResponse;
        }
    }
}
