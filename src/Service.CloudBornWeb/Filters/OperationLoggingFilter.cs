// <copyright file="OperationLoggingFilter.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.Service.CloudBornWeb.Filters
{
    using System;
    using System.Net;
    using Microsoft.AspNetCore.Mvc.Filters;
    using ServiceSample.CloudBornApplication.CloudBorn.Common.Logging;
    using ServiceSample.Services.Utilities.Logging;
    using ServiceSample.Common.ErrorHandling;
    using ServiceSample.Common.Logging;

    public class OperationLoggingFilter : IResourceFilter, IActionFilter
    {
        private readonly ServiceComponent serviceComponent;

        public OperationLoggingFilter(ServiceComponent serviceComponent)
        {
            this.serviceComponent = serviceComponent;
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            OperationLoggingFilterUtilities.AddOperation(context, this.serviceComponent.ToString());
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            OperationLoggingFilterUtilities.CloseOperation(context);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            OperationLoggingFilterUtilities.WrapOperationLogging(context, operation =>
            {
                var statusCode = GetStatusCode(context, operation);
                operation.StatusCode = statusCode;
            });
        }

        private static string GetStatusCode(ActionExecutedContext context, Operation operation)
        {
            string statusCode;
            if (context.Exception != null)
            {
                operation.LogFailure(
                    ErrorLevel.Error,
                    context.Exception,
                    context.Exception.TargetSite.Name);

                statusCode = GetStatusCodeFromError(context.Exception);
            }
            else
            {
                statusCode = GetStatusCodeAsString((HttpStatusCode)context.HttpContext.Response.StatusCode);
            }

            return statusCode;
        }

        private static string GetStatusCodeFromError(Exception exception)
        {
            if (exception is AggregateException aggregateEx)
            {
                exception = aggregateEx.Flatten().InnerException;
            }

            if (exception is MyServiceException serviceException)
            {
                int statusCode = serviceException.StatusCode;
                return ParseStatusCode(statusCode);
            }

            return HttpStatusCode.InternalServerError.ToString();
        }

        private static string ParseStatusCode(int statusCode)
        {
            if (Enum.IsDefined(typeof(HttpStatusCode), statusCode))
            {
                HttpStatusCode httpStatusCode = (HttpStatusCode)statusCode;
                return httpStatusCode.ToString();
            }

            return $"Code:{statusCode}";
        }

        private static string GetStatusCodeAsString(HttpStatusCode? statusCode)
        {
            if (statusCode.HasValue)
            {
                return statusCode.Value.ToString();
            }

            return string.Empty;
        }
    }
}
