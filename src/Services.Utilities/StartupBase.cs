// <copyright file="StartupBase.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Services.Utilities
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using ServiceSample.Common.Logging;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Base class for all Startup classes of all services
    /// </summary>
    public abstract class StartupBase
    {
        private readonly string component;

        protected StartupBase(string component)
        {
            this.component = component;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            using (var operation =
                Logger.StartOperation(OperationData.CreateGeneric(this.component, "ConfigureServices")))
            {
                operation.Run(() =>
                {
                    this.ConfigureServicesCore(services);
                });
            }
        }

        public abstract void ConfigureServicesCore(IServiceCollection services);

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var operation =
                Logger.StartOperation(OperationData.CreateGeneric(this.component, "Configure")))
            {
                operation.Run(() => { this.ConfigureCore(app, env); });
            }
        }

        public abstract void ConfigureCore(IApplicationBuilder app, IHostingEnvironment env);
    }
}