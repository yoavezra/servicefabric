// <copyright file="MyServiceException.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.ErrorHandling
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    /// <summary>
    /// Exception class to be used by all downstream components to report failures in a common way (such that it can be logged in a simple way)
    /// </summary>
    [Serializable]
    public class MyServiceException : Exception
    {
        [NonSerialized]
        private readonly string invokingComponent;

        [NonSerialized]
        private readonly int statusCode;

        public MyServiceException(string invokingComponent, int statusCode, string message)
            : base(message)
        {
            this.invokingComponent = invokingComponent;
            this.statusCode = statusCode;
        }

        public MyServiceException(string invokingComponent, int statusCode, string message, Exception innerException)
            : base(message, innerException)
        {
            this.invokingComponent = invokingComponent;
            this.statusCode = statusCode;
        }

        public string InvokingComponent => this.invokingComponent;

        // Status code returned by the calling service
        public int StatusCode => this.statusCode;

        protected MyServiceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.statusCode = info.GetInt32("StatusCode");
        }

        private MyServiceException()
            : base()
        {
        }

        private MyServiceException(string message)
            : base(message)
        {
        }

        private MyServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("StatusCode", this.statusCode);
            base.GetObjectData(info, context);
        }
    }
}