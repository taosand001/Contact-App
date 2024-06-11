using Contact.Shared.Attributes;

namespace Contact.Shared.Dto
{
    public class CreateAddressDto
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
        public int PersonalInformationId { get; set; }

        public CreateAddressDto() { }

        public CreateAddressDto(string? street, string? city, string? apartmentNumber, string? postalCode, string? houseNumber, int personalInformationId)
        {
            Street = street;
            City = city;
            ApartmentNumber = apartmentNumber;
            PostalCode = postalCode;
            HouseNumber = houseNumber;
            PersonalInformationId = personalInformationId;
        }
    }
}
