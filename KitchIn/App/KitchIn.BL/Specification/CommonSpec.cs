using SmartArch.Core.Domain.Base;
using SmartArch.Data.Specifications;

namespace KitchIn.BL.Specification
{
    public static class CommonSpec
    {
        public static ISpecification<T> ById<T>(long id) where T : BaseEntity
        {
            return new Specification<T>(e => e.Id == id);
        }
    }
}
