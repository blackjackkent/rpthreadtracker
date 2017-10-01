namespace RPThreadTrackerV3
{
	using System.Text;
	using System.Threading.Tasks;
	using AutoMapper;
	using Infrastructure.Entities;
	using Infrastructure.Identity;
	using Microsoft.AspNetCore.Authentication.Cookies;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using Microsoft.IdentityModel.Tokens;

	public class Startup
	{
		private IHostingEnvironment _env;
		private IConfigurationRoot _config { get; }

		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			_env = env;
			_config = builder.Build();
		}


		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Add framework services.
			services.AddSingleton(_config);
			var connection = _config["Data:ConnectionString"];
			services.AddDbContext<BudgetContext>(options => options.UseSqlServer(connection));
			//services.AddScoped<IRepository<Budget>, BudgetRepository>();
			//services.AddScoped<IBudgetService, BudgetService>();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<IPasswordHasher<IdentityUser>, MigrationPasswordHasher>();
			services.AddCors();
			services.AddMvc();
			services.AddAutoMapper();

			services.AddIdentity<IdentityUser, IdentityRole>(config =>
				{
					config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
					{
						OnRedirectToLogin = (ctx) =>
						{
							if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
							{
								ctx.Response.StatusCode = 401;
							}
							return Task.CompletedTask;
						},
						OnRedirectToAccessDenied = (ctx) =>
						{
							if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
							{
								ctx.Response.StatusCode = 403;
							}
							return Task.CompletedTask;
						}
					};
				})
				.AddEntityFrameworkStores<BudgetContext>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(_config.GetSection("Logging"));
			loggerFactory.AddDebug();

			app.UseIdentity();
			app.UseJwtBearerAuthentication(new JwtBearerOptions
			{
				AutomaticAuthenticate = true,
				AutomaticChallenge = true,
				TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = _config["Tokens:Issuer"],
					ValidAudience = _config["Tokens:Audience"],
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"])),
					ValidateLifetime = true
				}
			});
			app.UseCors(builder =>
				builder.WithOrigins("http://localhost:1989").AllowAnyHeader().AllowAnyMethod());
			app.UseMvc();
			//roleInitializer.Seed().Wait();
		}
	}
}
