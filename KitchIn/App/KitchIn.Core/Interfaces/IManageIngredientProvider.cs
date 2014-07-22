using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KitchIn.Core.Entities;

namespace KitchIn.Core.Interfaces
{
    public interface IManageIngredientProvider
    {
        IEnumerable<KeyValuePair<long, string>> GetAllIngredients();
    }
}
