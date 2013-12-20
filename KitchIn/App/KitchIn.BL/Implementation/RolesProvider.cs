using System.Linq;
using System.Web.Security;
using Microsoft.Practices.ServiceLocation;
using SmartArch.Data;
using KitchIn.BL.Specification;
using KitchIn.Core.Entities;

namespace KitchIn.BL.Implementation
{
    /// <summary>
    /// Represents service for management roles
    /// </summary>
    public class RolesProvider : RoleProvider
    {
        /// <summary>
        /// Definition of the user's membership roles
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="roleName">
        /// The role name.
        /// </param>
        /// <returns>
        /// True - user in role, false - user is't role
        /// </returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            return true; ////ServiceLocator.Current.GetInstance<IRepository<User>>().Find.All(UserSpec.ByLogin(username)).Any();
        }

        /// <summary>
        /// Gets the user all roles 
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <returns>
        /// The array user role
        /// </returns>
        public override string[] GetRolesForUser(string username)
        {
            User user = null; ////ServiceLocator.Current.GetInstance<IRepository<User>>().Find.All(UserSpec.ByLogin(username)).SingleOrDefault();

            //if (user != null)
            //{
            //    return user.Roles.Select(r => r.Name).ToArray();
            //}

            return new string[0];
        }

        /// <summary>
        /// Create new role
        /// </summary> 
        /// <param name="roleName">
        /// The role name.
        /// </param>
        public override void CreateRole(string roleName)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Delete role
        /// </summary>
        /// <param name="roleName">
        /// The role name.
        /// </param>
        /// <param name="throwOnPopulatedRole">
        /// The throw on populated role.
        /// </param>
        /// <returns>
        /// True - the role successfully removed; false - the role not successfully removed
        /// </returns>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Determining whether such a role
        /// </summary>
        /// <param name="roleName">
        /// The role name.
        /// </param>
        /// <returns>
        /// True - the role esists; false - the role not exists
        /// </returns>
        public override bool RoleExists(string roleName)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Add many role to many user
        /// </summary>
        /// <param name="usernames">
        /// The usernames.
        /// </param>
        /// <param name="roleNames">
        /// The role names.
        /// </param>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Remove multiple roles in multiple user
        /// </summary>
        /// <param name="usernames">
        /// The usernames.
        /// </param>
        /// <param name="roleNames">
        /// The role names.
        /// </param>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Search users by role 
        /// </summary>
        /// <param name="roleName">
        /// The role name.
        /// </param>
        /// <returns>
        /// The users array
        /// </returns>
        public override string[] GetUsersInRole(string roleName)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets all users role
        /// </summary>
        /// <returns>
        /// All users role
        /// </returns>
        public override string[] GetAllRoles()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Find users from role
        /// </summary>
        /// <param name="roleName">
        /// The role name.
        /// </param>
        /// <param name="usernameToMatch">
        /// The username to match.
        /// </param>
        /// <returns>
        /// The users array
        /// </returns>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets or sets ApplicationName.
        /// </summary>
        public override string ApplicationName
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }
    }
}