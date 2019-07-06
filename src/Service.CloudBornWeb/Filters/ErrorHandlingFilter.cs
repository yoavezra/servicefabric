// <copyright file="ErrorHandlingFilter.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.Service.CloudBornWeb.Filters
{
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// Used to handle errors that were raised from ASP.NET Controller Actions
    /// </summary>
    public class ErrorHandlingFilter : IAsyncExceptionFilter
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            await HandleException(context.HttpContext).ConfigureAwait(false);
            context.ExceptionHandled = true;
        }

        private static Task HandleException(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(HttpError.GeneralServerError().ToJson());
        }
    }
}
