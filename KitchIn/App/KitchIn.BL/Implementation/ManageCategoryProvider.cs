using System.Collections.Generic;
using System.Linq;
using KitchIn.Core.Entities;
using Microsoft.Practices.ServiceLocation;
using SmartArch.Data;
using KitchIn.Core.Interfaces;

namespace KitchIn.BL.Implementation
{
    public class ManageCategoryProvider : BaseProvider, IManageCategoryProvider
    {
        private readonly IRepository<Category> categoriesRepo;

        public ManageCategoryProvider()
        {
            this.categoriesRepo = ServiceLocator.Current.GetInstance<IRepository<Category>>();
        }

        public void Save(Category category)
        {
            this.categoriesRepo.Save(category);
        }

        public Category GetCategory(long id)
        {
            return this.categoriesRepo.SingleOrDefault(x => x.Id == id);
        }

        public Category GetCategory(string name)
        {
            return this.categoriesRepo.SingleOrDefault(x => x.Name == name);
        }

        public IEnumerable<KeyValuePair<long, string>> GetAllCategories()
        {
            return this.categoriesRepo.Select(x => new KeyValuePair<long, string>(x.Id, x.Name));
        }
    }
}
