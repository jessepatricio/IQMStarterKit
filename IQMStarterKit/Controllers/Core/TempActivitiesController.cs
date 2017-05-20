using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using IQMStarterKit.Models;
using IQMStarterKit.Models.Core;

namespace IQMStarterKit.Controllers.Core
{
    public class TempActivitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TempActivities
        public ActionResult Index()
        {
            return View(db.TempActivities.ToList());
        }

        // GET: TempActivities/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempActivity tempActivity = db.TempActivities.Find(id);
            if (tempActivity == null)
            {
                return HttpNotFound();
            }
            return View(tempActivity);
        }

        // GET: TempActivities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TempActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TempActivityId,Title,Description,SortBy,TempModuleId,CreatedDateTime,CreatedBy,ModifiedDateTime,ModifiedBy,IsRemoved")] TempActivity tempActivity)
        {
            if (ModelState.IsValid)
            {
                db.TempActivities.Add(tempActivity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tempActivity);
        }

        // GET: TempActivities/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempActivity tempActivity = db.TempActivities.Find(id);
            if (tempActivity == null)
            {
                return HttpNotFound();
            }
            return View(tempActivity);
        }

        // POST: TempActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TempActivityId,Title,Description,SortBy,TempModuleId,CreatedDateTime,CreatedBy,ModifiedDateTime,ModifiedBy,IsRemoved")] TempActivity tempActivity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tempActivity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tempActivity);
        }

        // GET: TempActivities/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempActivity tempActivity = db.TempActivities.Find(id);
            if (tempActivity == null)
            {
                return HttpNotFound();
            }
            return View(tempActivity);
        }

        // POST: TempActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            TempActivity tempActivity = db.TempActivities.Find(id);
            db.TempActivities.Remove(tempActivity);
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
    }
}
