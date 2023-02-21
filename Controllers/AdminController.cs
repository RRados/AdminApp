using AdminApp.Models;
using AdminApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace AdminApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AdministratorContext context;
        private readonly ILogger<HomeController> _logger;

        public AdminController(AdministratorContext context, ILogger<HomeController> logger)
        {
            this.context = context;
            _logger = logger;
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


        #region USER 

        // [Authorize(Roles = "Admin, User")]
        public IActionResult Index(int? page)
        {        
            var user = AdminRepository.GetUsers();
            return View(user.ToList());
        }

        [HttpGet]
        public IActionResult CreateNew()
        {
            User user = new User();

            var SveRole = context.Roles.ToList();

            ViewBag.SveRole = SveRole;

            return View();
        }


        [HttpPost]
        public IActionResult CreateNew(User user, int mojeRole)
        {
            if (ModelState.IsValid)
            {
                Role role = new Role();
                UserRole userRole = new UserRole();

                var rolla = context.UserRoles.Where(m => m.IdUser == user.IdUser).Where(m => m.IdRole == role.IdRole).FirstOrDefault();

                AdminRepository.Add(user);

                var userid = context.Users.Where(m => m.UserName == user.UserName && m.FirstName == user.FirstName && m.LastName == user.LastName && m.DateOfBirth == user.DateOfBirth).FirstOrDefault();
                AdminRepository.AddUserToRole(userid.IdUser, mojeRole);

                TempData["newUser"] = JsonConvert.SerializeObject(userRole);

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
      

        [HttpGet]
        public IActionResult Search(string searchName)
        {
            if (ModelState.IsValid)
            {
                IQueryable<User> searchData = from u in context.Users
                                              where u.UserName == searchName || u.LastName == searchName
                                              select u;

                if (string.Equals(searchData.ToString(),searchName))
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


        [HttpPost]
        public IActionResult Search(int id)
        {
            TempData["search"] = JsonConvert.SerializeObject(id);

            return View("Search");
        }



        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var SveRole = context.Roles.ToList();

            ViewBag.SveRole = SveRole;

            TempData["NijeURoli"] = JsonConvert.SerializeObject(SveRole);


            if (ModelState.IsValid)
            {
                User user = context.Users.Where(p => p.IdUser == id).FirstOrDefault();
                Role role = context.Roles.Where(p => p.IdRole == id).FirstOrDefault();           

                var u_Role = context.Roles.ToList();

                UserRole rolaLista = (from r in context.Roles
                            join ur in context.UserRoles on r.IdRole equals ur.IdRole
                            join u in context.Users on ur.IdUser equals u.IdUser
                            where u.IdUser == id
                            select new UserRole
                            {
                                IdRole = ur.IdRoleNavigation.IdRole,
                                IdUser = ur.IdUserNavigation.IdUser,
                                IdRoleNavigation = ur.IdRoleNavigation,
                                IdUserNavigation = ur.IdUserNavigation

                            }).FirstOrDefault();

                ViewBag.Rola = u_Role;
                ViewBag.rolaLista = rolaLista;
                    

                //AdminRepository.Edit(user);
                                    
                return View(rolaLista);
            }

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]        
        public IActionResult Edit(User user, int idRole,UserRole user_Role)
        {
            if (ModelState.IsValid)
            {
                User user1 = new User();
                user1 = user;
                Role role = new Role();
                UserRole userRole = new UserRole();
                var roll = context.UserRoles.Where(m=>m.IdUser == user.IdUser).Select(m=>m.IdRoleNavigation).FirstOrDefault();
                ViewBag.Roll = roll;
                //var rolla = context.UserRoles.Where(m => m.IdUser == user.IdUser).Where(m => m.IdRole == role.IdRole).FirstOrDefault();

                context.Update(user);
                context.SaveChanges();

                AdminRepository.Edit(user);

                var userid = context.Users.Where(m => m.UserName == user.UserName && m.FirstName == user.FirstName && m.LastName == user.LastName && m.DateOfBirth == user.DateOfBirth).FirstOrDefault();
                AdminRepository.EditUserInRole(userid.IdUser, idRole);

                TempData["editUserRole"] = JsonConvert.SerializeObject(roll);

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                User user = context.Users.Where(p => p.IdUser == id).FirstOrDefault();

                AdminRepository.Delete(user);

                TempData["delete"] = JsonConvert.SerializeObject(user);

                return View(user);
            }
            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(User user)
        {
            if (ModelState.IsValid)
            {
                AdminRepository.Delete(user);

                TempData["delete"] = JsonConvert.SerializeObject(user);
            }

            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            User user = context.Users.Where(p => p.IdUser == id).FirstOrDefault();

            Role role = context.Roles.Where(p => p.IdRole == user.IdUser).FirstOrDefault();

            UserRole userRole = context.UserRoles.Where(p => p.IdUserNavigation.IdUser == id).FirstOrDefault();

            UserRole all = (from r in context.Roles
                                 join ur in context.UserRoles on r.IdRole equals ur.IdRole
                                 join u in context.Users on ur.IdUser equals u.IdUser
                                 where u.IdUser == id
                                 select new UserRole
                                 {
                                     IdRole = ur.IdRoleNavigation.IdRole,
                                     IdUser = ur.IdUserNavigation.IdUser,
                                     IdRoleNavigation = ur.IdRoleNavigation,
                                     IdUserNavigation = ur.IdUserNavigation

                                 }).FirstOrDefault();
            ViewBag.Rola = all;    

            return View(userRole);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Details(UserRole userRole)
        {
            if (ModelState.IsValid)
            {
                UserRole _user = new();

                User user = context.Users.Where(p => p.IdUser == userRole.IdUser).FirstOrDefault();

                userRole = context.UserRoles.Where(p => p.IdUserNavigation.IdUser == user.IdUser).FirstOrDefault();

                Role role = context.Roles.Where(p => p.IdRole == userRole.IdUser).FirstOrDefault();

                IQueryable<UserRole> all = from r in context.Roles
                                           join ur in context.UserRoles on r.IdRole equals ur.IdRole
                                           join u in context.Users on ur.IdUser equals u.IdUser                                         
                                           select ur;

                ViewBag.Rola = context.Roles;

                user = context.Users.Where(p => p.IdUser == user.IdUser).FirstOrDefault();

                //ViewBag.All = AdminRepository.GetUserRolesId(user.IdUser).Where(p => p.IdRole == p.IdRole).First();

                return RedirectToAction("Details");
            }

            return View();
        }

#endregion

        #region Role

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditRole(int id)
        {
            if (ModelState.IsValid)
            {
                Role role = context.Roles.Where(p => p.IdRole == id).FirstOrDefault();

                return View(role);
            }
            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditRole(Role role)
        {
            if (ModelState.IsValid)
            {
                AdminRepository.EditRole(role);

                TempData["izmenaRole"] = JsonConvert.SerializeObject(role);

                return RedirectToAction("Role");
            }
            else
            {
                return View();
            }
        }



        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Role(Role role) { return View(); }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateRole()       {  return View();       }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateRole(Role role)
        {
            if (ModelState.IsValid)
            {
                AdminRepository.AddNewRole(role);

                UserRole ur = context.UserRoles.Where(p => p.IdRoleNavigation.RoleName == role.RoleName).FirstOrDefault();

                TempData["newRole"] = JsonConvert.SerializeObject(role);

                return RedirectToAction("Role");
            }
            
            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult DeleteRole(int id)
        {
            if (ModelState.IsValid)
            {
                Role brisiRolu = context.Roles.Where(p => p.IdRole == id).FirstOrDefault();

                ViewBag.BrisiRolu = brisiRolu;
               
                AdminRepository.Delete(brisiRolu);

                return View(brisiRolu);
            }
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteRole(Role role)
        {
            AdminRepository.Delete(role);

            TempData["deletedRole"] = JsonConvert.SerializeObject(role);
            
            return RedirectToAction("Role");         
        }

        #endregion

    }
}
