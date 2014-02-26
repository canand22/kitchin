using KitchIn.BL.Attributes;

namespace KitchIn.BL.Entities
{
    public enum Errors
    {
        [Errors("Email '{0}' NOT valid")]
        EmailNotValid,

        [Errors("Request can not be empty")]
        EmptyRequest,

        [Errors("First Name can not be empty")]
        EmptyFirstName,

        [Errors("Last Name can not be empty")]
        EmptyLastName,

        [Errors("Password can not be empty")]
        EmptyPassword,

        [Errors("Error creating user")]
        ErrorCreatingUser,

        [Errors("Not filled in the required fields")]
        NotFilledRequiredFields,

        [Errors("User Not Exist")]
        UserNotExist
    }
}
