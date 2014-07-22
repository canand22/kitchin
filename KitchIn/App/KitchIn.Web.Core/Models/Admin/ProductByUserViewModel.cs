using KitchIn.Core.Entities;

namespace KitchIn.Web.Core.Models.Admin
{
    public class ProductByUserViewModel
    {
        public long Id { get; set; }
        
        public string PosDescription { get; set; }
        
        public string Name { get; set; }

        public string UpcCode { get; set; }

        public Ingredient Ingredient { get; set; }

        public string Category { get; set; }

        public string Store { get; set; }

        public string ExpirationDate { get; set; }

        public string Date { get; set; }

        public string UsersEmail { get; set; }

        public string DeclineRow { get; set; }

        public string ApproveRow { get; set; }

    }
}