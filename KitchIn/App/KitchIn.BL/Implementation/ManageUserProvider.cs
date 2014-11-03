using System;
using System.Linq;
using KitchIn.Core.Entities;
using KitchIn.Core.Enums;
using KitchIn.Core.Interfaces;
using KitchIn.Core.Services.Mailing;
using Newtonsoft.Json;

namespace KitchIn.BL.Implementation
{
    using System.Security.Cryptography;
    using System.Text;

    public class ManageUserProvider : BaseProvider, IManageUserProvider
    {
        public User GetUser(string email)
        {
            return this.UserRepo.SingleOrDefault(x => x.Email == email);
        }

        public void Save(User user)
        {
            this.UserRepo.Save(user);
        }

        public User CreateUser(string email, string password, string firstname, string lastname)
        {
            var user = this.UserRepo.SingleOrDefault(x => x.Email == email);

            if (user == null)
            {
                user = new User
                           {
                               Email = email,
                               Password = this.GetHashString(password),
                               FirstName = firstname,
                               LastName = lastname,
                               SessionId = Guid.NewGuid(),
                               Role = UserRoles.User
                           };

                this.UserRepo.Save(user);
            }
            else
            {
                user = null;
            }

            return user;
        }

        public bool ChangeUserPassword(string email)
        {
            var user = this.UserRepo.SingleOrDefault(x => x.Email == email);

            return this.ChangePassword(user, String.Empty);
        }

        public bool ChangeUserData(Guid id, string newEmail, string firstName, string lastName)
        {
            var user = this.UserRepo.FirstOrDefault(x => x.SessionId == id);

            if (user == null)
            {
                return false;
            }

            user.Email = newEmail;
            user.FirstName = firstName;
            user.LastName = lastName;

            this.UserRepo.SaveChanges();
            return true;
        }

        public bool ChangeUserPassword(Guid id, string oldPassword, string newPassword)
        {
            var oldPass = this.GetHashString(oldPassword);
            var user = this.UserRepo.SingleOrDefault(x => x.SessionId == id && x.Password == oldPass);

            return this.ChangePassword(user, newPassword);
        }

        public User GetUser(string email, string password)
        {
            var guidPassword = this.GetHashString(password);
            return this.UserRepo.SingleOrDefault(x => x.Email == email && x.Password.Equals(guidPassword));
        }

        public void LogOut(Guid id)
        {
            var user = this.UserRepo.SingleOrDefault(x => x.SessionId == id);

            if (user == null)
            {
                return;
            }

            user.SessionId = null;
            this.UserRepo.SaveChanges();
        }

        public void LogIn(User user)
        {
            if (user != null)
            {
                var session = Guid.NewGuid();
                user.SessionId = session;
                this.UserRepo.Save(user);
                this.UserRepo.SaveChanges();
            }
        }

        private bool ChangePassword(User user, string password = null)
        {
            if (user == null || password == null)
            {
                return false;
            }

            user.Password = this.GetHashString(password);
            this.UserRepo.SaveChanges();
            new MailService().Send(user.Email, password);
            return true;
        }

        public bool IsExistUser(Guid guid)
        {
            var result = this.GetUser(guid) != null;
            return result;
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

        /// <summary>
        /// Return new password
        /// </summary>
        /// <param name="email">email of user</param>
        /// <returns>result of operation</returns>
        public bool ForgotPassword(string email) 
        {
            if (!String.IsNullOrWhiteSpace(email)) 
            {
                var user = this.GetUser(email);
                var password = this.GeneratePassword();
                return this.ChangePassword(user, password);                
            }

            return false;
        }

        /// <summary>
        /// Generate new password
        /// </summary>
        /// <returns>password string</returns>
        public string GeneratePassword() 
        {
            var random = new Random();
            var str = String.Empty;

            for (int i = 0; i < 15; i++)
            {
                var smb = (char)random.Next(47, 123);
                str += smb > 57 && smb < 65 ? (char)64 : smb;
            }

            return str;
        }
    }
}