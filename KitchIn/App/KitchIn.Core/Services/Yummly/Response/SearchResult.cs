using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KitchIn.Core.Services.Yummly.Response
{
    public class SearchResult
    {
        public int TotalCount { set; get; }
        public List<RecipeSearchRes> Recipes { set; get; }

        public SearchResult()
        {
            Recipes = new List<RecipeSearchRes>();
        }
    }
}
