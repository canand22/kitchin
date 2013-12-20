using System.Globalization;
using FluentValidation.Attributes;
using KitchIn.Web.Core.Validation;

namespace KitchIn.Web.Core.Models.Admin
{
    [Validator(typeof(StoreViewModelValidator))]
    public class StoreViewModel
    {
        public long? Id { get; set; }

        public string Name { get; set; }

        public double? Latitude
        {
            get { return ConvertToDouble(this.LatitudeS); } 
            set { this.LatitudeS = value.ToString(); }
        }

        public double? Longitude
        {
            get { return ConvertToDouble(this.LongitudeS); }
            set { this.LongitudeS = value.ToString(); }
        }

        public string LatitudeS { get; set; }

        public string LongitudeS { get; set; } 

        private static double? ConvertToDouble(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = CultureInfo.CurrentCulture.Name != "en-US"
                                     ? str.Replace('.', ',')
                                     : str.Replace(',', '.');

                double d;
                if (double.TryParse(str, out d))
                {
                    return d;
                }
            }

            return null;
        }
    }
}