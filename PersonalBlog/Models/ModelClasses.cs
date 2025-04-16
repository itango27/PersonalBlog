using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.Models
{
    public class ModelClasses
    {
    }
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class ApplicationUser : IdentityUser
    {
        // Add custom properties if needed
    }
	public class Project
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; }

		[MaxLength(20)]
		public string Status { get; set; } // e.g., "Active", "On Hold", "Complete"

		public string Notes { get; set; }

		public ICollection<DevLogEntry> Entries { get; set; } = new List<DevLogEntry>();
		public ICollection<ProjectTodo> Todos { get; set; } = new List<ProjectTodo>();
	}
	public class DevLogEntry
	{
		public int Id { get; set; }

		[Required]
		public DateTime Date { get; set; } = DateTime.UtcNow;

		[Required]
		[MaxLength(100)]
		public string Title { get; set; }

		[MaxLength(300)]
		public string Summary { get; set; }

		public string Content { get; set; }

		public int? ProjectId { get; set; }
		public Project Project { get; set; }

		public ICollection<DevLogTag> DevLogTags { get; set; } = new List<DevLogTag>();
	}
	public class Tag
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		public ICollection<DevLogTag> DevLogTags { get; set; } = new List<DevLogTag>();
	}
	public class DevLogTag
	{
		public int DevLogEntryId { get; set; }
		public DevLogEntry DevLogEntry { get; set; }

		public int TagId { get; set; }
		public Tag Tag { get; set; }
	}
	public class ProjectTodo
	{
		public int Id { get; set; }

		public int ProjectId { get; set; }
		public Project Project { get; set; }

		[Required]
		[MaxLength(150)]
		public string Title { get; set; }

		public bool IsCompleted { get; set; } = false;
	}
}
