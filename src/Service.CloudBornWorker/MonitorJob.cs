// <copyright file="MonitorJob.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.Service.CloudBornWorker
{
    using System.Threading;
    using System.Threading.Tasks;
    using ServiceSample.CloudBornApplication.CloudBorn.Common.Logging;
    using ServiceSample.Common.Logging;

    public class MonitorJob
    {
        public static async Task Run(CloudBornWorkerConfiguration configuration, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using (var operation = Logger.StartOperation(OperationDataBuilder.Create(OperationCloudBornWorker.MonitorJob)))
                {
                    await operation.RunAsync(async () =>
                    {
                        // Replace Task.Yield() with your code...
                        await Task.Yield();
                    });
                }

                await Task.Delay(configuration.MonitoringJobInterval, cancellationToken);
            }
        }
    }
}
