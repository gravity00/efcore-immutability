using Microsoft.EntityFrameworkCore;

namespace EfCoreImmutabilitySample;

public class EfCoreImmutabilityDbContext : DbContext
{
    public EfCoreImmutabilityDbContext(DbContextOptions<EfCoreImmutabilityDbContext> options)
        : base(options)
    {

    }
}