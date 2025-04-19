using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PersonalBlog.Models
{
    public class PersonalBlogContext : DbContext
    {
        public PersonalBlogContext(DbContextOptions<PersonalBlogContext> options)
            : base(options)
        {
        }

		public DbSet<Project> Projects { get; set; }
		public DbSet<DevLogEntry> DevLogEntries { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<DevLogTag> DevLogTags { get; set; }
		public DbSet<ProjectTodo> ProjectTodos { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<DevLogTag>()
				.HasKey(x => new { x.DevLogEntryId, x.TagId });

			modelBuilder.Entity<DevLogTag>()
				.HasOne(x => x.DevLogEntry)
				.WithMany(x => x.DevLogTags)
				.HasForeignKey(x => x.DevLogEntryId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<DevLogTag>()
				.HasOne(x => x.Tag)
				.WithMany(x => x.DevLogTags)
				.HasForeignKey(x => x.TagId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Tag>()
				.HasIndex(t => t.Name)
			.IsUnique();

			modelBuilder.Entity<Project>()
				.HasIndex(p => p.Name)
				.IsUnique();
		}
	}

    public class PersonalBlogContextFactory : IDesignTimeDbContextFactory<PersonalBlogContext>
    {
        public PersonalBlogContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PersonalBlogContext>();

            // Set up your connection string (adjust to match your config)
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.development.json")
                .Build();

            var connectionString = configuration.GetConnectionString("PersonalBlogContextConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new PersonalBlogContext(optionsBuilder.Options);
        }
    }
}
