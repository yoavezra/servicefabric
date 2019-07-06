// <copyright file="ServiceInstanceListenersBuilder.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Services.Utilities.ServiceFabric
{
    using System.Fabric;
    using System.Fabric.Description;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Hosting;
    using ServiceSample.Common.Logging;
    using ServiceSample.Services.Utilities.Configuration.ServiceFabric;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
    using Microsoft.ServiceFabric.Services.Communication.Runtime;
    using OperationData = ServiceSample.Common.Logging.OperationData;

    public static class ServiceInstanceListenersBuilder
    {
        public static ServiceInstanceListener CreateHttpListener<TStartup>(
            ICodePackageActivationContext activationContext,
            string component)
            where TStartup : class
        {
            using (var operation =
                Logger.StartOperation(OperationData.CreateGeneric(component, "CreateServiceInstanceListeners")))
            {
                return operation.Run(() =>
                {
                    var urls = activationContext.GetEndpoints()
                        .Where(e => e.Protocol == EndpointProtocol.Http || e.Protocol == EndpointProtocol.Https)
                        .Select(e => GetListenerUrl(e)).ToArray();

                    var endpointName = activationContext.GetEndpoints()
                        .Where(endpoint => endpoint.Protocol == EndpointProtocol.Http || endpoint.Protocol == EndpointProtocol.Https).Single().Name;

                    return new ServiceInstanceListener(serviceContext => new HttpSysCommunicationListener(
                        serviceContext,
                        endpointName,
                        (url, listener) =>
                        {
                            return new WebHostBuilder()
                                .UseHttpSys()
                                .ConfigureServices(
                                    services => services.AddSingleton(serviceContext))
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                .UseStartup<TStartup>()
                                .UseUrls(urls)
                                .ConfigureAppConfiguration(builder =>
                                {
                                    foreach (string configurationPackageName in serviceContext
                                        .CodePackageActivationContext
                                        .GetConfigurationPackageNames())
                                    {
                                        builder.AddServiceFabricConfiguration(configurationPackageName);
                                    }
                                })
                                .Build();
                        }));
                });
            }
        }

        /// <summary>
        /// Url as represeted by <see cref="HttpSysCommunicationListener"/>
        /// taken from:
        /// https://github.com/Azure/service-fabric-aspnetcore/blob/develop/src/Microsoft.ServiceFabric.AspNetCore.HttpSys/HttpSysCommunicationListener.cs
        /// </summary>
        /// <returns>url for the listener.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Url is commonly lower case")]
        private static string GetListenerUrl(EndpointResourceDescription endpointResourceDescription)
        {
            var listenUrl = string.Format(
                CultureInfo.InvariantCulture,
                "{0}://+:{1}",
                endpointResourceDescription.Protocol.ToString().ToLowerInvariant(),
                endpointResourceDescription.Port);

            return listenUrl;
        }
    }
}