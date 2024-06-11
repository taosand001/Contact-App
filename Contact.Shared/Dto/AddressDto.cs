using Contact.Shared.Attributes;

namespace Contact.Shared.Dto
{
    public class AddressDto
    {
        [FieldValidation("Street")]
        public string? Street { get; set; }
        [FieldValidation("City")]
        public string? City { get; set; }
        [FieldValidation("ApartmentNumber")]
        public string? ApartmentNumber { get; set; }
        [FieldValidation("PostalCode")]
        public string? PostalCode { get; set; }
        [FieldValidation("HouseNumber")]
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
