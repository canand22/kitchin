using System;
using KitchIn.Core.Entities;

namespace KitchIn.Core.Interfaces
{
    public interface IManageUserProvider
    {
        User CreateUser(string email, string password, string firstname, string lastname);

        bool ChangeUserPassword(string email);

        bool ChangeUserPassword(Guid id, string oldPassword, string newPassword);

        User GetUser(string email, string password);

        User GetUser(string email);

        void Save(User user);

        void LogOut(Guid id);

        void LogIn(User user);

        bool IsExistUser(Guid guid);
    }
}