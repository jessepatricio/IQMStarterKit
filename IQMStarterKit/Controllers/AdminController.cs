#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using IQMStarterKit.Models;
using Microsoft.Ajax.Utilities;
using PagedList;

#endregion Includes


namespace IQMStarterKit.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        // GET: Admin
        
        #region MainList Index
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

                var colUsers = new List<ExtendedUserCustom>();
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
                    var user = new ExtendedUserCustom
                    {
                        UserName = item.UserName,
                        Email = item.Email,
                        LockoutEndDateUtc = item.LockoutEndDateUtc
                    };
                    colUsers.Add(user);
                }
                
              
                //set the number of pages
                var userAsIPagedList = new StaticPagedList<ExtendedUserCustom>(
                    
                    colUsers, intPage, intPageSize, intTotalPageCount
                    
                    );

                return View(userAsIPagedList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex.Message);
                var colUserExtendeds = new List<ExtendedUserCustom>();

                return View(colUserExtendeds.ToPagedList(1, 25));
                
            }
        }

        #endregion

        // Users ***************

        // GET: /Admin/Create
        #region Get Create
        public ActionResult Create()
        {
            var objUserExtended = new ExtendedUserCustom();
            ViewBag.Roles = GetAllRolesAsSelectList();
            return View(objUserExtended);
        }
        #endregion

        // POST: /Admin/Create
        #region POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExtendedUserCustom extUser)
        {
            try
            {
                if (extUser == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var Email = extUser.Email.Trim();
                var UserName = extUser.UserName.Trim();
                var Password = extUser.Password.Trim();

                if (Email == "") throw new Exception("No Email.");
                if (Password == "") throw new Exception("No Password.");

                //username is lowercase of the email
                UserName = Email.ToLower();
                Password = Password.ToLower();

                //create user
                var objNewAdminUser = new ApplicationUser
                {
                    UserName = UserName,
                    Email = Email
                };

                var AdminUserCreateResult = UserManager.Create(objNewAdminUser, Password);

                if (AdminUserCreateResult.Succeeded == true)
                {
                    string strNewRole = Convert.ToString(Request.Form["Roles"]);
                    if (strNewRole != "0")
                    {
                        //put user in role
                        UserManager.AddToRole(objNewAdminUser.Id, strNewRole);
                    }

                    return Redirect("~/Admin");
                }
                else
                {
                    ViewBag.Roles = GetAllRolesAsSelectList();
                    ModelState.AddModelError(string.Empty, "Error: Failed to create user. Check password constraints.");
                    return View(extUser);
                }
                
            }
            catch (Exception ex)
            {
                ViewBag.Roles = GetAllRolesAsSelectList();
                ModelState.AddModelError(string.Empty, "Error: " + ex.Message);
                return View("AddRole");
            }

        }
        #endregion

        // GET: /Admin/EditUser
        #region GET EditUser
        public ActionResult EditUser(string UserName)
        {
            if (UserName == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
          
            var objExtUserCustom = GetUser(UserName);

            if (objExtUserCustom == null) return HttpNotFound();

            return View(objExtUserCustom);
        }
        #endregion

        // POST: /Admin/EditUser
        #region POST EditUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(ExtendedUserCustom extUser)
        {
            try
            {
                if (extUser == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                var objExtendedUser = UpdateUser(extUser);
                if (objExtendedUser == null) return HttpNotFound();

                return Redirect("~/Admin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex.Message);
                return View("EditUser", GetUser(extUser.UserName));
            }
        }
        #endregion

        // DELETE: /Admin/DeleteUser
        #region DeleteUser
        public ActionResult DeleteUser(string UserName)
        {
            try
            {
                if (UserName == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
              
                if (UserName.ToLower() == this.User.Identity.Name.ToLower())
                {
                    ModelState.AddModelError(
                        string.Empty, "Error: Cannot delete the current user");

                    return View("EditUser");
                }

                var objExtendedUser = GetUser(UserName);

                if (objExtendedUser == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    DeleteUser(objExtendedUser);
                }

                return Redirect("~/Admin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex.Message);
                return View("EditUser", GetUser(UserName));
            }
        }
        #endregion

        // GET: /Admin/EditRoles
        #region GET EditRoles
        public ActionResult EditRoles(string userName)
        {
            if (userName == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
           
            userName = userName.ToLower();

            // Check that we have an actual user
            var objExpandedUser = GetUser(userName);

            if (objExpandedUser == null)
            {
                return HttpNotFound();
            }

            var objUserAndRoles =
                GetUserAndRoles(userName);

            return View(objUserAndRoles);
        }
        #endregion

        // PUT: /Admin/EditRoles
        [HttpPost]
        [ValidateAntiForgeryToken]
        #region POST EditRoles
        public ActionResult EditRoles(UserAndRolesCustom userAndRoles)
        {
            try
            {
                if (userAndRoles == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                string userName = userAndRoles.UserName;
                string strNewRole = Convert.ToString(Request.Form["AddRole"]);

                if (strNewRole != "No Roles Found")
                {
                    // Go get the User
                    var user = UserManager.FindByName(userName);

                    // Put user in role
                    UserManager.AddToRole(user.Id, strNewRole);
                }

                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(userName));

                var objUserAndRoles = GetUserAndRoles(userName);


                return View(objUserAndRoles);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("EditRoles");
            }
        }
        #endregion

        // DELETE: /Admin/DeleteRole
        #region DELETE DeleteRole
        public ActionResult DeleteRole(string userName, string roleName)
        {
            try
            {
                if ((userName == null) || (roleName == null))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                userName = userName.ToLower();

                // Check that we have an actual user
                ExtendedUserCustom objExpandedUser = GetUser(userName);

                if (objExpandedUser == null)
                {
                    return HttpNotFound();
                }

                if (userName.ToLower() ==
                    this.User.Identity.Name.ToLower() && roleName == "Administrator")
                {
                    ModelState.AddModelError(string.Empty,
                        "Error: Cannot delete Administrator Role for the current user");
                }

                // Go get the User
                ApplicationUser user = UserManager.FindByName(userName);
                // Remove User from role
                UserManager.RemoveFromRoles(user.Id, roleName);
                UserManager.Update(user);

                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(userName));

                return RedirectToAction("EditRoles", new { UserName = userName });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);

                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(userName));

                var objUserAndRolesDTO =
                    GetUserAndRoles(userName);

                return View("EditRoles", objUserAndRolesDTO);
            }
        }
        #endregion

        // Roles ****************************

        // GET: /Admin/ManageRoles
        #region ManageRoles
        public ActionResult ManageRoles()
        {
            var roleManager = 
                new RoleManager<IdentityRole>
                (
                    new RoleStore<IdentityRole>(new ApplicationDbContext())
                    );

            List<RoleCustom> colRole = (from objRole in roleManager.Roles
                select new RoleCustom
                {
                    Id = objRole.Id,
                    RoleName = objRole.Name
                }).ToList();

            return View(colRole);
        }
        #endregion

        // GET: /Admin/AddRole
        #region GET AddRole
        public ActionResult AddRole()
        {
            var objRole = new RoleCustom();
            return View(objRole);
        }
        #endregion

        // POST: /Admin/AddRole
        #region POST AddRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRole(RoleCustom role)
        {
            try
            {
                if (role == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
               
                var roleName = role.RoleName.Trim();

                if (roleName == "")
                {
                    throw new Exception("No RoleName");
                }

                // Create Role
                var roleManager =
                    new RoleManager<IdentityRole>
                    (
                        new RoleStore<IdentityRole>(new ApplicationDbContext())
                         );

                if (!roleManager.RoleExists(roleName)) roleManager.Create(new IdentityRole(roleName));
               
                return Redirect("~/Admin/ViewAllRoles");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex.Message);
                return View("AddRole");
            }
        }
        #endregion

        // DELETE: /Admin/DeleteUserRole
        #region DeleteUserRole
        public ActionResult DeleteUserRole(string roleName)
        {
            try
            {
                if (roleName == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (roleName.ToLower() == "administrator")
                {
                    throw new Exception(String.Format("Cannot delete {0} Role.", roleName));
                }

                var roleManager =
                    new RoleManager<IdentityRole>(
                        new RoleStore<IdentityRole>(new ApplicationDbContext()));

                var usersInRole = roleManager.FindByName(roleName).Users.Count();
                if (usersInRole > 0)
                {
                    throw new Exception($"Canot delete {roleName} Role because it still has users.");            
                }

                var objRoleToDelete = (from objRole in roleManager.Roles
                    where objRole.Name == roleName
                    select objRole).FirstOrDefault();
                if (objRoleToDelete != null)
                {
                    roleManager.Delete(objRoleToDelete);
                }
                else
                {
                    throw new Exception($"Cannot delete { roleName } Role does not exist.");
                }

                var colRoleDTO = (from objRole in roleManager.Roles
                    select new RoleCustom
                    {
                        Id = objRole.Id,
                        RoleName = objRole.Name
                    }).ToList();

                return View("ManageRoles", colRoleDTO);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex.Message);

                var roleManager =
                    new RoleManager<IdentityRole>(
                        new RoleStore<IdentityRole>(new ApplicationDbContext()));

                List<RoleCustom> colRole = (from objRole in roleManager.Roles
                    select new RoleCustom
                    {
                        Id = objRole.Id,
                        RoleName = objRole.Name
                    }).ToList();

                return View("ManageRoles", colRole);
            }
        }
        #endregion
        



        //Utility

        #region ApplicationUserManager
        public ApplicationUserManager UserManager
        {
            get { return _userManager ??
                    HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>(); }

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

        #region GetAllRolesAsSelectList
        private List<SelectListItem> GetAllRolesAsSelectList()
        {
            var SelectRoleListItems = new List<SelectListItem>();
            var roleManager =
                new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var colRoleSelectList = roleManager.Roles.OrderBy(x => x.Name).ToList();
            SelectRoleListItems.Add(
                new SelectListItem
                {
                    Text = "Select",
                    Value = "0"
                });
            foreach (var item in colRoleSelectList)
            {
                SelectRoleListItems.Add(
                    new SelectListItem
                    {
                        Text = item.Name.ToString(),
                        Value = item.Name.ToString()
                    });
            }
            return SelectRoleListItems;
        }
        #endregion

        #region GetUser
        private ExtendedUserCustom GetUser(string userName)
        {
            var objExtUser = new ExtendedUserCustom();
            var result = UserManager.FindByName(userName);

            //not found throw exception
            if (result == null) throw new Exception("User not found.");

            objExtUser.UserName = result.UserName;
            objExtUser.Email = result.Email;
            objExtUser.LockoutEndDateUtc = result.LockoutEndDateUtc;
            objExtUser.AccessFailedCount = result.AccessFailedCount;
            objExtUser.PhoneNumber = result.PhoneNumber;

            return objExtUser;

        }
        #endregion

        #region UpdateUser
        private ExtendedUserCustom UpdateUser(ExtendedUserCustom extUser)
        {
            var result = UserManager.FindByName(extUser.UserName);

            //not found
            if (result == null) throw  new Exception("User not found.");

            result.Email = extUser.Email;

            //is account locked? unlock it
            if (UserManager.IsLockedOut(result.Id)) UserManager.ResetAccessFailedCount(result.Id);

            UserManager.Update(result);

            //was a password sent across?
            if (!string.IsNullOrEmpty(extUser.Password))
            {
                //remove current password
                var removePassword = UserManager.RemovePassword(result.Id);
                if (removePassword.Succeeded)
                {
                    //add new password
                    var AddPassword = UserManager.AddPassword(
                        result.Id,
                        extUser.Password
                    );

                    if (AddPassword.Errors.Count() > 0)
                    {
                       throw  new Exception(AddPassword.Errors.FirstOrDefault());
                    }
                }
            }

            return extUser;
        }
        #endregion

        #region DeleteUser
        private void DeleteUser(ExtendedUserCustom extUser)
        {
            var user = UserManager.FindByName(extUser.UserName);
            
            //not found?
            if (user == null) throw new Exception("User not found.");

            UserManager.RemoveFromRoles(user.Id, UserManager.GetRoles(user.Id).ToArray());
            UserManager.Update(user);
            UserManager.Delete(user);
        }
        #endregion

        #region GetUserAndRoles
        private UserAndRolesCustom GetUserAndRoles(string UserName)
        {
            // Go get the User
            ApplicationUser user = UserManager.FindByName(UserName);

            List<UserRoleCustom> colUserRoleDTO =
            (from objRole in UserManager.GetRoles(user.Id)
                select new UserRoleCustom
                {
                    RoleName = objRole,
                    UserName = UserName
                }).ToList();

            if (colUserRoleDTO.Count() == 0)
            {
                colUserRoleDTO.Add(new UserRoleCustom{ RoleName = "No Roles Found" });
            }

            ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));
            
            // Create UserRolesAndPermissionsDTO
            UserAndRolesCustom objUserAndRolesDTO =
                new UserAndRolesCustom
                {
                    UserName = UserName,
                    colUserRole = colUserRoleDTO
                };

            return objUserAndRolesDTO;
        }
        #endregion

        #region RolesUserIsNotIn
        private List<string> RolesUserIsNotIn(string UserName)
        {
            // Get roles the user is not in
            var colAllRoles = RoleManager.Roles.Select(x => x.Name).ToList();
            // Go get the roles for an individual
            ApplicationUser user = UserManager.FindByName(UserName);
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