using Contact.Shared.Attributes;
using Contact.Shared.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Shared.Dto
{
    public class PersonalInformationDto
    {
        [FieldValidation("Email")]
        public string? Email { get; set; }
        [FieldValidation("FirstName")]
        public string? FirstName { get; set; }
        [FieldValidation("LastName")]
        public string? LastName { get; set; }
        [FieldValidation("DateofBirth")]
        [BindProperty]
        public DateTime DateOfBirth { get; set; }
        public GenderType Gender { get; set; }
        [FieldValidation("PhoneNumber")]
        public string? PhoneNumber { get; set; }
        [AllowedExtension([".png", ".jpg"])]
        [MaxFileSize(2 * 1024 * 1024)]
        public IFormFile? Image { get; set; }
        [FieldValidation("PersonalCode")]
        public string? PersonalCode { get; set; }
        public AddressDto? Address { get; set; }

        public PersonalInformationDto() { }

        public PersonalInformationDto(string? email, string? firstName, string? lastName, DateTime dateOfBirth, GenderType gender, string? phoneNumber, IFormFile? image, string? personalCode, AddressDto? address)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            PhoneNumber = phoneNumber;
            Image = image;
            PersonalCode = personalCode;
            Address = address;
        }
    }


}
