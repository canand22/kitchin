using FluentValidation;

using Microsoft.Practices.ServiceLocation;

using KitchIn.Core.Interfaces;
using KitchIn.Resources;
using KitchIn.Web.Core.Models.Account;

namespace KitchIn.Web.Core.Validation
{
    /// <summary>
    ///  Validation register model by fluent validation
    /// </summary>
    public class RegisterModelValidator : AbstractValidator<RegisterModel>
    {
        private IMembershipProvider MembershipProvider
        {
            get { return ServiceLocator.Current.GetInstance<IMembershipProvider>(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterModelValidator"/> class.
        /// </summary>
        public RegisterModelValidator()
        {
            this.RuleFor(x => x.Login)
                .NotEmpty().WithLocalizedMessage(() => WEB.Models_Validation_Account_LoginNotEmpty)
                .Length(2, 25).WithLocalizedMessage(() => WEB.Models_Validation_Account_LoginNameSize)
                .Must((x, login) => !this.MembershipProvider.ValidateLogin(login)).WithLocalizedMessage(() => WEB.ModelState_UserAlreadyExists);

            this.RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty().WithLocalizedMessage(() => WEB.Model_Validation_Account_EmailNotEmpty)
                .Length(0, 125).WithLocalizedMessage(() => WEB.Model_Validation_Account_EmailSize)
                .When(x => !string.IsNullOrEmpty(x.Email));

            this.RuleFor(x => x.Password)
                .NotEmpty().WithLocalizedMessage(() => WEB.Models_Validation_Account_PasswordNotEmpty)
                .Length(6, 25).WithLocalizedMessage(() => WEB.Models_Validation_Account_PasswordSize);

            this.RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithLocalizedMessage(() => WEB.Models_Validation_Account_PasswordNotEmpty)
                .Length(6, 25).WithLocalizedMessage(() => WEB.Models_Validation_Account_PasswordSize)
                .Must((x, confirmPassword) => confirmPassword == x.Password).WithLocalizedMessage(() => WEB.Model_Validation_Account_ConfirmPassword);
        }
    }
}