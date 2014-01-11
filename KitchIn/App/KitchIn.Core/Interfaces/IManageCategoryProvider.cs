using System.Collections.Generic;
using KitchIn.Core.Entities;
using KitchIn.Core.Models;

namespace KitchIn.Core.Interfaces
{
    public interface IManageCategoryProvider
    {
        void Save(Category category);

        Category GetCategory(long id);

        IEnumerable<KeyValuePair<long, string>> GetAllCategories();
    }
}