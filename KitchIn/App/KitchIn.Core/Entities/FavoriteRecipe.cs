using Iesi.Collections.Generic;
using SmartArch.Core.Domain.Base;

namespace KitchIn.Core.Entities
{
    public class FavoriteRecipe : BaseEntity
    {
        public FavoriteRecipe()
        {
            this.Users = new HashedSet<User>();
        }
        
        public virtual string RecipeBigOven { get; set; }

        public virtual string Name { get; set; }

        public virtual ISet<User> Users { get; set; }
        
        public virtual void AddUsers(User user)
        {
            user.FavoriteRecipes.Add(this);
            this.Users.Add(user);
        }
    }
}