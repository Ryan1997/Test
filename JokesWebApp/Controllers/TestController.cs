using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcClient.Extensions;

namespace JokesWebApp.Controllers
{
    public class TestController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> IndexAsync() => View("login", await HttpContext.GetExternalProvidersAsync());

        [HttpPost]
        [HttpGet]
        [Authorize]
        public IActionResult signin()
        {
            return Redirect("~/Home");
        }
    }
}
