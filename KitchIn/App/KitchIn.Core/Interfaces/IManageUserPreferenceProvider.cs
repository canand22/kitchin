using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KitchIn.Core.Entities;
using KitchIn.Core.Models;

namespace KitchIn.Core.Interfaces
{
    public interface IManageUserPreferenceProvider : IProvider
    {
        string AddOrUpdateUserPreference(Guid id, UserPreferenceiPhoneModel model);
        string RemoveUserPreference(Guid id, UserPreferenceiPhoneModel model);
    }
}
