// <copyright file="TestAppController.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.Service.CloudBornWeb.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ServiceSample.CloudBornApplication.Service.CloudBornWeb.Configuration;

    // Allowing only test apps to access
    [Authorize(Policy = AuthorizationPolicyConstants.OnlyTestApp)]
    public class TestAppController : Controller
    {
        public IActionResult Ping()
        {
            return this.Ok();
        }
    }
}
