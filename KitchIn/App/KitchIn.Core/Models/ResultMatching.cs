using System.Collections.Generic;

namespace KitchIn.Core.Models
{
    public class ResultMatching //: IEqualityComparer<ResultMatching>
    {
        public string ItemName { get; set; }

        public string ItemShortName { get; set; }

        public long? Id { get; set; }

        public bool IsSuccessMatching { get; set; }

        public string Category { get; set; }

        //public bool Equals(ResultMatching x, ResultMatching y)
        //{
        //    return y.Id != null && (x.Id != null && x.Id.Value == y.Id.Value);
        //}

        //public int GetHashCode(ResultMatching obj)
        //{
        //    return (base.GetHashCode() * 397) ^ (Id != null ? Id.Value.GetHashCode() : 0);
        //}
    }
}
