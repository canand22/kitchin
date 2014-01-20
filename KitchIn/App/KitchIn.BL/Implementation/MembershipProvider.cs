using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using AutoMapper;
using KitchIn.Core.Enums;
using SmartArch.Data;
using SmartArch.Data.Specifications;
using KitchIn.BL.Specification;
using KitchIn.Core.Entities;
using KitchIn.Core.Interfaces;
using SmartArch.Web.Membership;

namespace KitchIn.BL.Implementation
{
    /// <summary>
    /// Represents service for create and validation users
    /// </summary>
    public class MembershipProvider : IMembershipProvider
    {
        private const string DEFAULT_ROLE_NAME = "User";

        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IRepository<User> repositoryUser;

        static MembershipProvider()
        {
            Mapper.CreateMap<IMembershipUser, User>()
                .ForMember(u => u.Password, a => a.Ignore())
                .ForMember(u => u.Id, a => a.Ignore());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MembershipProvider"/> class.
        /// </summary>
        /// <param name="repositoryUser">The repository user.</param>
        /// <param name="repositoryRole">The repository Role.</param>
        public MembershipProvider(IRepository<User> repositoryUser)
        {
            this.repositoryUser = repositoryUser;
        }

        /// <summary>
        /// Validate user account
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>
        /// True - user valid; False - user not valid
        /// </returns>
        public bool ValidateUser(IMembershipUser user)
        {
            return this.ValidateUser(user.Login, user.Password);
        }

        /// <summary>
        /// Validate user account
        /// </summary>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// True - user valid; False - user not valid
        /// </returns>
        public bool ValidateUser(string email, string password)
        {
            //var guidPassword = this.GetHashString(password);
            return this.repositoryUser.Any(x => x.Email.Equals(email) && x.Password.Equals(password));
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="roles">
        /// The list Role.
        /// </param>
        public void CreateUser(IMembershipUser user, UserRoles roles)
        {
            var userEntity = new User
                {
                    ////Login = user.Login,
                    Email = user.Email,
                    Password = this.GetHashString(user.Password),
                    Role = roles
                };

            this.repositoryUser.Save(userEntity);
        }

        /// <summary>
        /// Creat new user. With the "user" role default 
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        public void CreateUser(IMembershipUser user)
        {
            this.CreateUser(user, UserRoles.User);
        }

        /// <summary>
        /// Update current user
        /// </summary>
        /// <param name="user">The current user.</param>
        /// <param name="roles">The roles.</param>
        public void UpdateUser(IMembershipUser user)
        {
            var userEntity = this.repositoryUser.Find.All(CommonSpec.ById<User>(user.Id)).FirstOrDefault();
            if (userEntity == null)
            {
                throw new ArgumentNullException("user", "User not found.");
            }

            Mapper.Map(user, userEntity);

            userEntity.Password = string.IsNullOrEmpty(user.Password) ? userEntity.Password : this.GetHashString(user.Password);

            //if (roles != null)
            //{
            //    userEntity.Roles.Clear();

            //    foreach (var activeRole in roles)
            //    {
            //        userEntity.Roles.Add(this.repositoryRole.First(x => x.Name.Equals(activeRole)));
            //    }
            //}

            this.repositoryUser.Save(userEntity);
        }

        /// <summary>
        /// Validate for the same logins
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <returns>
        /// True - login exists; False - login not exists 
        /// </returns>
        public bool ValidateLogin(string login)
        {
            return this.repositoryUser.Any(x => x.Email.Equals(login));
        }

        /// <summary>
        /// Get the user
        /// </summary>
        /// <param name="userId">
        /// The user Id.
        /// </param>
        /// <param name="newPassword">
        /// The new Password.
        /// </param>
        public void ChangeUserPassword(long userId, string newPassword)
        {
            var getUser = this.repositoryUser.First(x => x.Id.Equals(userId));
            getUser.Password = this.GetHashString(newPassword);
            this.repositoryUser.Save(getUser);
        }

        /// <summary>
        /// Get the user data
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <returns>
        /// The user data
        /// </returns>
        public User GetUser(string login)
        {
            var query = this.repositoryUser.First(x => x.Email.Equals(login));
            return query;
        }

        public void LoginUser(string email, string password)
        {
            var user = this.GetUser(email);
            if (user != null)
            {
                this.AddSession(user);
            }
        }

        public void LogoutUser(User user)
        {
            //var user = this.GetUser(email);
            if (user != null)
            {
                this.RemoveSession(user);
            }
        }

        private void AddSession(User user)
        {
            var session = Guid.NewGuid();
            user.SessionId = session;
            repositoryUser.Save(user);
            repositoryUser.SaveChanges();
        }

        private void RemoveSession(User user)
        {
            user.SessionId = null;
            repositoryUser.Save(user);
            repositoryUser.SaveChanges();
        }
        
        /// <summary>
        /// Convert string to hash-code
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The hash-code
        /// </returns>
        private string GetHashString(string password)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            var csp = new MD5CryptoServiceProvider();
            byte[] byteHash = csp.ComputeHash(bytes);

            return byteHash.Aggregate(string.Empty, (current, b) => current + string.Format("{0:x2}", b));
        }
    }
}