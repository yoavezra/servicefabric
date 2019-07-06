// <copyright file="Startup.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.Service.CloudBornWeb
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO.Compression;
    using System.Net;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.ResponseCompression;
    using ServiceSample.CloudBornApplication.CloudBorn.Common.Logging;
    using ServiceSample.Services.Resources.Configuration;
    using ServiceSample.Services.Utilities.Authentication;
    using ServiceSample.CloudBornApplication.Service.CloudBornWeb.Configuration;
    using ServiceSample.CloudBornApplication.Service.CloudBornWeb.ErrorHandling;
    using ServiceSample.CloudBornApplication.Service.CloudBornWeb.Filters;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using ServiceSample.Common.Logging;
    using Swashbuckle.AspNetCore.Swagger;

    public class Startup : Services.Utilities.StartupBase
    {
        private const string CorsPolicyName = "CorsPolicy";

        private readonly CloudBornWebConfiguration configuration;

        public Startup(IConfiguration configuration)
            : base(ServiceComponent.CloudBornWebService.ToString())
        {
            this.configuration = CreateConfiguration(configuration);
            Logger.SetDataCenter(this.configuration.ServiceEnvironmentSettings.DataCenterName);
        }

        public override void ConfigureServicesCore(IServiceCollection services)
        {
            // Connection Cryptography Compliance: All connections must be utilizing TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Cloud Born API", Version = "v1" });
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(new ErrorHandlingFilter());
                options.Filters.Add(new OperationLoggingFilter(ServiceComponent.CloudBornWebService));
                options.AllowValidatingTopLevelNodes = true;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                    TokenValidation.CreateAuthenticationOptionsForMultiTenant(options, this.configuration.TokenValidationSettings));

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicyConstants.OnlyTestApp, policy => policy.RequireClaim(
                    "appid",
                    this.configuration.AuthorizedResources.TestTrustedAppId));
            });

            // Add Gzip compression to our responses
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.AddCors(options =>
            {
                options.AddPolicy(
                    CorsPolicyName,
                    policy =>
                    {
                        // Allow origins to match a configured wildcarded domains
                        policy
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .AllowAnyMethod()
                            .WithHeaders("authorization", "accept", "content-type", "origin");
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Not Implemented Yet")]
        public override void ConfigureCore(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contacts API V1");
            });
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            app.UseAuthentication();

            app.UseCors(CorsPolicyName);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller}/{action=Index}/{id?}");
            });

            // Enable serving static HTML/JS files for our single page app.
            app.UseResponseCompression();
        }

        private static CloudBornWebConfiguration CreateConfiguration(IConfiguration configuration)
        {
            ServiceEnvironmentSettings serviceEnvironmentSettings = configuration.GetSection("ServiceEnvironment").Get<ServiceEnvironmentSettings>();
            var environmentSettings = EnvironmentSettingsLoader.Load(serviceEnvironmentSettings.EnvironmentSettingsResourceName);

            TokenValidationSettings tokenValidationSettings = configuration.GetSection("TokenValidation").Get<TokenValidationSettings>();

            var authorizedResources = configuration.GetSection("AuthorizedResources").Get<AuthorizedResources>();

            return new CloudBornWebConfiguration(serviceEnvironmentSettings, environmentSettings, tokenValidationSettings, authorizedResources);
        }
    }
}
