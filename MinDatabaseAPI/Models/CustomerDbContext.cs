using Microsoft.EntityFrameworkCore;

namespace MinDatabaseAPI.Models
{
    public class CustomerDbContext : DbContext
    {
        private string _connectionString =
            "Server=(localdb)\\mssqllocaldb;Database=MinDatabase;Trusted_Connection=True;";
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options) 
        {
        }

        public DbSet<Administration> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
