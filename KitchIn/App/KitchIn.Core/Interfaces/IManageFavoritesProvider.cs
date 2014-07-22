using System;
using KitchIn.Core.Entities;
using SmartArch.Data;

namespace KitchIn.Core.Interfaces
{
    public interface IManageFavoritesProvider : IProvider
    {
        IRepository<FavoriteRecipe> FavoritesRepo { get; }

        bool SetFavorites(Guid id, string bigOvenRecipe, bool hasFavorites);
    }
}