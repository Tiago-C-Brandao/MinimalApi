using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Entities;

namespace MinimalApi.Infrastructure.Db
{
    public class MinimalApiContext : DbContext
    {
        private readonly IConfiguration _configurationAppSettings;
        public MinimalApiContext(IConfiguration configurationAppSettings)
        {
            _configurationAppSettings = configurationAppSettings;
        }
        public DbSet<Admin> Admins { get; set; } = default!;
        public DbSet<Vehicle> Vehicles { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                    {
                        Id = 1,
                        Email = "admin@teste.com",
                        Password = "123456",
                        Role = "Adm"
                    }
            );
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                var stringConnection = _configurationAppSettings.GetConnectionString("MySql")?.ToString();
                if (!string.IsNullOrEmpty(stringConnection)) 
                {           
                    optionsBuilder.UseMySql(stringConnection, ServerVersion.AutoDetect(stringConnection));
                }
            }
        }
    }
}
