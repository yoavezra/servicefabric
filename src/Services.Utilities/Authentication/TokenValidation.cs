// <copyright file="TokenValidation.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Services.Utilities.Authentication
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;

    public static class TokenValidation
    {
        private const string IssuerClaimType = "iss";

        /// <summary>
        /// Used for applications that accept requests ONLY from MSIT tenant
        /// </summary>
        public static void CreateAuthenticationOptionsForSingleTenant(
            JwtBearerOptions options, TokenValidationSettings configuration, Guid tenantId)
        {
            options.Authority = configuration.AuthorizationServer;
            var validIssuer = $"{configuration.IssuerPrefix}/{tenantId}/";

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = validIssuer,
                ValidateAudience = true,
                ValidAudiences = configuration.ValidAudiences.Split(','),
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };
        }

        /// <summary>
        /// Used for applications that accept requests from users of multiple tenants
        /// </summary>
        public static void CreateAuthenticationOptionsForMultiTenant(
            JwtBearerOptions options, TokenValidationSettings configuration)
        {
            options.Authority = configuration.AuthorizationServer;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                // Disable issuer validation to allow the multi-tenant app to accept tokens from any tenant
                ValidateIssuer = false,
                ValidateAudience = true,
                ValidAudiences = configuration.ValidAudiences.Split(','),
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };

            options.Events = new JwtBearerEvents()
            {
                // After token was validated - add additional token validation check to verify the issuer of the multi tenant app token is as expected
                OnTokenValidated = context =>
                {
                    return ValidateTokenIssuer(context, configuration.IssuerPrefix);
                },
            };
        }

        private static Task ValidateTokenIssuer(TokenValidatedContext context, string issuerPrefix)
        {
            string issuer = context.Principal.FindFirst(IssuerClaimType).Value;
            if (issuer.StartsWith(issuerPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            return Unauthorized(context);
        }

        private static Task Unauthorized(TokenValidatedContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            // String might need localization
            context.Fail("Authentication failed: Token issuer validation error");

            return Task.CompletedTask;
        }
    }
}