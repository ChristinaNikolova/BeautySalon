﻿namespace BeautySalon.Web.Controllers
{
    using System.Diagnostics;

    using BeautySalon.Web.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        //check if works on HomeComtroller(Attr)
        [AllowAnonymous]
        public IActionResult Index()
        {
            return this.View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return this.View();
        }

        [Authorize]
        public IActionResult Chat()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
