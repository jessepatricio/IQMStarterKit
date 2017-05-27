﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IQMStarterKit.Models;
using IQMStarterKit.Models.Core;

namespace IQMStarterKit.Controllers.Core
{
    [Authorize(Roles = "Administrator")]
    public class TempActivitiesController : CommonController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        // GET: TempActivities
        public ActionResult Index()
        {
            return View(_context.TempActivities.ToList());
        }

        // GET: TempActivities/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempActivity tempActivity = _context.TempActivities.Find(id);
            if (tempActivity == null)
            {
                return HttpNotFound();
            }

            //get user fullname
            tempActivity.CreatedBy = GetFullName(tempActivity.CreatedBy);
            tempActivity.ModifiedBy = GetFullName(tempActivity.ModifiedBy);

            return View(tempActivity);
        }

        // GET: TempActivities/Create
        public ActionResult Create()
        {
            var tempActivity = new TempActivityViewModels
            {
                TempModules = _context.TempModules.Where(m => m.IsRemoved == false).ToList()
            };

            return View(tempActivity);
        }

        // POST: TempActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TempActivityId,Title,Description,PageName,SortOrder,TempModuleId,CreatedDateTime,CreatedBy,ModifiedDateTime,ModifiedBy,IsRemoved")] TempActivityViewModels tempActivity)
        {
            
            if (ModelState.IsValid)
            {
                var modActivity = new TempActivity();
                modActivity.Title = tempActivity.Title;
                modActivity.Description = tempActivity.Description;
                modActivity.PageName = tempActivity.PageName;
                modActivity.TempModuleId = tempActivity.TempModuleId;
                modActivity.SortOrder = tempActivity.SortOrder;

                //assign system fields
                modActivity.CreatedDateTime = DateTime.Now;
                modActivity.CreatedBy = GetSessionUserId();

                modActivity.ModifiedDateTime = DateTime.Now;
                modActivity.ModifiedBy = GetSessionUserId();

                _context.TempActivities.Add(modActivity);
                _context.SaveChanges();
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
            TempActivity tempActivity = _context.TempActivities.Find(id);
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
        public ActionResult Edit([Bind(Include = "TempActivityId,Title,Description,PageName,SortOrder,TempModuleId,CreatedDateTime,CreatedBy,ModifiedDateTime,ModifiedBy,IsRemoved")] TempActivity tempActivity)
        {
            var recActivity = _context.TempActivities.Find(tempActivity.TempActivityId);
            
            if (ModelState.IsValid)
            {
                if (recActivity != null)
                {
                    recActivity.Title = tempActivity.Title;
                    recActivity.Description = tempActivity.Description;
                    recActivity.PageName = tempActivity.PageName;
                    recActivity.SortOrder = tempActivity.SortOrder;
                    recActivity.TempModuleId = tempActivity.TempModuleId;
                    recActivity.CreatedDateTime = tempActivity.CreatedDateTime;
                    recActivity.CreatedBy = GetSessionUserId();
                    //update date stamp
                    recActivity.ModifiedDateTime = DateTime.Now;
                    recActivity.ModifiedBy = GetSessionUserId();

                    _context.Entry(recActivity).State = EntityState.Modified;
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(recActivity);
        }

        // GET: TempActivities/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempActivity tempActivity = _context.TempActivities.Find(id);
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
            TempActivity tempActivity = _context.TempActivities.Find(id);
            _context.TempActivities.Remove(tempActivity);
            _context.SaveChanges();
            return RedirectToAction("Index");
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
