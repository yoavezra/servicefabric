// <copyright file="ServiceFabricConfigurationProvider.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Services.Utilities.Configuration.ServiceFabric
{
    using System.Fabric;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Injects Service Fabric configurations into ASP.net IConfiguration object
    /// </summary>
    public class ServiceFabricConfigurationProvider : ConfigurationProvider
    {
        private readonly string packageName;
        private readonly CodePackageActivationContext context;

        public ServiceFabricConfigurationProvider(string packageName)
        {
            this.packageName = packageName;
            this.context = FabricRuntime.GetActivationContext();
            this.context.ConfigurationPackageModifiedEvent += (sender, e) =>
            {
                this.LoadPackage(e.NewPackage, reload: true);
                this.OnReload(); // Notify the change
            };
        }

        public override void Load()
        {
            var config = this.context.GetConfigurationPackageObject(this.packageName);
            this.LoadPackage(config);
        }

        private void LoadPackage(ConfigurationPackage config, bool reload = false)
        {
            if (reload)
            {
                this.Data.Clear();  // Rememove the old keys on re-load
            }

            foreach (var section in config.Settings.Sections)
            {
                foreach (var param in section.Parameters)
                {
                    this.Data[$"{section.Name}:{param.Name}"] = param.Value;
                }
            }
        }
    }
}