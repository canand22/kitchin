using System.Collections.Generic;
using KitchIn.Core.Models;

namespace KitchIn.Core.Interfaces
{
    public interface IManageMatchingTexts
    {
        IList<ResultMatching> GetResultsOfTheMatching(string[] textForRecognizer, long storeId);
    }
}
