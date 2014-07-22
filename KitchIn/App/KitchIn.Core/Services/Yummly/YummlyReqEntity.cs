using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KitchIn.Core.Services.Yummly
{
    public class YummlyReqEntity
    {
        public string[] CookWith { set; get; }
        public string[] CookWithout { set; get; }
        public string[] Allergies { set; get; }
        public string[] Diets { set; get; }
        public string[] Cuisine { set; get; }
        public string[] DishType { set; get; }
        public string[] Holiday { set; get; }
        public string[] Meal { set; get; }
        public string Time { set; get; }
    }
}
