using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Contact.Shared.Model
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? ApartmentNumber { get; set; }
        public string? PostalCode { get; set; }
        public string? HouseNumber { get; set; }
        [JsonIgnore]
        public int PersonalInformationId { get; set; }
        [JsonIgnore]
        public PersonalInformation? PersonalInformation { get; set; }
    }
}
