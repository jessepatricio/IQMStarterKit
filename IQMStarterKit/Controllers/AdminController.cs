using IQMStarterKit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using PagedList;

namespace IQMStarterKit.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        // GET: Admin

        public ActionResult Index(string searchKeyword, string currentFilter, int? page)
        {
            try
            {
                int intPage = 1;
                int intPageSize = 5;
                int intTotalPageCount = 0;

                if (searchKeyword != null)
                {
                    intPage = 1;
                }
                else
                {
                    if (currentFilter != null)
                    {
                        searchKeyword = currentFilter;
                        intPage = page ?? 1;
                    }
                    else
                    {
                        searchKeyword = "";
                        intPage = page ?? 1;
                    }
                }


                ViewBag.CurrentFilter = searchKeyword;
                var colUsers = new List<UserExtended>();
                int intSkip = (intPage - 1) * intPageSize;
                intTotalPageCount = UserManager.Users
                    .Count(u => u.UserName.Contains(searchKeyword));
                var result = UserManager.Users
                    .Where(c => c.UserName.Contains(searchKeyword))
                    .OrderBy(c => c.UserName)
                    .Skip(intSkip)
                    .Take(intPageSize)
                    .ToList();

                foreach (var item in result)
                {
                    UserExtended user = new UserExtended
                    {
                        Username = item.UserName,
                        Email = item.Email,
                        LockoutEndDateUtc = item.LockoutEndDateUtc
                    };
                    colUsers.Add(user);
                }
                
                //Tried building but no errors (!)

                //set the number of pages
                var userAsIPagedList = new StaticPagedList<UserExtended>(
                    
                    colUsers, intPage, intPageSize, intTotalPageCount
                    
                    );

                return View(userAsIPagedList);
            }
            catch (Exception ex)
            {
              ModelState.AddModelError(string.Empty, "Error: " + ex.Message);
                List<UserExtended> colUserExtendeds = new List<UserExtended>();
                return View(colUserExtendeds.ToPagedList(1, 25));
                
            }
        }

        public ActionResult ManageRole()
        {

            return View();
        }


        //Utility
        #region ApplicationUserManager

        public ApplicationUserManager UserManager
        {
            get { return _userManager ??
                    HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>(); }

            private set { _userManager = value; }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ??
                       HttpContext.GetOwinContext()
                           .GetUserManager<ApplicationRoleManager>();
            }

            private set { _roleManager = value; }
        }

        #endregion
    }
}