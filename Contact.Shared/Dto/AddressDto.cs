using Contact.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Contact.Shared.Dto
{
    public class AddressDto
    {
        [FieldValidation("Street")]
        [Required]
        public string? Street { get; set; }
        [FieldValidation("City")]
        [Required]
        public string? City { get; set; }
        [FieldValidation("ApartmentNumber")]
        public string? ApartmentNumber { get; set; }
        [FieldValidation("PostalCode")]
        [Required]
        public string? PostalCode { get; set; }
        [FieldValidation("HouseNumber")]
        [Required]
        public string? HouseNumber { get; set; }

        public AddressDto() { }

        public AddressDto(string? street, string? city, string? apartmentNumber, string? postalCode, string? houseNumber)
        {
            Street = street;
            City = city;
            ApartmentNumber = apartmentNumber;
            PostalCode = postalCode;
            HouseNumber = houseNumber;
        }
    }
}
