using Google;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using PersonalBlog.Areas.Identity.Data;
using PersonalBlog.Models;
using PersonalBlog.Services;
using Serilog;

internal class Program
{
	private static void Main(string[] args)
	{
		Log.Logger = new LoggerConfiguration().CreateLogger();

		var builder = WebApplication.CreateBuilder(args);

		builder.Configuration
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
			.AddEnvironmentVariables();

		builder.Host.UseSerilog((context, loggerConfiguration) =>
		{
			loggerConfiguration.ReadFrom.Configuration(context.Configuration);
		});

		builder.WebHost.ConfigureKestrel(serverOptions =>
		{
			serverOptions.ListenAnyIP(5064); // HTTP
			serverOptions.ListenAnyIP(7115, listenOptions =>
			{
				listenOptions.UseHttps();     //Now HTTPS works on port 7115
			});
		});

		var config = builder.Configuration;

		builder.Services.AddDbContext<PersonalBlogIdentityContext>(options =>
			options.UseSqlServer(config.GetConnectionString("PersonalBlogIdentityContextConnection")));

		builder.Services.AddDbContext<PersonalBlogContext>(options =>
			options.UseSqlServer(config.GetConnectionString("PersonalBlogContextConnection")));

		builder.Services.AddDataProtection()
			.PersistKeysToDbContext<PersonalBlogIdentityContext>()
			.SetApplicationName("PersonalBlog")
			.ProtectKeysWithCertificate("86658ee0a625301aceaea8733d2986c11d823e89");

		builder.Services.AddDefaultIdentity<PersonalBlogUser>(options => options.SignIn.RequireConfirmedAccount = true)
			.AddEntityFrameworkStores<PersonalBlogIdentityContext>()
			.AddDefaultTokenProviders();

		builder.Services.AddAuthentication()
			.AddGoogle(options =>
			{
				var googleAuth = config.GetSection("Authentication:Google");
				options.ClientId = googleAuth["ClientId"];
				options.ClientSecret = googleAuth["ClientSecret"];
				options.CallbackPath = "/signin-google";
			});

		builder.Services.AddControllersWithViews();
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
		builder.Services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
		builder.Services.AddRazorPages();

		var app = builder.Build();

		if (app.Environment.IsDevelopment())
		{
			app.UseMigrationsEndPoint();
		}
		else
		{
			app.UseExceptionHandler("/Home/Error");
			app.UseHsts();
		}

		app.UseStaticFiles();
		app.UseRouting();
		app.UseAuthorization();

		app.MapControllerRoute(
			name: "default",
			pattern: "{controller=Home}/{action=Index}/{id?}");

		app.MapControllerRoute(
			name: "explorer",
			pattern: "{controller=Explorer}/{action=Index}/{id?}");

		app.MapRazorPages();
		app.MapControllers();

		app.Run();
	}
}
