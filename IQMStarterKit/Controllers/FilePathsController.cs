using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IQMStarterKit.Models;

namespace IQMStarterKit.Controllers
{
    public class FilePathsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FilePaths
        public ActionResult Index()
        {
            return View(db.FilePaths.ToList());
        }

        // GET: FilePaths/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilePath filePath = db.FilePaths.Find(id);
            if (filePath == null)
            {
                return HttpNotFound();
            }
            return View(filePath);
        }

        // GET: FilePaths/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FilePaths/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FilePathId,FileName,FileType,ContentType,Content,StudentActivityId")] FilePath filePath)
        {
            if (ModelState.IsValid)
            {
                db.FilePaths.Add(filePath);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(filePath);
        }

        // GET: FilePaths/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilePath filePath = db.FilePaths.Find(id);
            if (filePath == null)
            {
                return HttpNotFound();
            }
            return View(filePath);
        }

        // POST: FilePaths/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FilePathId,FileName,FileType,ContentType,Content,StudentActivityId")] FilePath filePath)
        {
            if (ModelState.IsValid)
            {
                db.Entry(filePath).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(filePath);
        }

        // GET: FilePaths/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilePath filePath = db.FilePaths.Find(id);
            if (filePath == null)
            {
                return HttpNotFound();
            }
            return View(filePath);
        }

        // POST: FilePaths/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            FilePath filePath = db.FilePaths.Find(id);
            db.FilePaths.Remove(filePath);
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
