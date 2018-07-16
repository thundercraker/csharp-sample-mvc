using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EngineerTest.Models.Data;
using Hangfire.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace EngineerTest.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<CryptoTrade> CryptoTrades { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<CryptoTrade>()
                .HasKey(c => new { c.RunId, c.TradeNum });
        }
    }

    public class ApplicationDbContextFactory
    {
        private readonly IHostingEnvironment _environment;
        private readonly IConfiguration _configuration;

        public ApplicationDbContextFactory(
            IConfiguration configuration,
            IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        
        public virtual void ConfigureContextOptions(
            DbContextOptionsBuilder optionsBuilder)
        {
            if (_environment.IsDevelopment())
            {
                optionsBuilder
                    .UseSqlite(_configuration.GetConnectionString("DefaultConnection"));
            } else if (_environment.IsStaging())
            {
                optionsBuilder
                    .UseSqlServer(_configuration.GetConnectionString("DbConnectionStg"));
            } else if (_environment.IsProduction())
            {
                optionsBuilder
                    .UseSqlServer(_configuration.GetConnectionString("DbConnection"));
            }
        }

        public virtual ApplicationDbContext GetContext()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            ConfigureContextOptions(builder);
            return new ApplicationDbContext(builder.Options);
        }
    }
}
