using IQMStarterKit.Models;
using IQMStarterKit.Models.Alert;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;


namespace IQMStarterKit.Controllers
{
    [Authorize(Roles = "Administrator,Tutor")]
    public class GroupModelsController : CommonController
    {
        private const string TutorId = "882f1ae2-fb15-4bc7-9d54-ea9785a41399";
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        // GET: GroupModels
        public ActionResult Index()
        {
            Session["email"] = null;
            var grpModels = new List<GroupModel>();
            //get only groups assigned for tutors
            if (User.IsInRole("Administrator"))
            {
                grpModels = _context.GroupModels.Where(m => m.IsRemoved == false).OrderByDescending(m => m.GroupName).ToList();

            }
            else
            {
                //filter user with tutor's assigned group only
                var tutorId = User.Identity.GetUserId();
                //get tutor assigned groups
                var tutorGroups = _context.GroupTutorModels.Where(m => m.TutorId == tutorId).Select(m => m.GroupId).ToList();
                // get all groupd id for the current tutor
                grpModels = _context.GroupModels.Where(m => m.IsRemoved == false && tutorGroups.Contains(m.GroupId)).OrderByDescending(m => m.GroupName).ToList();
            }



            return View(grpModels);
        }

        // GET: GroupModels/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupModel groupModel = _context.GroupModels.Find(id);
            if (groupModel == null)
            {
                return HttpNotFound();
            }

            groupModel.CreatedBy = GetFullName(groupModel.CreatedBy);
            groupModel.ModifiedBy = GetFullName(groupModel.ModifiedBy);

            return View(groupModel);
        }

        // GET: GroupModels/Create
        public ActionResult Create()
        {
            ViewBag.Tutors = GetUsersInRole(TutorId);

            return View();
        }

        // POST: GroupModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GroupModel grpModel)
        {

            var existGroup = _context.GroupModels.Where(m => m.GroupName == grpModel.GroupName);
            if (existGroup.Any())
            {
                //ModelState.AddModelError(String.Empty, @"GroupName already existe!");
                ViewBag.Tutors = GetUsersInRole(TutorId);
                return RedirectToAction("Create").WithError("Group already existed!");
            }


            var groupModel = new GroupModel();

            //remove null error field from form to get modelstate to true
            ModelState.Remove("ModifiedBy");
            ModelState.Remove("CreatedBy");

            if (ModelState.IsValid)
            {
                //assign system fields
                groupModel.CreatedDateTime = DateTime.Now;
                groupModel.CreatedBy = GetSessionUserId();

                groupModel.ModifiedDateTime = DateTime.Now;
                groupModel.ModifiedBy = GetSessionUserId();

                //update model
                UpdateModel<IGroupModel>(groupModel);

                _context.GroupModels.Add(groupModel);
                _context.SaveChanges();

                return RedirectToAction("Index").WithSuccess("Group Added Successfullly!");
            }

            return View(groupModel);
        }

        // GET: GroupModels/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GroupModel groupModel = _context.GroupModels.Find(id);


            if (groupModel == null)
            {
                return HttpNotFound();
            }

            ViewBag.Tutors = GetUsersInRole(TutorId);

            return View(groupModel);
        }

        // POST: GroupModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupId,GroupName,TutorId,Description,CreatedDateTime,CreatedBy,ModifiedDateTime,ModifiedBy,IsRemoved")] GroupModel groupModel)
        {


            ModelState.Remove("ModifiedBy");

            if (ModelState.IsValid)
            {


                groupModel.ModifiedDateTime = DateTime.Now;
                groupModel.ModifiedBy = GetSessionUserId();

                _context.Entry(groupModel).State = EntityState.Modified;
                _context.SaveChanges();

                return RedirectToAction("Index").WithSuccess("Updated successfully!");
            }
            else
            {
                return View(groupModel).WithError("Invalid Update!");
            }



        }

        // GET: GroupModels/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupModel groupModel = _context.GroupModels.Find(id);
            if (groupModel == null)
            {
                return HttpNotFound();
            }
            //get user fullname
            groupModel.CreatedBy = GetFullName(groupModel.CreatedBy);
            groupModel.ModifiedBy = GetFullName(groupModel.ModifiedBy);
            return View(groupModel);
        }

        // POST: GroupModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            GroupModel groupModel = _context.GroupModels.Find(id);
            if (groupModel != null) groupModel.IsRemoved = true; //soft delete
            _context.Entry(groupModel).State = EntityState.Modified;
            //db.GroupModels.Remove(groupModel);
            _context.SaveChanges();

            return RedirectToAction("Index").WithSuccess("Deleted successfully!");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        public IQueryable<ApplicationUser> GetUsersInRole(string roleId)
        {
            return from user in _context.Users
                   where user.Roles.Any(r => r.RoleId == roleId)
                   select user;
        }
    }
}
