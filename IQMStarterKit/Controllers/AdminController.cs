#region Includes
using IQMStarterKit.Models;
using IQMStarterKit.Models.Alert;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

#endregion Includes


namespace IQMStarterKit.Controllers
{
    [Authorize]
    public class AdminController : CommonController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Admin

        #region MainList Index
        public ActionResult Index(string searchKeyword, string currentFilter, int? page)
        {
            try
            {
                var colUsers = new List<ExtendedUserCustom>();

                var result = UserManager.Users
                    .OrderBy(c => c.FullName)
                    .ToList();

                foreach (var item in result)
                {
                    var user = new ExtendedUserCustom
                    {
                        Email = item.Email,
                        FullName = item.FullName,
                        GroupId = item.GroupId
                    };
                    colUsers.Add(user);
                }

                //get group list for dropdown
                ViewBag.Groups = _context.GroupModels.Where(m => m.IsRemoved == false)
                    .OrderByDescending(m => m.CreatedDateTime).ToList();

                return View(colUsers);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex.Message);
                var colUserExtendeds = new List<ExtendedUserCustom>();
                return View(colUserExtendeds);
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
        public ActionResult Create(ExtendedUserCustom model)
        {
            var user = new ApplicationUser { UserName = model.Email.Trim(), Email = model.Email.Trim(), FullName = model.FullName.Trim() };
            try
            {

                var adminUserCreateResult = UserManager.Create(user, model.Password);

                if (adminUserCreateResult.Succeeded == true)
                {
                    string strNewRole = Convert.ToString(Request.Form["Roles"]);
                    if (strNewRole != "0")
                    {
                        //put user in role
                        UserManager.AddToRole(user.Id, strNewRole);
                    }

                    return Redirect("~/Admin").WithSuccess("User created successfully!");
                }
                else
                {
                    ViewBag.Roles = GetAllRolesAsSelectList();
                    //ModelState.AddModelError(string.Empty, @"Error: Failed to create user. Check Role constraints.");
                    return View(model).WithError("Failed to create user. Check role contrainsts.");
                }

            }
            catch (Exception ex)
            {
                ViewBag.Roles = GetAllRolesAsSelectList();
                ModelState.AddModelError(string.Empty, @"Error: " + ex.Message);
                return View(model);
            }

        }
        #endregion

        // GET: /Admin/EditUser
        #region GET EditUser
        public ActionResult EditUser(string email)
        {
            if (email == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var objExtUserCustom = GetUser(email);
            var userroles = GetUserAndRoles(email);

            var role = userroles.colUserRole.FirstOrDefault(m => m.RoleName == "Student");
            ViewBag.IsStudent = (role != null) ? true : false;

            objExtUserCustom.Groups = _context.GroupModels.Where(m => m.IsRemoved == false)
                .OrderByDescending(m => m.CreatedBy).ToList();



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

                return Redirect("~/Admin").WithSuccess("User updated successfully!");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, @"Error: " + ex.Message);
                return View("EditUser", GetUser(extUser.Email));
            }
        }
        #endregion

        // DELETE: /Admin/DeleteUser
        #region DeleteUser

        public ActionResult DeleteUser(string email)
        {
            try
            {
                if (email == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                if (email.ToLower() == this.User.Identity.Name.ToLower())
                {
                    ModelState.AddModelError(
                        string.Empty, "Error: Cannot delete the current user");

                    return View("EditUser");
                }

                var objExtendedUser = GetUser(email);

                if (objExtendedUser == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    DeleteUser(objExtendedUser);
                }

                ViewBag.email = "";
                return Redirect("~/Admin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex.Message);
                return View("EditUser", GetUser(email));
            }
        }
        #endregion

        // GET: /Admin/EditRoles
        #region GET EditRoles
        public ActionResult EditRoles(string email)
        {
            if (email == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            email = email.ToLower();

            // Check that we have an actual user
            var objExpandedUser = GetUser(email);

            if (objExpandedUser == null)
            {
                return HttpNotFound();
            }

            var objUserAndRoles =
                GetUserAndRoles(email);

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

                string email = userAndRoles.Email;
                string strNewRole = Convert.ToString(Request.Form["AddRole"]);

                if (strNewRole != "No Roles Found")
                {
                    // Go get the User
                    var user = UserManager.FindByEmail(email);

                    // Put user in role
                    UserManager.AddToRole(user.Id, strNewRole);
                }

                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(email));

                var objUserAndRoles = GetUserAndRoles(email);


                return View(objUserAndRoles);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, @"Error: " + ex);
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

        // GET: /Admin/ManageRole
        #region ManageRole
        public ActionResult ManageRole()
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

                return Redirect("~/Admin/ManageRole").WithSuccess("Role added successfully!");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, @"Error: " + ex.Message);
                return View("AddRole");
            }
        }
        #endregion

        // DELETE: /Admin/DeleteUserRole
        #region DeleteUserRole
        [Authorize(Roles = "Administrator")]
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
                    throw new Exception($"Cannot delete {roleName} Role.");
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

                var colRoleDto = (from objRole in roleManager.Roles
                                  select new RoleCustom
                                  {
                                      Id = objRole.Id,
                                      RoleName = objRole.Name
                                  }).ToList();

                return View("ManageRole", colRoleDto).WithSuccess("Role deleted successfully!");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, @"Error: " + ex.Message);

                var roleManager =
                    new RoleManager<IdentityRole>(
                        new RoleStore<IdentityRole>(new ApplicationDbContext()));

                List<RoleCustom> colRole = (from objRole in roleManager.Roles
                                            select new RoleCustom
                                            {
                                                Id = objRole.Id,
                                                RoleName = objRole.Name
                                            }).ToList();

                return View("ManageRole", colRole);
            }
        }
        #endregion


        // Tutor's administration
        public ActionResult ListStudents()
        {
            var colUsers = new List<ExtendedUserCustom>();

            var users = UserManager.Users.ToList();

            foreach (var item in users)
            {


                var user = new ExtendedUserCustom
                {

                    Email = item.Email,
                    FullName = item.FullName,
                    GroupId = item.GroupId,
                    ProgressValue = item.OverallProgress

                };
                colUsers.Add(user);
            }

            //get group list for dropdownlist
            ViewBag.Groups = _context.GroupModels.Where(m => m.IsRemoved == false)
                .OrderByDescending(m => m.CreatedDateTime).ToList();

            return View(colUsers);

        }



        //Utility

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
        private ExtendedUserCustom GetUser(string email)
        {
            var objExtUser = new ExtendedUserCustom();
            var result = UserManager.FindByEmail(email);

            //not found throw exception
            if (result == null) throw new Exception("User not found.");

            objExtUser.Email = result.Email;
            objExtUser.FullName = result.FullName;
            objExtUser.GroupId = result.GroupId;

            return objExtUser;

        }
        #endregion

        #region UpdateUser
        private ExtendedUserCustom UpdateUser(ExtendedUserCustom extUser)
        {
            var result = UserManager.FindByEmail(extUser.Email);

            //not found
            if (result == null) throw new Exception("User not found.");

            result.Email = extUser.Email.Trim();
            result.FullName = extUser.FullName.Trim();
            result.GroupId = extUser.GroupId;
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
                        throw new Exception(AddPassword.Errors.FirstOrDefault());
                    }
                }
            }

            return extUser;
        }
        #endregion

        #region DeleteUser
        private void DeleteUser(ExtendedUserCustom extUser)
        {
            var user = UserManager.FindByEmail(extUser.Email);

            //not found?
            if (user == null) throw new Exception("User not found.");

            UserManager.RemoveFromRoles(user.Id, UserManager.GetRoles(user.Id).ToArray());
            UserManager.Update(user);
            UserManager.Delete(user);


        }
        #endregion

        #region GetUserAndRoles
        private UserAndRolesCustom GetUserAndRoles(string email)
        {
            // Go get the User
            ApplicationUser user = UserManager.FindByEmail(email);

            List<UserRoleCustom> colUserRoleDTO =
            (from objRole in UserManager.GetRoles(user.Id)
             select new UserRoleCustom
             {
                 RoleName = objRole,
                 Email = user.Email
             }).ToList();

            if (!colUserRoleDTO.Any())
            {
                colUserRoleDTO.Add(new UserRoleCustom { RoleName = "No Roles Found" });
            }

            ViewBag.AddRole = new SelectList(RolesUserIsNotIn(email));

            // Create UserRolesAndPermissionsDTO
            UserAndRolesCustom objUserAndRolesDTO =
                new UserAndRolesCustom
                {
                    Email = user.Email,
                    colUserRole = colUserRoleDTO
                };

            return objUserAndRolesDTO;
        }
        #endregion



    }
}