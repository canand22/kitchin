using System;
using System.Data;

namespace KitchIn.Tools.ImportDataFromPotash.Entities
{
    public class GroceryItem
    {
        public String UpcCode { get; private set; }
        public String PosDescription { get; private set; }
        public String LongDescription { get; private set; }
        public String ApiMainMatch { get; private set; }
        public String Category { get; private set; }
        public String BrandName { get; private set; }
        public String CourseType { get; private set; }
        public String Allergydiet { get; private set; }

        public GroceryItem(String upcCode, String posDescription, String longDescription, String apiMainMatch, String category,
            String brandName, String courseType, String allergydiet)
        {
            this.UpcCode = upcCode;
            this.PosDescription = posDescription;
            this.LongDescription = longDescription;
            this.ApiMainMatch = apiMainMatch;
            this.Category = category;
            this.BrandName = brandName;
            this.CourseType = courseType;
            this.Allergydiet = allergydiet;
        }

        public GroceryItem(DataRow item)
        {
            this.UpcCode = item["UPC CODE"].ToString();
            this.PosDescription = item["POS DESCRIPTION (DO NOT EDIT)   *THIS PRINTS ON RECEIPT*"].ToString();
            this.LongDescription = item["LONG DESCRIPTION (NEEDS EDITING/SPELL CHECK)                      *THIS APPEARS ON KITCHIN APP AFTER RECOGNIZING*"].ToString();
            this.ApiMainMatch = item["API MAIN INGREDIENT MATCH (DATABASE - API)"].ToString();
            this.Category = item["FOOD CATEGORY                                 (Major food category icons in 'My KitchIn')"].ToString();
            this.BrandName = item["BRAND NAME"].ToString();
            this.CourseType = item["COURSE TYPE                        (If applicable) - What Meal of Day is it best for?"].ToString();
            this.Allergydiet = item["ALLERGY/DIET"].ToString();
        }
    }
}
