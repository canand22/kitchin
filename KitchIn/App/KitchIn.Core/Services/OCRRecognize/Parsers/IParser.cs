using System.Collections.Generic;

namespace KitchIn.Core.Services
{
    public interface IParser
    {
        IList<ProductParserModel> GetProducts();

        int NumberOfItems { get; } 
    }
}