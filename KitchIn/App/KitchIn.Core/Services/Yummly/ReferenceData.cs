using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KitchIn.Core.Entities;
using SmartArch.Data;

namespace KitchIn.Core.Services.Yummly
{
    public class ReferenceData
    {
        public long Id { set; get; }
        public string Category { set; get; }
        public string YummlyName { set; get; }
        public string FullName { set; get; }
        public string ShortName { set; get; }
    }
}
