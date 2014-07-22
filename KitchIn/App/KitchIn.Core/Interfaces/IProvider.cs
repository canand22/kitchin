using System;
using KitchIn.Core.Entities;
using SmartArch.Data;

namespace KitchIn.Core.Interfaces
{
    public interface IProvider
    {
        IRepository<User> UserRepo { get; }

        User GetUser(Guid guid);
    }
}