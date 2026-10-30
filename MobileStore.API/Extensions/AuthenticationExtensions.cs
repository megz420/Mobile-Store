using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace MobileStore.API.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring authentication services.
    /// </summary>
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Registers JWT bearer authentication and authorization services.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="configuration">The application configuration containing JWT settings.</param>
        /// <returns>The configured service collection.</returns>
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ============================================
            // Get JWT Settings from appsettings.json
            // ============================================
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? 
                throw new InvalidOperationException("JWT SecretKey is missing from configuration.");
            var issuer = jwtSettings["Issuer"] ?? 
                throw new InvalidOperationException("JWT Issuer is missing from configuration.");
            var audience = jwtSettings["Audience"] ?? 
                throw new InvalidOperationException("JWT Audience is missing from configuration.");
            var expiryMinutes = jwtSettings.GetValue<int>("ExpiryMinutes", 60);

            var key = Encoding.ASCII.GetBytes(secretKey);

            // ============================================
            // Register Authentication with JWT Bearer
            // ============================================
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // Set to true in production
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // ============================================
            // Register Authorization
            // ============================================
            services.AddAuthorization();

            return services;
        }
    }
}