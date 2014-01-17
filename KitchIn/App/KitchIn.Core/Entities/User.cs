using System;
using Iesi.Collections.Generic;
using KitchIn.Core.Enums;
using SmartArch.Core.Domain.Base;

namespace KitchIn.Core.Entities
{
    /// <summary>
    /// Describes User entity
    /// </summary>
    public class User : BaseEntity
    {
        ///// <summary>
        ///// Gets or sets the login.
        ///// </summary>
        ///// <value>
        ///// The login.
        ///// </value>
        //public virtual string Login { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public virtual string Password { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public virtual string Email { get; set; }

        /// <summary>
        /// Gets or sets the First Name.
        /// </summary>
        /// <value>
        /// The First Name.
        /// </value>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Last Name.
        /// </summary>
        /// <value>
        /// The Last Name.
        /// </value>
        public virtual string LastName { get; set; }


        /// <summary>
        /// Gets or sets the sessionId.
        /// </summary>
        /// <value>
        /// The session id.
        /// </value>
        public virtual Guid? SessionId { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public virtual UserRoles Role { get; set; }

        public virtual ISet<ProductsOnKitchen> Products { get; set; }

        public virtual ISet<FavoriteRecipe> FavoriteRecipes { get; set; }

        public User()
        {
            this.Products = new HashedSet<ProductsOnKitchen>();
            this.FavoriteRecipes = new HashedSet<FavoriteRecipe>();
        }

        public virtual void AddFavoriteRecipes(FavoriteRecipe recipe)
        {
            recipe.Users.Add(this);
            this.FavoriteRecipes.Add(recipe);
        }
    }
}
