using System;
using System.Globalization;
using FluentValidation;
using FluentValidation.Results;
using KitchIn.Web.Core.Models.Admin;

namespace KitchIn.Web.Core.Validation
{
    public class StoreViewModelValidator : AbstractValidator<StoreViewModel>
    {
         public StoreViewModelValidator()
         {
             this.RuleFor(x => x.Name)
                 .NotEmpty();

             this.RuleFor(x => x.Latitude)
                 .Must(x => CheckCoord(x.Value, -90.00, 90.00))
                 .WithMessage("fgsdhfgshfgh");

             this.RuleFor(x => x.Longitude)
               .Must(x => CheckCoord(x.Value, -180.00, 180.00))
                 .WithMessage("fgsdhfgshfgh");
        }


        private static bool CheckCoord(double? coord, double from, double to)
         {
             return coord.HasValue && (coord.Value <= to && coord.Value >= from);
         }

    }
}