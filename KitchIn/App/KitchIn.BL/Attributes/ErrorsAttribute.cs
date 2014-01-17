using System;

namespace KitchIn.BL.Attributes
{
    public class ErrorsAttribute : Attribute
    {
        public String Error { get; set; }

        public ErrorsAttribute(String error)
        {
            Error = error;
        }

    }
}
