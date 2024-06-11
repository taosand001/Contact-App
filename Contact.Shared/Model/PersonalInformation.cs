using Contact.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contact.Shared.Model
{
    [Index(nameof(Email), IsUnique = true)]
    public class PersonalInformation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Email { get; set; }
        [StringLength(50)]
        public string? FirstName { get; set; }
        [StringLength(50)]
        public string? LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public GenderType Gender { get; set; }
        [StringLength(15)]
        public string? PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageContentType { get; set; }
        [StringLength(11)]
        public string? PersonalCode { get; set; }
        public Address? Address { get; set; }
        public User? User { get; set; }
    }
}
