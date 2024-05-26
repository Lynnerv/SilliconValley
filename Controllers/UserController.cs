using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SilliconValley.Integration.reqres;
using SilliconValley.Integration.reqres.dto;

namespace SilliconValley.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly ReqresApiIntegration _integration;

        public UserController(ILogger<UserController> logger, ReqresApiIntegration integration)
        {
            _logger = logger;
            _integration = integration;
        }

        public async Task<IActionResult> Index()
        {
            List<User> user = await _integration.GetAll();
            List<User> filtro = user
            .OrderBy(users => users.id)            
            .ToList();
            return View(filtro);
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                ViewData["msj"]=await _integration.CreateUser(newUser);
                return View();
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
