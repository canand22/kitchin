using FluentValidation;
using KitchIn.Resources;
using KitchIn.Web.Core.Models.Admin;

namespace KitchIn.Web.Core.Validation
{
    /// <summary>
    /// Validation user edit model by fluent validation
    /// </summary>
    public class UserEditModelValidation : AbstractValidator<UserEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserEditModelValidation"/> class.
        /// </summary>
        public UserEditModelValidation()
        {
            this.RuleFor(x => x.Login).NotEmpty().WithLocalizedMessage(() => WEB.Models_Validation_Account_LoginNotEmpty).Length(2, 25).WithLocalizedMessage(() => WEB.Models_Validation_Account_LoginNameSize).When(x => !string.IsNullOrEmpty(x.Login), ApplyConditionTo.CurrentValidator);
            this.RuleFor(x => x.Email).EmailAddress().NotEmpty().WithLocalizedMessage(() => WEB.Model_Validation_Account_EmailNotEmpty).Length(0, 125).WithLocalizedMessage(() => WEB.Model_Validation_Account_EmailSize).When(x => !string.IsNullOrEmpty(x.Email), ApplyConditionTo.CurrentValidator);

            this.RuleFor(x => x.Password)
                .NotEmpty().WithLocalizedMessage(() => WEB.Models_Validation_Account_PasswordNotEmpty).Unless(
                    u => u.Id > 0, ApplyConditionTo.CurrentValidator)
                .Length(6, 25).WithLocalizedMessage(() => WEB.Models_Validation_Account_PasswordSize).Unless(u => string.IsNullOrEmpty(u.Password), ApplyConditionTo.CurrentValidator);
        }
    }
}