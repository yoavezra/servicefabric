// <copyright file="UserController.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.Service.CloudBornWeb.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    // Allowing only authenticated AAD users to access
    [Authorize]
    public class UserController : Controller
    {
        public IActionResult Ping()
        {
            return this.Ok();
        }
    }
}
