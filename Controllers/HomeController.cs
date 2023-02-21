using AdminApp.Models;
using AdminApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace AdminApp.Controllers
{
    
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            _logger.LogError("Error", HttpContext.User.ToString());

            return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Search(string searchName)
        {
            var context = new AdministratorContext();

            if (ModelState.IsValid)
            {
                IQueryable<User> searchData = from u in context.Users
                                              where u.UserName == searchName || u.LastName == searchName
                                              select u;

                if (string.Equals(searchData.ToString(), searchName))
                {
                    TempData["search"] = JsonConvert.SerializeObject(searchData);
                    //return Redirect("Index");
                }
                else
                {
                    searchData.Where(s => s.UserName.Contains(searchName) || s.LastName.Contains(searchName));

                    return View(searchData);
                }
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Search(int id)
        {
            TempData["search"] = JsonConvert.SerializeObject(id);

            return View("Search");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var execeptionHandlerPathFeture = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = execeptionHandlerPathFeture.Error.Message,
                    Source = execeptionHandlerPathFeture.Error.Source,
                    ErrorPath = execeptionHandlerPathFeture.Path,
                    StackTrace = execeptionHandlerPathFeture.Error.StackTrace,
                    InnerException = Convert.ToString(execeptionHandlerPathFeture.Error.InnerException)
                });
        }
    }
}
