using System;
using System.Collections.Generic;
using KitchIn.Core.Entities;
using KitchIn.Core.Enums;
using SmartArch.Web.Membership;

namespace KitchIn.Core.Interfaces
{
    /// <summary>
    ///  Represents interface of membership provider
    /// </summary>
    public interface IMembershipProvider
    {
        /// <summary>
        /// Validate user account
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// True - user valid; False - user not valid 
        /// </returns>
        bool ValidateUser(IMembershipUser user);

        /// <summary>
        /// Validate user account
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// True - user valid; False - user not valid 
        /// </returns>
        bool ValidateUser(string login, string password);

        /// <summary>
        /// Create new user. With the list of roles
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="role">
        /// The role.
        /// </param>
        void CreateUser(IMembershipUser user, UserRoles role);

        /// <summary>
        /// Create new user. With the "user" role default 
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        void CreateUser(IMembershipUser user);

        /// <summary>
        /// Update current user
        /// </summary>
        /// <param name="user">
        /// The current-user.
        /// </param>
        /// <param name="role">
        /// The role.
        /// </param>
        void UpdateUser(IMembershipUser user);

        /// <summary>
        /// Validate for the same logins
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <returns>
        /// True - login exists; False - login not exists 
        /// </returns>
        bool ValidateLogin(string login);

        /// <summary>
        /// Get the user
        /// </summary>
        /// <param name="userId">
        /// The user Id.
        /// </param>
        /// <param name="newPassword">
        /// The new Password.
        /// </param>
        void ChangeUserPassword(long userId, string newPassword);

        /// <summary>
        /// Get the user data
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <returns>
        /// The user data
        /// </returns>
        User GetUser(string login);

        void LoginUser(string email, string password);

        void LogoutUser(User user);
    }
}