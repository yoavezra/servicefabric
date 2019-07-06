// <copyright file="DiagnosticsController.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.Service.CloudBornWeb.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Unauthenticated endpoint used by Traffic Manager to monitor the availability of the service
    /// </summary>
    [SuppressMessage("Microsoft.Analyzers.ManagedCodeAnalysis", "CA1822:MarkMembersAsStatic", Justification = "Controller class must be instance")]
    [Route("api/Diagnostics")]
    public class DiagnosticsController : Controller
    {
        [HttpGet]
        [Route("Health")]
        public IActionResult Health()
        {
            return this.Ok();
        }
    }
}
