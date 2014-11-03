using System.Collections.Generic;
using KitchIn.Core.Entities;
using KitchIn.Core.Services.Yummly.Response;

namespace KitchIn.Core.Services.Yummly
{
    public interface IYummly
    {
        void UpdateMetadata();
        SearchResult Search(YummlyReqEntity entity);
        RecipeRes GetRecipe(string id);
        IList<ReferenceData> GetIngredientsRelations(string key);
        IList<ReferenceData> GetIngredientsRelationsByYammlyName(string key);
    }
}