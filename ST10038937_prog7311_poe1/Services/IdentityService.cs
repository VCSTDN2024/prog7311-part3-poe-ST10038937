using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ST10038937_prog7311_poe1.Models;

namespace ST10038937_prog7311_poe1.Services
{
    public static class IdentityService
    {
        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<Data.ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();

            // Register custom application services
            services.AddScoped<IAuditService, AuditService>();

            return services;
        }
    }
}
