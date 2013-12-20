using System;
using System.Linq;
using KitchIn.Core.Entities;
using KitchIn.Core.Interfaces;
using Microsoft.Practices.ServiceLocation;
using SmartArch.Data;

namespace KitchIn.BL.Implementation
{
    public class BaseProvider : IProvider
    {
        public IRepository<User> UserRepo
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IRepository<User>>();
            }
        }

        public User GetUser(Guid guid)
        {
            return this.UserRepo.SingleOrDefault(x => x.SessionId == guid);
        }
    }
}