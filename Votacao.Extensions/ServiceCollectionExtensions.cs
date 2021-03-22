using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Votacao.Repository;
using Votacao.Repository.Models;
using Votacao.Repository.Models.Security;
using Votacao.Repository.Models.System;
using Votacao.Security;
using Votacao.Security.Models;

namespace Votacao.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IRepository<TEntity> GetRepository<TEntity>(this IServiceProvider provider) where TEntity : class, IEntity
        {
            return provider.GetRequiredService<IRepository<TEntity>>();
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Identity>, BaseRepository<Identity>>();
            services.AddScoped<IRepository<Role>, BaseRepository<Role>>();
            services.AddScoped<IRepository<Candidate>, BaseRepository<Candidate>>();
            services.AddScoped<IRepository<Vote>, BaseRepository<Vote>>();
            services.AddScoped<IRepository<Election>, BaseRepository<Election>>();
            return services;
        }

        public static IServiceCollection AddUserContext(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContextLoader, UserContextLoader>();
            services.AddScoped<IUserContext>(s =>
            {
                var userContext = new UserContext();
                s.GetRequiredService<IUserContextLoader>().Load(userContext);
                return userContext;
            });
            return services;
        }

        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<Identity, Role>()
                .AddEntityFrameworkStores<ModelContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IAuthenticationService, JwtIdentityAuthenticationService>();
            services.AddSingleton<Security.IAuthorizationService, AuthorizationService>();

            var tokenConfig = configuration.GetSection<TokenConfiguration>();
            services.AddSingleton(tokenConfig);

            var signingConfig = new SigningConfiguration(tokenConfig);
            services.AddSingleton(signingConfig);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters.IssuerSigningKey = signingConfig.Key;
                opt.TokenValidationParameters.ValidAudience = tokenConfig.ValidAudience;
                opt.TokenValidationParameters.ValidIssuer = tokenConfig.ValidIssuer;
                opt.TokenValidationParameters.ValidateIssuerSigningKey = tokenConfig.ValidateIssuerSigningKey;
                opt.TokenValidationParameters.ValidateLifetime = tokenConfig.ValidateLifetime;
                opt.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(TokenConfiguration.Policy, new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            return services;
        }
    }
}
