using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace IQMStarterKit.Models
{
    public class CommonController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();

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



        public IEnumerable<ApplicationUser> GetGroupTutors(byte groupId)
        {



            var tutors = new List<ApplicationUser>();


            // get all users with tutor role
            var users = _context.Users.Include(u => u.Roles).Where(u => u.Roles.Any(r => r.RoleId == "882f1ae2-fb15-4bc7-9d54-ea9785a41399")).ToList();
            // get all tutors assigned to the group id
            var groupTutors = _context.GroupTutorModels.Where(m => m.GroupId == groupId).ToList();

            foreach (var user in users)
            {

                //include only tutors assigned in group
                var x = groupTutors.Where(m => m.TutorId == user.Id).FirstOrDefault();
                if (x != null)
                {
                    tutors.Add(user);
                }


            }

            return tutors;
        }


        public IEnumerable<ApplicationUser> GetTutors(byte groupId)
        {


            var tutors = new List<ApplicationUser>();


            // get all users with tutor role
            var users = _context.Users.Include(u => u.Roles).Where(u => u.Roles.Any(r => r.RoleId == "882f1ae2-fb15-4bc7-9d54-ea9785a41399")).ToList();
            // get all tutors assigned to the group id
            var groupTutors = _context.GroupTutorModels.Where(m => m.GroupId == groupId).ToList();

            foreach (var user in users)
            {

                //include only tutors not  assigned in group
                var x = groupTutors.Where(m => m.TutorId == user.Id).FirstOrDefault();
                if (x == null)
                {
                    tutors.Add(user);
                }


            }



            return tutors;

        }

        public IQueryable<ApplicationUser> GetUsersInRole(string roleId)
        {
            return from user in _context.Users
                   where user.Roles.Any(r => r.RoleId == roleId)
                   select user;
        }

    }
}