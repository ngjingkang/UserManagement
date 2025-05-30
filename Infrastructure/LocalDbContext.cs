using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public partial class LocalDbContext(
            IConfiguration configuration,
            DbContextOptions<LocalDbContext> options) : DbContext(options)
    {
        public virtual DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
          => optionsBuilder.UseSqlServer(configuration.GetConnectionString("LocalDb"));


    }
}
