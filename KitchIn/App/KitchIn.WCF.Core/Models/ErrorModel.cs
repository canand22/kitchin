namespace KitchIn.WCF.Core.Models
{
    /// <summary>
    /// The error model
    /// </summary>
    public class ErrorModel
    {
        /// <summary>
        /// Gets or sets ErrorName.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets ErrorDescription.
        /// </summary>
        public string ErrorDescription { get; set; }
    }
}