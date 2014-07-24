using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KitchIn.Core.Services.Yummly.Response
{
    public class RecipeRes
    {
        public string RecipeName { set; get; }
        public string Picture { set; get; }
        public int Rating { set; get; }
        public long Time { set; get; }
        public string Served { set; get; }
        public string[] Ingredients { set; get; }
        public string RecipeUrl { set; get; }

        public Dictionary<string, Tuple<string, double, string>> Nutritions { set; get; }
    }
}
