using FluentValidation;
using KitchIn.Resources;
using KitchIn.Web.Core.Models.Account;

namespace KitchIn.Web.Core.Validation
{
    /// <summary>
    /// Validation change password model by fluent validation
    /// </summary>
    public class ChangePasswordModelValidator : AbstractValidator<ChangePasswordModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordModelValidator"/> class.
        /// </summary>
        public ChangePasswordModelValidator()
        {
            this.RuleFor(x => x.OldPassword)
                .NotEmpty().WithLocalizedMessage(() => WEB.Models_Validation_Account_PasswordNotEmpty)
                .Length(6, 25).WithLocalizedMessage(() => WEB.Models_Validation_Account_PasswordSize);

            this.RuleFor(x => x.NewPassword)
                .NotEmpty().WithLocalizedMessage(() => WEB.Models_Validation_Account_PasswordNotEmpty)
                .Length(6, 25).WithLocalizedMessage(() => WEB.Models_Validation_Account_PasswordSize);

            this.RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithLocalizedMessage(() => WEB.Models_Validation_Account_PasswordNotEmpty)
                .Length(6, 25).WithLocalizedMessage(() => WEB.Models_Validation_Account_PasswordSize)
                .Must((x, confirmPassword) => confirmPassword == x.NewPassword).WithLocalizedMessage(() => WEB.Model_Validation_Account_ConfirmPassword);
        }
    }
}