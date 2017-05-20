using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using IQMStarterKit.Models;
using IQMStarterKit.Models.Core;

namespace IQMStarterKit.Controllers.Core
{
    public class TempModulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        // GET: TempModules
        public ActionResult Index()
        {
            return View(db.TempModules.Where(m=>m.IsRemoved==false).ToList());
        }

        // GET: TempModules/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempModule tempModule = db.TempModules.Find(id);
            if (tempModule == null)
            {
                return HttpNotFound();
            }
            return View(tempModule);
        }

        // GET: TempModules/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TempModules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TempModuleId,Title,Description,SortOrder,TempWorkbookId,CreatedDateTime,CreatedBy,ModifiedDateTime,ModifiedBy,IsRemoved")] TempModule tempModule)
        {
            if (ModelState.IsValid)
            {
                db.TempModules.Add(tempModule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tempModule);
        }

        // GET: TempModules/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempModule tempModule = db.TempModules.Find(id);
            if (tempModule == null)
            {
                return HttpNotFound();
            }
            return View(tempModule);
        }

        // POST: TempModules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TempModuleId,Title,Description,SortOrder,TempWorkbookId,CreatedDateTime,CreatedBy,ModifiedDateTime,ModifiedBy,IsRemoved")] TempModule tempModule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tempModule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tempModule);
        }

        // GET: TempModules/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempModule tempModule = db.TempModules.Find(id);
            if (tempModule == null)
            {
                return HttpNotFound();
            }
            return View(tempModule);
        }

        // POST: TempModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            TempModule tempModule = db.TempModules.Find(id);
            db.TempModules.Remove(tempModule);
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
