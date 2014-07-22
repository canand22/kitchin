using System.Collections.Generic;
using KitchIn.Core.Entities;

namespace KitchIn.Core.Services.Yummly
{
    public interface IYummly
    {
        void UpdateMetadata();
        RecipeSearchJson SearchRecipes(string request);
        Recipe GetRecipe(string id);
    }
}