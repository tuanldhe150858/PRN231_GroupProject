﻿using Microsoft.AspNetCore.Mvc;

namespace PRN_Final_Client.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
