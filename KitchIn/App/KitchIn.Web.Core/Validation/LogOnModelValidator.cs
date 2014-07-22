using FluentValidation;

using Microsoft.Practices.ServiceLocation;

using KitchIn.Core.Interfaces;
using KitchIn.Resources;
using KitchIn.Web.Core.Models.Account;

namespace KitchIn.Web.Core.Validation
{
    /// <summary>
    /// Validation logOn model by fluent validation
    /// </summary>
    public class LogOnModelValidator : AbstractValidator<LogOnModel>
    {
        private IMembershipProvider MembershipProvider
        {
            get { return ServiceLocator.Current.GetInstance<IMembershipProvider>(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogOnModelValidator"/> class.
        /// </summary>
        public LogOnModelValidator()
        {
            //this.RuleFor(x => x.Login)
            //    .NotEmpty().WithLocalizedMessage(() => WEB.Models_Validation_Account_LoginNotEmpty)
            //    .Length(2, 25).WithLocalizedMessage(() => WEB.Models_Validation_Account_LoginNameSize)
            //    .Must((x, login) => this.MembershipProvider.ValidateUser(x)).WithLocalizedMessage(() => WEB.ModelState_InvaliLoginPassword);

            //this.RuleFor(x => x.Password)
            //    .NotEmpty().WithLocalizedName(() => WEB.Models_Validation_Account_PasswordNotEmpty)
            //    .Length(6, 25).WithLocalizedMessage(() => WEB.Models_Validation_Account_PasswordSize);
        }
    }
}