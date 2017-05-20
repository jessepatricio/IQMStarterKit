using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IQMStarterKit.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace IQMStarterKit.Controllers.Core
{
    [Authorize]
    public class TempWorkbooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;


        // GET: TempWorkbooks
        public ActionResult Index()
        {
            return View(db.TempWorkbooks.Where(m=>m.IsRemoved==false).ToList());
        }

        // GET: TempWorkbooks/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempWorkbook tempWorkbook = db.TempWorkbooks.Find(id);


            if (tempWorkbook == null)
            {
                return HttpNotFound();
            }

            //get user fullname
            tempWorkbook.CreatedBy = GetFullName(tempWorkbook.CreatedBy);
            tempWorkbook.ModifiedBy = GetFullName(tempWorkbook.ModifiedBy);

            return View(tempWorkbook);
        }

        // GET: TempWorkbooks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TempWorkbooks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TempWorkbookId,Title,Description,Version,CreatedDateTime,CreatedBy,ModifiedDateTime,ModifiedBy,IsRemoved")] TempWorkbook tempWorkbook)
        {
            if (ModelState.IsValid)
            {
                //update hidden fields

                tempWorkbook.CreatedBy = GetSessionUserId();
                tempWorkbook.CreatedDateTime = DateTime.Now;
                
                tempWorkbook.ModifiedBy = GetSessionUserId();
                tempWorkbook.ModifiedDateTime = DateTime.Now;

                tempWorkbook.IsRemoved = false;

                db.TempWorkbooks.Add(tempWorkbook);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tempWorkbook);
        }

        // GET: TempWorkbooks/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempWorkbook tempWorkbook = db.TempWorkbooks.Find(id);
            if (tempWorkbook == null)
            {
                return HttpNotFound();
            }
            return View(tempWorkbook);
        }

        // POST: TempWorkbooks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TempWorkbookId,Title,Description,Version,CreatedBy,CreatedDateTime,ModifiedBy,ModifiedDateTime,IsDeleted")] TempWorkbook tempWorkbook)
        {
           
            //get the original records from db
            var tmpWrkBook = new TempWorkbook();
            tmpWrkBook = db.TempWorkbooks.Find(tempWorkbook.TempWorkbookId);

            //update the record
            if (tmpWrkBook != null)
            {
                tmpWrkBook.Title = tempWorkbook.Title;
                tmpWrkBook.Version = tempWorkbook.Version;
                tmpWrkBook.Description = tempWorkbook.Description;
           
                tmpWrkBook.ModifiedDateTime = DateTime.Now;
                tmpWrkBook.ModifiedBy = GetSessionUserId();


                if (ModelState.IsValid)
                {
                    db.Entry(tmpWrkBook).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(tempWorkbook);
        }

        // GET: TempWorkbooks/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempWorkbook tempWorkbook = db.TempWorkbooks.Find(id);
            if (tempWorkbook == null)
            {
                return HttpNotFound();
            }

            //get user fullname
            tempWorkbook.CreatedBy = GetFullName(tempWorkbook.CreatedBy);
            tempWorkbook.ModifiedBy = GetFullName(tempWorkbook.ModifiedBy);
            return View(tempWorkbook);
        }

        // POST: TempWorkbooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            TempWorkbook tempWorkbook = db.TempWorkbooks.Find(id);
            if (tempWorkbook != null) tempWorkbook.IsRemoved = true; //soft delete
            //db.TempWorkbooks.Remove(tempWorkbook);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


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

        public string GetSessionUserId()
        {
            return UserManager.FindByEmail(User.Identity.Name).Id.ToString();
        }

        public string GetFullName(string userId)
        {
            return UserManager.FindById(userId).FullName.ToString();
        }
    }
}
