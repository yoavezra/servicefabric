// <copyright file="OperationDataBuilder.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.CloudBorn.Common.Logging
{
    using ServiceSample.Common.Logging;

    public class OperationDataBuilder
    {
        public static OperationData Create(OperationCloudBornWebService operation)
        {
            return new OperationData(operation.ToString(), ServiceComponent.CloudBornWebService.ToString());
        }

        public static OperationData Create(OperationCloudBornWorker operation)
        {
            return new OperationData(operation.ToString(), ServiceComponent.CloudBornWorker.ToString());
        }
    }
}
