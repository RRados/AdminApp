using AdminApp.Migrations;
using AdminApp.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace AdminApp.Repository
{

    public class AdminRepository
    {
        public AdminRepository()   {    }



        #region metode   LOGIN      RegisterUser - ValidUser - GetLogIns - Login

        public static void RegisterUser(User register)
        {
            var context = new AdministratorContext();

            context.Add(register);

            context.SaveChanges();         
        }

        //public User ValidUser(User logInUser)
        //{
        //    using (var context = new AdministratorContext())
        //    {
        //        User user = context.Users.Where(p => p.Email.Equals(logInUser.Email) &&
        //                                        p.Password.Equals(logInUser.Password)).SingleOrDefault();

        //        if (user == null)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            return user;
        //        }
        //    }
        //}
    
        public static IEnumerable<LogIn> GetLogIns(int logIn)
        {
            var context = new AdministratorContext();
                               
            return context.LogIns.ToList();
        }

        // pisem u bazu trenutno vreme kad se korisnik ulogovao i njegov id
        public static void Login(int idlogIn)
        {
            var context = new AdministratorContext();

            LogIn log = new LogIn();
            log.IdUser = idlogIn;
            log.LoggedIn = DateTime.Now;           

            context.Add(log);
            context.SaveChanges();
        }

        public static void LogOutTime(int idLogOut)
        {
            var context = new AdministratorContext();
            LogIn newLog = new LogIn();
            newLog.IdUser = idLogOut;
            newLog.LoggedOut = DateTime.Now;

            context.Update(newLog);
            context.SaveChanges();
        }

        #endregion


        #region  metode   USER    Get - Delete - Edit - Add


        public static List<User> GetUsers()
        {
            var context = new AdministratorContext();

            return context.Users.ToList();
        }


        public static void Delete(User user)
        {
            var context = new AdministratorContext();

            context.Users.Remove(user);

            context.SaveChanges();
        }


        public static void Edit(User user)
        {
            UserRole userRole = new UserRole();

            var context = new AdministratorContext();
          
            userRole = (
                from r in context.Roles
                join ur in context.UserRoles on r.IdRole equals ur.IdRole
                join u in context.Users on ur.IdUser equals u.IdUser
                where ur.IdUser == user.IdUser
                select new UserRole
                {
                    IdRole = ur.IdRoleNavigation.IdRole,
                    IdUser = ur.IdUserNavigation.IdUser,
                    IdRoleNavigation = ur.IdRoleNavigation,
                    IdUserNavigation = ur.IdUserNavigation
                }
                ).FirstOrDefault();

            if (user != null && userRole != null)
            {
                //context.Users.Update(user);
                context.UserRoles.Update(userRole);
                
                context.SaveChanges();
            } 
        }


        public static void Add(User user)
        {
            var context = new AdministratorContext();

            context.Add(user);

            context.SaveChanges();
        }

        #endregion


        #region metode   ROLE       Get - Add - Edit - Delete


        public static IEnumerable<Role> GetRole()
        {
            var context = new AdministratorContext();

            return context.Roles.ToList();
        }


        public static void AddNewRole(Role role)
        {
            var context = new AdministratorContext();

            context.Add(role);

            context.SaveChanges();
        }


        public static void EditRole(Role role)
        {
            var context = new AdministratorContext();

            context.Roles.Update(role);

            context.SaveChanges();
        }


        public static void Delete(Role role)
        {
            var context = new AdministratorContext();

            context.Roles.Remove(role);

            context.SaveChanges();
        }


        #endregion


        #region metode   USER_ROLE      Get - GetId - AddUser - Edit


        public static List<UserRole> GetUserRoles()
        {
            var context = new AdministratorContext();

            return context.UserRoles.ToList();
        }

        public static UserRole GetUserRolesId(int id)
        {
            var context = new AdministratorContext();

            var idRole = context.UserRoles.Where(p => p.IdUserNavigation.IdUser == id).FirstOrDefault();

            return idRole;
        }


        public static void AddUserToRole(int idusera, int idrole)
        {
            var context = new AdministratorContext();

            UserRole novarola = new UserRole();
            Role rola = new Role();
            User user = new User();


            novarola.IdUser = idusera;
            novarola.IdRole = idrole;

            var staraRola = context.UserRoles.Where(p => p.IdRole == rola.IdRole && p.IdUser == user.IdUser).FirstOrDefault();

            if (staraRola != null)
            {
                context.Remove(staraRola);
                context.SaveChanges();
                //AddUserToRole(novarola.IdUser, novarola.IdRole);
            }
            context.UserRoles.Add(novarola);

            context.SaveChanges();
        }

        public static void EditUserInRole(int idusera, int idrole)
        {                       
            var context = new AdministratorContext();
            
            UserRole novarola = new UserRole();
            User user= new User();
            Role rola = new Role();


            rola.IdRole = idrole;
            user.IdUser = idusera;

            var staraRola = context.UserRoles.Where(p => p.IdRole == rola.IdRole && p.IdUser == user.IdUser).FirstOrDefault();

            if (staraRola != null)
            {
                context.Remove(staraRola);  // ako postoji korisnik u nekoj roli.. a mora inace pada app - brisem ga
                context.SaveChanges();
                AddUserToRole(novarola.IdUser, novarola.IdRole); // pa kreiram novog sa novog rolom.. 
            }

            //novarola.IdUser = user.IdUser;
            //novarola.IdRole = rola.IdRole;

            //context.UserRoles.Update(novarola);           

            //context.SaveChanges();

        }


        #endregion
    }
}
