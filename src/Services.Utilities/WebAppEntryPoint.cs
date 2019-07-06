// <copyright file="WebAppEntryPoint.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Services.Utilities
{
    using System.IO;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using ServiceSample.Services.Utilities.Configuration.ServiceFabric;

    /// <summary>
    /// Main entry point for web app
    /// </summary>
    /// <typeparam name="TStartup">ASP.NET Startup class</typeparam>
    public class WebAppEntryPoint<TStartup>
        where TStartup : class
    {
        public void Run(string[] args)
        {
            BuildWebHost(args).Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    if (!context.HostingEnvironment.IsDevelopment())
                    {
                        return;
                    }

                    var settingFiles = Directory.GetFiles(
                        Path.Combine(context.HostingEnvironment.ContentRootPath, "PackageRoot"),
                        "Settings.Xml",
                        SearchOption.AllDirectories);
                    foreach (string settingFile in settingFiles)
                    {
                        builder.AddServiceFabricXmlConfiguration(settingFile);
                    }
                })
                .UseStartup<TStartup>()
                .Build();
    }
}