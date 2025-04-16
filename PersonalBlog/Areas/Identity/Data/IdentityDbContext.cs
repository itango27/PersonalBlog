using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PersonalBlog.Areas.Identity.Data
{
    public class PersonalBlogIdentityContext : IdentityDbContext<PersonalBlogUser>, IDataProtectionKeyContext
    {

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        public PersonalBlogIdentityContext(DbContextOptions<PersonalBlogIdentityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }

    public class PersonalBlogIdentityContextFactory : IDesignTimeDbContextFactory<PersonalBlogIdentityContext>
    {
        public PersonalBlogIdentityContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PersonalBlogIdentityContext>();

            // Set up your connection string (adjust to match your config)
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("PersonalBlogIdentityContextConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new PersonalBlogIdentityContext(optionsBuilder.Options);
        }
    }
}
