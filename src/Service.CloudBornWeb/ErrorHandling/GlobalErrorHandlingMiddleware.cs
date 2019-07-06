// <copyright file="GlobalErrorHandlingMiddleware.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.Service.CloudBornWeb.ErrorHandling
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using ServiceSample.CloudBornApplication.CloudBorn.Common.Logging;
    using ServiceSample.Common.Logging;

    /// <summary>
    /// Used to handle errors that were NOT raised from ASP.NET Controller Actions (the latter are handled by ErrorHandlingFilter)
    /// </summary>
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await this.next(httpContext).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                Logger.LogFailure(
                    OperationData.CreateGeneric(ServiceComponent.CloudBornWebService.ToString(), "GlobalErrorHandling"),
                    $"Exception {ex.TargetSite.Name} that was not caught in MVC filters",
                    ErrorLevel.Critical,
                    ex);

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}
