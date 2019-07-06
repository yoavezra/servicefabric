// <copyright file="CloudBornWebService.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.Service.CloudBornWeb
{
    using System.Collections.Generic;
    using System.Fabric;
    using ServiceSample.Services.Utilities.ServiceFabric;
    using Microsoft.ServiceFabric.Services.Communication.Runtime;
    using Microsoft.ServiceFabric.Services.Runtime;
    using ServiceSample.CloudBornApplication.CloudBorn.Common.Logging;

    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance.
    /// </summary>
    internal sealed class CloudBornWebService : StatelessService
    {
        private readonly StatelessServiceContext serviceContext;

        public CloudBornWebService(StatelessServiceContext serviceContext)
            : base(serviceContext)
        {
            this.serviceContext = serviceContext;
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            ServiceInstanceListener serviceInstanceListener =
                ServiceInstanceListenersBuilder.CreateHttpListener<Startup>(
                this.serviceContext.CodePackageActivationContext, ServiceComponent.CloudBornWebService.ToString());

            return new[]
            {
                serviceInstanceListener,
            };
        }
    }
}
