using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace KitchIn.Core.Services.Yummly.Response
{
    public class RecipeSearchRes
    {
        public string Id { set; get; }
        public string Title { set; get; }
        public string[] PhotoUrl { set; get; }
        public double TotalTime { set; get; }
        public double? Kalories { set; get; }
        public string[] Ingredients { set; get; }
        public double Rating { set; get; } 
    }
}
