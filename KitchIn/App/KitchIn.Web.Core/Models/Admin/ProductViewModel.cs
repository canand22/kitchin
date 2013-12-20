namespace KitchIn.Web.Core.Models.Admin
{
    public class ProductViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string ExpirationDate { get; set; }

        public bool IsAddedByUser { get; set; }

        public string DeclineRow { get; set; }

        public string ApproveRow { get; set; }
    }
}