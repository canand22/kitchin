using System;
using System.Linq;
using KitchIn.Core.Entities;
using KitchIn.Core.Interfaces;
using Microsoft.Practices.ServiceLocation;
using SmartArch.Data;

namespace KitchIn.BL.Implementation
{
    public class ManageFavoritesProvider : BaseProvider, IManageFavoritesProvider
    {
        public IRepository<FavoriteRecipe> FavoritesRepo
        {
            get { return ServiceLocator.Current.GetInstance<IRepository<FavoriteRecipe>>();  }
        }

        public bool SetFavorites(Guid id, string bigOvenRecipe, bool hasFavorites)
        {
            var user = this.GetUser(id);

            if (user == null)
            {
                return false;
            }
            
            var favorite = user.FavoriteRecipes.SingleOrDefault(x => x.RecipeBigOven == bigOvenRecipe);

            if (favorite == null)
            {
                var f = new FavoriteRecipe
                            {
                                Name = "TestRecipe",
                                RecipeBigOven = bigOvenRecipe
                            };
                f.Users.Add(user);
                this.FavoritesRepo.Save(f);
                user.FavoriteRecipes.Add(f);
            }
            else
            {
                if (!hasFavorites)
                {
                    favorite.Users.Remove(user);
                    this.FavoritesRepo.SaveChanges();
                    user.FavoriteRecipes.Remove(favorite);
                }
            }
            
            this.UserRepo.SaveChanges();

            return true;
        }
    }
}