using System;
using System.Linq;
using KitchIn.Core.Entities;
using KitchIn.Core.Enums;
using KitchIn.Core.Interfaces;
using KitchIn.Core.Services.Mailing;

namespace KitchIn.BL.Implementation
{
    public class ManageUserProvider : BaseProvider, IManageUserProvider 
    {
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
                               Password = password,
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
            
            return this.ChangePassword(user);
        }

        public bool ChangeUserPassword(Guid id, string oldPassword, string newPassword)
        {
            var user = this.UserRepo.SingleOrDefault(x => x.SessionId == id && x.Password == oldPassword);

            return this.ChangePassword(user, newPassword);
        }

        public User GetUser(string email, string password)
        {
            return this.UserRepo.SingleOrDefault(x => x.Email == email && x.Password == password);
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

        private bool ChangePassword(User user, string password = null)
        {
            if (user == null)
            {
                return false;
            }

            user.Password = password ?? "new password";
            this.UserRepo.SaveChanges();
            new MailService().Send(user.Email, user.Password);

            return true;
        }
    }
}