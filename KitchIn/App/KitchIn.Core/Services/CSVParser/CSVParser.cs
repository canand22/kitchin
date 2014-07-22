using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KitchIn.Core.Entities;

namespace KitchIn.Core.Services.CSVParser
{
    public class CSVParser : ICSVParser
    {
        private readonly Stream fileStream;

        public CSVParser(Stream stream)
        {
            this.fileStream = stream;
        }

        public List<Product> Parse(ref bool status, ref string message)
        {
            var reader = new StreamReader(this.fileStream);
            var products = new List<Product>();
            var lines = reader.ReadToEnd().Split(new[] { "\r\n" }, StringSplitOptions.None).ToList();

            return products;
        }
    }
}