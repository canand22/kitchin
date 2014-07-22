using System.Collections.Generic;
using KitchIn.Core.Entities;

namespace KitchIn.Core.Services.CSVParser
{
    public interface ICSVParser
    {
        List<Product> Parse(ref bool status, ref string message);
    }
}