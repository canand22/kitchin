using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KitchIn.Core.Entities;
using KitchIn.Core.Interfaces;
using SmartArch.Data;

namespace KitchIn.BL.Implementation
{
    public class ManageIngredientProvider : IManageIngredientProvider
    {
        private readonly IRepository<Ingredient> ingredientRepo;

        public ManageIngredientProvider(IRepository<Ingredient> ingredientRepo)
        {
            this.ingredientRepo = ingredientRepo;

        }
        public IEnumerable<KeyValuePair<long, string>> GetAllIngredients()
        {
            return this.ingredientRepo.Select(x => new KeyValuePair<long, string>(x.Id, x.Term));
        }
    }
}
