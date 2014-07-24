using System.Collections.Generic;
using KitchIn.Core.Entities;
using KitchIn.Core.Services.Yummly.Response;

namespace KitchIn.Core.Services.Yummly
{
    public interface IYummly
    {
        void UpdateMetadata();
        IEnumerable<RecipeSearchRes> SearchRecipes(YummlyReqEntity entity);
        RecipeRes GetRecipe(string id);
    }
}