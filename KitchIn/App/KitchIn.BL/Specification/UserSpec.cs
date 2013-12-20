using System;
using SmartArch.Data.Specifications;
using KitchIn.Core.Entities;

namespace KitchIn.BL.Specification
{
    public static class UserSpec
    {
        public static ISpecification<User> ByLogin(Guid id)
        {
            return new Specification<User>(u => u.SessionId == id);
        }
    }
}
