// <copyright file="Program.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.Service.CloudBornWeb
{
    using ServiceSample.CloudBornApplication.CloudBorn.Common.Logging;
    using ServiceSample.Common.Logging;
    using ServiceSample.Services.Utilities;
    using ServiceSample.Services.Utilities.ServiceFabric;

    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main(string[] args)
        {
            const string serviceTypeName = "Service.CloudBornWebType";

            Logger.Initialize(
                EtwProviderNames.CloudBornApplicationOperation,
                ServiceFabricUtilities.AppBuildVersion,
                "CloudBornWebService");

            using (var operation = Logger.StartOperation(OperationDataBuilder.Create(OperationCloudBornWebService.ProgramMain)))
            {
                operation.Run(() =>
                {
                    if (ServiceFabricUtilities.IsRunningUnderServiceFabric)
                    {
                        ServiceFabricStatelessEntryPoint.Register(
                            serviceTypeName,
                            context => new CloudBornWebService(context));
                    }
                    else
                    {
                        new WebAppEntryPoint<Startup>().Run(args);
                    }
                });
            }
        }
    }
}
