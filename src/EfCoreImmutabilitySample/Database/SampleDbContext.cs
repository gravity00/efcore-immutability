using Microsoft.EntityFrameworkCore;

namespace EfCoreImmutabilitySample.Database;

public class SampleDbContext : DbContext
{
    public SampleDbContext(DbContextOptions<SampleDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<PersonEntity>(cfg =>
        {
            cfg.ToTable("Persons");

            cfg.HasKey(e => e.Id);
            cfg.HasAlternateKey(e => e.ExternalId);

            cfg.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
            cfg.Property(e => e.ExternalId)
                .IsRequired();
            cfg.Property(e => e.Forename)
                .IsRequired()
                .HasMaxLength(64);
            cfg.Property(e => e.Surname)
                .IsRequired()
                .HasMaxLength(64);
            cfg.Property(e => e.MiddleName)
                .HasMaxLength(128);
            cfg.Property(e => e.Birthdate);
        });
    }
}