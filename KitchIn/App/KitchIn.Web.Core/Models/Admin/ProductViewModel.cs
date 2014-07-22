namespace KitchIn.Web.Core.Models.Admin
{
    using System;

    public class ProductViewModel
    {
        public long Id { get; set; }
        
        public string PosDescription { get; set; }
        
        public string Name { get; set; }

        public string IngredientName { get; set; }

        public string Category { get; set; }

        public string ExpirationDate { get; set; }

        public string TypeAdd { get; set; }

        public string ModificationDate { get; set; }

        public string DeclineRow { get; set; }

        public string ApproveRow { get; set; }

        public string Store { get; set; }
    }
}