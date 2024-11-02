using Hospital.Domain.Entities;
using Hospital.Domain.Values;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Patient> Patients => base.Set<Patient>();
        public DbSet<Name> Names => base.Set<Name>();
        public DbSet<GivenName> GivenNames => base.Set<GivenName>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Hospital");

            modelBuilder.Entity<GivenName>()
               .HasKey(entity => entity.Id);


            modelBuilder.Entity<Name>()
                .HasKey(entity => entity.Id);

            modelBuilder.Entity<Name>()
                .Property(entity => entity.PatientId)
                .HasConversion(id => id.Value, value => new(value));


            modelBuilder.Entity<Patient>()
                .Property(entity => entity.Id)
                .HasConversion(id => id.Value, value => new(value));

            modelBuilder.Entity<Patient>()
                .HasKey(entity => entity.Id);

            modelBuilder.Entity<Patient>()
                .HasOne(entity => entity.Name)
                .WithOne()
                .HasForeignKey<Name>(name => name.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Patient>()
                .Property(entity => entity.Gender)
                .HasConversion<string>();
        }
    }
}
