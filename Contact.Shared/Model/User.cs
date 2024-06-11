using Contact.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Contact.Shared.Model
{
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(20)]
        public string Username { get; set; }
        public string Email { get; set; }
        [StringLength(15)]
        [JsonIgnore]
        public string? Token { get; set; }
        [JsonIgnore]
        public byte[] PasswordHash { get; set; }
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }
        [JsonIgnore]
        public RoleType Role { get; set; }
        [JsonIgnore]
        public DateTime LastPasswordChange { get; set; }
        [JsonIgnore]
        public List<PersonalInformation> PersonalInformation { get; set; } = new List<PersonalInformation>();
    }
}
