// <copyright file="Program.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.Service.CloudBornWorker
{
    using System.Threading;
    using System.Threading.Tasks;
    using ServiceSample.CloudBornApplication.CloudBorn.Common.Logging;
    using Microsoft.ServiceFabric.Services.Runtime;
    using ServiceSample.Common.Logging;
    using ServiceSample.Services.Utilities.ServiceFabric;

    public static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static async Task Main()
        {
            Logger.Initialize(
                EtwProviderNames.CloudBornApplicationOperation,
                ServiceFabricUtilities.AppBuildVersion,
                "CloudBornWorker");

            using (var operation = Logger.StartOperation(OperationDataBuilder.Create(OperationCloudBornWorker.ProgramMain)))
            {
                await operation.RunAsync(async () =>
                {
                    // The ServiceManifest.XML file defines one or more service type names.
                    // Registering a service maps a service type name to a .NET type.
                    // When Service Fabric creates an instance of this service type,
                    // an instance of the class is created in this host process.
                    await ServiceRuntime.RegisterServiceAsync(
                        "Service.CloudBornWorkerType",
                        context => new CloudBornWorker(context));
                });
            }

            // Prevents this host process from terminating so services keep running.
            await Task.Delay(Timeout.Infinite);
        }
    }
}
