using Microsoft.EntityFrameworkCore;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.Repositories
{
    public class GenshinDbContext : DbContext
    {
        public GenshinDbContext(DbContextOptions<GenshinDbContext> options)
            : base(options)
        {
        }

        public DbSet<GenshinEntity> GenshinCharacters { get; set; } = null!;
    }
}
