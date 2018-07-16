using System;
using EngineerTest.Data;
using EngineerTest.Filters.HangFire;
using EngineerTest.Jobs;
using EngineerTest.Models.Data;
using EngineerTest.Services;
using Hangfire;
using Hangfire.SQLite;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EngineerTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        private IConfiguration Configuration { get; }
        
        private IHostingEnvironment CurrentEnvironment { get; set; } 

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ApplicationDbContextFactory>(sd => 
                new ApplicationDbContextFactory(
                    Configuration, CurrentEnvironment));
            services.AddDbContext<ApplicationDbContext>(builder =>
            {
                var factory = new ApplicationDbContextFactory(
                    Configuration, CurrentEnvironment);

                factory.ConfigureContextOptions(builder);
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });
            
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            if (CurrentEnvironment.IsDevelopment())
            {
                services.AddTransient<IEmailService, DevOnlyEmailService>();
            }

            services.AddTransient<CryptowatchService>(sd => 
                new CryptowatchService(
                    sd.GetService<ApplicationDbContextFactory>(),
                    sd.GetService<ILogger<CryptowatchService>>()));
            
            services.AddMvc();

            services.AddHangfire(configuration =>
            {
                configuration.UseActivator(new ContainerJobActivator(services.BuildServiceProvider()));
                configuration.UseSQLiteStorage(Configuration.GetConnectionString("HangFireConnection"));
            });
        }
        
        public class ContainerJobActivator : JobActivator
        {
            private readonly IServiceProvider _provider;

            public ContainerJobActivator(IServiceProvider provider)
            {
                _provider = provider;
            }

            public override object ActivateJob(Type type)
            {
                return _provider.GetService(type);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            
            app.UseAuthentication();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new [] { new CustomDashboardAuthorizationFilter() }
            });
            app.UseHangfireServer();
            
            // Set up the api update job
            RecurringJob.AddOrUpdate<CryptowatchService>(
                "DataRefrash.Cryptowatch",
                _ => _.GetMarketTradeItemsAndSaveSummarySync(), 
                Cron.Minutely);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "About",
                    template: "About",
                    defaults: new { controller = "Home", action = "About" });
                routes.MapRoute(
                    name: "Contact",
                    template: "Contact",
                    defaults: new { controller = "Home", action = "Contact" });
            });
        }
    }
}
