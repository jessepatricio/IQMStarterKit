using IQMStarterKit.Models;
using IQMStarterKit.Models.Alert;
using IQMStarterKit.Models.Core;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace IQMStarterKit.Controllers.Core
{
    [Authorize(Roles = "Administrator")]
    public class TempModulesController : CommonController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();


        // GET: TempModules
        public ActionResult Index()
        {
            var tempModule = _context.TempModules.Where(m => m.IsRemoved == false).ToList();
            foreach (var item in tempModule)
            {
                //get all module activities
                item.TempActivities = _context.TempActivities.Where(m => m.TempModuleId == item.TempModuleId).ToList();
            }

            return View(tempModule);
        }

        // GET: TempModules/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempModule tempModule = _context.TempModules.Find(id);
            if (tempModule == null)
            {
                return HttpNotFound();
            }

            //get user fullname
            tempModule.CreatedBy = GetFullName(tempModule.CreatedBy);
            tempModule.ModifiedBy = GetFullName(tempModule.ModifiedBy);

            return View(tempModule);
        }

        // GET: TempModules/Create
        public ActionResult Create()
        {
            TempModuleViewModels tempModule = new TempModuleViewModels();
            tempModule.TempWorkbooks = _context.TempWorkbooks.Where(m => m.IsRemoved == false).ToList();

            return View(tempModule);
        }

        // POST: TempModules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TempModuleId,Title,Description,SortOrder,TempWorkbookId,CreatedDateTime,CreatedBy,ModifiedDateTime,ModifiedBy,IsRemoved")] TempModuleViewModels tempModule)
        {

            var module = new TempModule();

            module.Title = tempModule.Title;
            module.Description = tempModule.Description;
            module.SortOrder = tempModule.SortOrder;
            module.TempWorkbookId = tempModule.TempWorkbookId;

            if (ModelState.IsValid)
            {
                //assign system fields
                module.CreatedDateTime = DateTime.Now;
                module.CreatedBy = GetSessionUserId();

                module.ModifiedDateTime = DateTime.Now;
                module.ModifiedBy = GetSessionUserId();

                _context.TempModules.Add(module);
                _context.SaveChanges();
                return RedirectToAction("Index").WithSuccess("Module created successfully!");
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
            TempModule tempModule = _context.TempModules.Find(id);
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
            var recModule = _context.TempModules.Find(tempModule.TempModuleId);

            if (ModelState.IsValid)
            {
                if (recModule != null)
                {
                    recModule.Title = tempModule.Title;
                    recModule.Description = tempModule.Description;
                    recModule.SortOrder = tempModule.SortOrder;
                    recModule.TempWorkbookId = tempModule.TempWorkbookId;
                    recModule.CreatedBy = tempModule.CreatedBy;
                    recModule.CreatedDateTime = tempModule.CreatedDateTime;
                    //update date stamp
                    recModule.ModifiedDateTime = DateTime.Now;
                    recModule.ModifiedBy = GetSessionUserId();

                    _context.Entry(recModule).State = EntityState.Modified;
                }
                _context.SaveChanges();
                return RedirectToAction("Index").WithSuccess("Module updated successfully!");
            }
            return View(recModule);
        }

        // GET: TempModules/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempModule tempModule = _context.TempModules.Find(id);
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
            TempModule tempModule = _context.TempModules.Find(id);

            tempModule.IsRemoved = true;


            _context.Entry(tempModule).State = EntityState.Modified;

            //_context.TempModules.Remove(tempModule);
            _context.SaveChanges();
            return RedirectToAction("Index").WithSuccess("Module successfully deleted!");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
