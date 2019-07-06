// <copyright file="ServiceFabricStatelessEntryPoint.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Services.Utilities.ServiceFabric
{
    using System;
    using System.Threading;
    using Microsoft.ServiceFabric.Services.Runtime;

    /// <summary>
    /// Main entry point for a Service Fabric Stateless service
    /// </summary>
    public static class ServiceFabricStatelessEntryPoint
    {
        public static void Register(
            string serviceTypeName,
            Func<System.Fabric.StatelessServiceContext, StatelessService> serviceFactory)
        {
            // The ServiceManifest.XML file defines one or more service type names.
            // Registering a service maps a service type name to a .NET type.
            // When Service Fabric creates an instance of this service type,
            // an instance of the class is created in this host process.
            ServiceRuntime.RegisterServiceAsync(serviceTypeName, serviceFactory).GetAwaiter().GetResult();

            // Prevents this host process from terminating so services keeps running.
            Thread.Sleep(Timeout.Infinite);
        }
    }
}