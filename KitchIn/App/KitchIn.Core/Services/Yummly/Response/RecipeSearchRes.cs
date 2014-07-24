using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace KitchIn.Core.Services.Yummly.Response
{
    public class RecipeSearchRes
    {
        public JToken Id { set; get; }
        public string Title { set; get; }
        public string PhotoUrl { set; get; }
        public JToken TotalTime { set; get; }
        public double? Kalories { set; get; }
        public JToken[] Ingredients { set; get; }
    }
}
