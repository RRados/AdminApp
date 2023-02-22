using AdminApp.Models;
using AdminApp.Repository;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace AdminApp.Controllers
{

    [AllowAnonymous]
    public class LogInController : Controller
    {        
        AdministratorContext context;

        public LogInController()
        {
            context = new AdministratorContext();
        }



        public IActionResult Index()
        {
            LogIn login = new LogIn();
            User user = new User();

            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            User log = new User();

            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string returnUrl)
        {
            User logInValid = ValidateLogin(email, password);

            if (logInValid == null)
            {                
                TempData["LoginFailed"] = $"{email} i {password} su netacni !";

                return Redirect("/login/login");
            }
            else 
            {                
                await SignInUser(email,password);

                // pisem vreme u tabelu LOGINS
                var pamtiUlogovanog = context.Users.Where(p => p.Email == email).Select(p => p.IdUser).First();
                AdminRepository.Login(pamtiUlogovanog);

                                           
                if (string.IsNullOrWhiteSpace(returnUrl) || !returnUrl.StartsWith("/"))                    
                    TempData["LoginSuccess"] = $"{email} dobrodosli";
                {
                    if (User.IsInRole("Admin"))
                    { 
                        returnUrl = "/Admin/Index";
                    }
                    else
                    {
                        returnUrl = "/Home/Index";
                    }
                }                

                return Redirect(returnUrl);
            }
         

            /* ako je cekirano REMEMBER ME

             await HttpContext.SignInUser(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = true
                });
             */
        }

        private User ValidateLogin(string email, string password)
        {
            using (var context = new AdministratorContext())
            {
                User user = context.Users.Where(p => p.Email.Equals(email) &&
                                                p.Password.Equals(password)).SingleOrDefault();

                if (user == null)
                {
                    return null;
                }
                else
                {
                    return user;
                }
            }
        }

        //Ovde se loginuje user
        public async Task SignInUser(string email, string pass)
        {

            //vidim da li postoji, ako postoji uzmem ga celog
            User user = ValidateLogin(email, pass);


            //sada mu nadjem rolu koju ima
            UserRole role = context.UserRoles.Include("IdRoleNavigation").
                Where(p => p.IdUserNavigation.IdUser == user.IdUser).FirstOrDefault();


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email ),
                new Claim(ClaimTypes.Role, role.IdRoleNavigation.RoleName),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        }

        // logOut..
        public  IActionResult SignOutUser(string email)
        {          
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var user = HttpContext.User.FindFirst(ClaimTypes.Email);
            int idUser = context.Users.Where(p => p.Email == user.Value).Select(a => a.IdUser).First();
            AdminRepository.LogOutTime(idUser);

            return RedirectToAction("Login"); // kad se izlogujes, vrati te na Login page..
        }




        [HttpGet]
        public IActionResult Register()
        {
            LogIn log = new LogIn();
            
            return View();   
        }




        [HttpPost]
        public IActionResult Register(User register)
        {
            User reg = new User();

            if(ModelState.IsValid)
            {               
                AdminRepository.RegisterUser(register);

                TempData["register"] = JsonConvert.SerializeObject(reg);

                return Redirect("/Login");
            }
            return View(register);        
        }
    }
}
      