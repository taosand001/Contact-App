using Contact.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace Contact.Domain.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<PersonalInformation> PersonalInformations { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<PersonalInformation>()
                .HasOne(e => e.Address)
                .WithOne(e => e.PersonalInformation)
                .HasForeignKey<Address>(e => e.PersonalInformationId)
                .IsRequired();
        }
    }
}
