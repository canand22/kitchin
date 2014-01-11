using System;
using System.Data;

namespace KitchIn.Tools.ImportDataFromPotash.Entities
{
    public class NonFoodItem
    {
        public String UpcCode { get; private set; }
        public String PosDescription { get; private set; }
        public String LongDescription { get; private set; }

        public NonFoodItem(String upcCode, String posDescription, String longDescription)
        {
            this.UpcCode = upcCode;
            this.PosDescription = posDescription;
            this.LongDescription = longDescription;
        }

        public NonFoodItem(DataRow item)
        {
            this.UpcCode = item["UPC CODE"].ToString();
            this.PosDescription = item["POS DESCRIPTION"].ToString();
            this.LongDescription = item["LONG DESCRIPTION"].ToString();
        }
    }
}
