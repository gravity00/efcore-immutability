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

        builder.Entity<CarBrandEntity>(cfg =>
        {
            cfg.ToTable("CarBrands");

            cfg.HasKey(e => e.Id);
            cfg.HasAlternateKey(e => e.ExternalId);

            cfg.HasIndex(e => e.Name).IsUnique();

            cfg.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
            cfg.Property(e => e.ExternalId)
                .IsRequired();
            cfg.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(32);
            cfg.Property(e => e.Description)
                .HasMaxLength(256);
        });

        builder.Entity<CarModelEntity>(cfg =>
        {
            cfg.ToTable("CarModels");

            cfg.HasKey(e => e.Id);
            cfg.HasAlternateKey(e => e.ExternalId);

            cfg.HasIndex(e => e.Name).IsUnique();

            cfg.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
            cfg.Property(e => e.ExternalId)
                .IsRequired();
            cfg.HasOne<CarBrandEntity>()
                .WithMany()
                .HasForeignKey(e => e.BrandId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
            cfg.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(32);
            cfg.Property(e => e.Description)
                .HasMaxLength(256);
        });

        builder.Entity<CarEntity>(cfg =>
        {
            cfg.ToTable("Cars");

            cfg.HasKey(e => e.Id);
            cfg.HasAlternateKey(e => e.ExternalId);

            cfg.HasIndex(e => e.Plate).IsUnique();

            cfg.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
            cfg.Property(e => e.ExternalId)
                .IsRequired();
            cfg.HasOne<CarModelEntity>()
                .WithMany()
                .HasForeignKey(e => e.ModelId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}