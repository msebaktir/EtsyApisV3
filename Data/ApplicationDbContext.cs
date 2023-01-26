using dotnetEtsyApp.Models.RecordsData.Cradentials;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dotnetEtsyApp.Data{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // create a static method to create a new instance of the ApplicationDbContext
        public static ApplicationDbContext Create(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlite(connectionString);
            return new ApplicationDbContext(optionsBuilder.Options);
        }
        public DbSet<Store> Stores { get; set; }
        public DbSet<UserStoreAuthority> UserStoreAuthorities { get; set; }

    }
}