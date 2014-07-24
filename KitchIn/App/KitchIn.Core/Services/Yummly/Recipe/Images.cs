using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KitchIn.Core.Services.Yummly.Recipe
{
    public class Images
    {
        public ImageUrlsBySize imageUrlsBySize { get; set; }
        public string hostedSmallUrl { get; set; }
        public string hostedMediumUrl { get; set; }
        public string hostedLargeUrl { get; set; }
    }
}
