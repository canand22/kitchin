using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using KitchIn.Core.Models;

namespace KitchIn.WCF.Core.Models
{
    [DataContract]
    [KnownType(typeof(ProductModel))]
    public class PreviewResponse
    {
        public PreviewResponse()
        {
            this.PreviewProducts = new Collection<KeyValuePair<string, IList<ProductModel>>>();
        }

        [DataMember]
        public IList<KeyValuePair<string, IList<ProductModel>>> PreviewProducts { get; set; }
    }
}
