using Contact.Shared.Attributes;

namespace Contact.Shared.Dto
{
    public record UserDto
    {
        [FieldValidation("Username")]
        public string Username { get; init; }
        [FieldValidation("Password")]
        public string Password { get; init; }
        [FieldValidation("Email")]
        public string Email { get; init; }
    }
}
