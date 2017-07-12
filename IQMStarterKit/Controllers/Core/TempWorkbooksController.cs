using IQMStarterKit.Models;
using IQMStarterKit.Models.Alert;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace IQMStarterKit.Controllers.Core
{
    [Authorize(Roles = "Administrator")]
    public class TempWorkbooksController : CommonController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();


        // GET: TempWorkbooks
        public ActionResult Index()
        {
            Session["email"] = null;

            return View(_context.TempWorkbooks.Where(m => m.IsRemoved == false).ToList());
        }

        // GET: TempWorkbooks/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempWorkbook tempWorkbook = _context.TempWorkbooks.Find(id);


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

            ModelState.Remove("CreatedBy");
            ModelState.Remove("ModifiedBy");
            ModelState.Remove("CreatedDateTime");
            ModelState.Remove("ModifiedDateTime");
            ModelState.Remove("TempModules");

            if (ModelState.IsValid)
            {
                //update hidden fields

                tempWorkbook.CreatedBy = GetSessionUserId();
                tempWorkbook.CreatedDateTime = DateTime.Now;

                tempWorkbook.ModifiedBy = GetSessionUserId();
                tempWorkbook.ModifiedDateTime = DateTime.Now;

                tempWorkbook.IsRemoved = false;

                _context.TempWorkbooks.Add(tempWorkbook);
                _context.SaveChanges();
                return RedirectToAction("Index").WithSuccess("Created Successfully!");
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
            TempWorkbook tempWorkbook = _context.TempWorkbooks.Find(id);
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
        //public ActionResult Edit([Bind(Include = "TempWorkbookId,Title,Description,Version,CreatedBy,CreatedDateTime,ModifiedBy,ModifiedDateTime,IsDeleted")] TempWorkbook tempWorkbook)
        public ActionResult Edit(int id)
        {

            //get the original records from db
            var tmpWrkBook = new TempWorkbook();
            tmpWrkBook = _context.TempWorkbooks.Find(id);

            //update the class record
            UpdateModel<ITempWorkbook>(tmpWrkBook);


            if (tmpWrkBook != null)
            {
                //update record stamp
                tmpWrkBook.ModifiedDateTime = DateTime.Now;
                tmpWrkBook.ModifiedBy = GetSessionUserId();


                if (ModelState.IsValid)
                {
                    //save changes
                    _context.Entry(tmpWrkBook).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("Index").WithSuccess("Updated Successfully!");
                }
            }

            return View(tmpWrkBook);
        }

        // GET: TempWorkbooks/Delete/5
        //public ActionResult Delete(byte? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TempWorkbook tempWorkbook = _context.TempWorkbooks.Find(id);
        //    if (tempWorkbook == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    //get user fullname
        //    tempWorkbook.CreatedBy = GetFullName(tempWorkbook.CreatedBy);
        //    tempWorkbook.ModifiedBy = GetFullName(tempWorkbook.ModifiedBy);
        //    return View(tempWorkbook);
        //}

        // POST: TempWorkbooks/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(byte id)
        //{
        //    TempWorkbook tempWorkbook = _context.TempWorkbooks.Find(id);
        //    if (tempWorkbook != null) tempWorkbook.IsRemoved = true; //soft delete
        //    _context.Entry(tempWorkbook).State = EntityState.Modified;
        //    //db.TempWorkbooks.Remove(tempWorkbook);
        //    _context.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _context.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

    }
}
