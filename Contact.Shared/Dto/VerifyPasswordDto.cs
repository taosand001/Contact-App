using Contact.Shared.Attributes;

namespace Contact.Shared.Dto
{
    public class VerifyPasswordDto
    {
        [FieldValidation("Password")]
        public string newPassword { get; set; }
        public string Token { get; set; }
    }
}
