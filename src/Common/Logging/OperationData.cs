// <copyright file="OperationData.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.Logging
{
    /// <summary>
    /// This struct contains data on the operation being executed.
    /// It also provides an easy way to construct an operation in the context of its service
    /// We create an enum for every service, which includes all the operations that can be logged in the context of the service
    /// We also add an override for the Create method that allows to create OperationData for the service.
    /// </summary>
    public struct OperationData
    {
        public string OperationName { get;  }

        public string Component { get; }

        public bool IsAudit { get; }

        public OperationData(string operationName, string component, bool isAudit = false)
        {
            this.OperationName = operationName;
            this.Component = component;
            this.IsAudit = isAudit;
        }

        public static OperationData CreateGeneric(string serviceComponent, string operationName, bool isAudit = false)
        {
            return new OperationData(operationName, serviceComponent, isAudit);
        }

        public override string ToString()
        {
            return $"{this.OperationName}_{this.Component}";
        }
    }
}