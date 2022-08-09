using Microsoft.EntityFrameworkCore;

namespace EfCoreImmutabilitySample;

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

            cfg.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
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