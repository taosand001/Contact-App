using Contact.Shared.Attributes;
using Contact.Shared.Enum;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Contact.Shared.Dto
{
    public class UpdatePersonalInformationDto
    {
        [FieldValidation("Email")]
        [Required]
        public string? Email { get; set; }
        [FieldValidation("FirstName")]
        [Required]
        public string? FirstName { get; set; }
        [FieldValidation("LastName")]
        [Required]
        public string? LastName { get; set; }
        [FieldValidation("PhoneNumber")]
        [Required]
        public string? PhoneNumber { get; set; }
        [AllowedExtension([".png", ".jpg"])]
        [MaxFileSize(2 * 1024 * 1024)]
        [Required]
        public IFormFile? Image { get; set; }
        [FieldValidation("PersonalCode")]
        [Required]
        public AddressDto? Address { get; set; }

        public UpdatePersonalInformationDto() { }

        public UpdatePersonalInformationDto(string? email, string? firstName, string? lastName, DateTime dateOfBirth, GenderType gender, string? phoneNumber, IFormFile? image, string? personalCode, AddressDto? address)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Image = image;
            Address = address;
        }
    }
}
