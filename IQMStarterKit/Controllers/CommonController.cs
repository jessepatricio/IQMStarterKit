using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace IQMStarterKit.Models
{
    public class CommonController : Controller 
    {

        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        //Utility

        #region ApplicationUserManager
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??
                       HttpContext.GetOwinContext()
                           .GetUserManager<ApplicationUserManager>();
            }

            private set { _userManager = value; }
        }
        #endregion

        #region ApplicationRoleManager
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

        public string GetSessionUserId()
        {
            return UserManager.FindByEmail(User.Identity.Name).Id.ToString();
        }

        public string GetFullName(string userId)
        {
            return (userId != null) ? UserManager.FindById(userId).FullName.ToString() : String.Empty;
        }

        #region RolesUserIsNotIn
        public List<string> RolesUserIsNotIn(string email)
        {
            // Get roles the user is not in
            var colAllRoles = RoleManager.Roles.Select(x => x.Name).ToList();
            // Go get the roles for an individual
            ApplicationUser user = UserManager.FindByEmail(email);
            // If we could not find the user, throw an exception
            if (user == null)
            {
                throw new Exception("Could not find the User");
            }
            var colRolesForUser = UserManager.GetRoles(user.Id).ToList();
            var colRolesUserInNotIn = (from objRole in colAllRoles
                where !colRolesForUser.Contains(objRole)
                select objRole).ToList();
            if (colRolesUserInNotIn.Count() == 0)
            {
                colRolesUserInNotIn.Add("No Roles Found");
            }
            return colRolesUserInNotIn;
        }
        #endregion


        
    }
}