using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Ducode.Wolk.Application.Interfaces.Authentication;
using Ducode.Wolk.Application.Interfaces.Identity;
using Ducode.Wolk.Configuration;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.Identity.Impl;
using Ducode.Wolk.Identity.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Ducode.Wolk.Identity
{
    public static class IdentityModule
    {
        public static IServiceCollection AddIdentityModule(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddIdentityCore<User>()
                .AddRoles<ApplicationRole>()
                .AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>();
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services
                .AddOptions<IdentityConfiguration>()
                .Configure<IConfiguration>((opt, conf) => configuration.Bind(nameof(IdentityConfiguration), opt));

            services.AddTransient<IJwtManager, JwtManager>();
            services.AddTransient<IRegistrationManager, RegistrationManager>();
            services.AddTransient<ISignInManager, SignInManager>();
            services.AddTransient<IUserContext, UserContext>();
            services.AddTransient<IUserManager, UserManagerWrapper>();

            // Add authentication
            var identityConfig = configuration.GetSection(nameof(IdentityConfiguration)).Get<IdentityConfiguration>();
            var keyBytes = Encoding.ASCII.GetBytes(identityConfig.JwtSecret);
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }
    }
}
