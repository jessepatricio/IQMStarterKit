using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IQMStarterKit.Models;
using IQMStarterKit.Models.Core;
using Microsoft.AspNet.Identity;

namespace IQMStarterKit.Controllers
{
    public class ModuleController : CommonController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        // GET: Module
        public ActionResult Index()
        {
            //var toc = new TOCViewModels
            //{
            //    TempWorkbook =
            //        _context.TempWorkbooks.FirstOrDefault(m => m.IsRemoved == false && m.TempWorkbookId == 4),
            //    TempModules = _context.TempModules.ToList()
            //};


            ////load all activities to each module
            //foreach (var item in toc.TempModules)
            //{
            //    //should get student activities here
            //    var actView = new List<TempActivity>();
            //    var tmpActs = _context.TempActivities.Where(m => m.TempModuleId == item.TempModuleId).ToList();
            //    foreach (var row in tmpActs)
            //    {
            //        var stdAct = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == row.TempActivityId);

            //        var act = new TempActivity()
            //        {
            //           TempActivityId = row.TempActivityId,
            //           TempModuleId = row.TempModuleId,
            //           Title = row.Title,
            //           Description = row.Description,
            //           PageName = row.PageName,
            //           SortOrder = row.SortOrder,
            //           ProgressValue = stdAct?.ProgressValue ?? 0,
            //           Context = stdAct?.Context ?? string.Empty

            //        };

            //        actView.Add(act);
            //    }

            //    item.TempActivities =  actView;

            //}

            //var student = UserManager.FindByEmail(User.Identity.Name);
            //ViewBag.StudentName = student.FullName;

            return RedirectToAction("ViewStudentActivities", "Module");
            //return View(toc);
        }

        public ActionResult ViewStudentActivities(string email)
        {
            var toc = new TOCViewModels
            {
                TempWorkbook =
                    _context.TempWorkbooks.FirstOrDefault(m => m.IsRemoved == false && m.TempWorkbookId == 4),
                TempModules = _context.TempModules.ToList()
            };

            if (string.IsNullOrEmpty(email)) email = User.Identity.Name;
                
            var student = UserManager.FindByEmail(email);
            ViewBag.StudentName = student.FullName;

            //load all activities to each module
            foreach (var item in toc.TempModules)
            {
                //should get student activities here
                var actView = new List<TempActivity>();
                var tmpActs = _context.TempActivities.Where(m => m.TempModuleId == item.TempModuleId).ToList();
                foreach (var row in tmpActs)
                {
                    var stdAct = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == row.TempActivityId && m.CreatedBy == student.Id);

                    var act = new TempActivity()
                    {
                        TempActivityId = row.TempActivityId,
                        TempModuleId = row.TempModuleId,
                        Title = row.Title,
                        Description = row.Description,
                        PageName = row.PageName,
                        SortOrder = row.SortOrder,
                        ProgressValue = stdAct?.ProgressValue ?? 0,
                        Context = stdAct?.Context ?? string.Empty

                    };

                    actView.Add(act);
                }
                
                item.TempActivities = actView;

            }

            return View("Index", toc);
        }

        public ActionResult Page1()
        {
            return View();
        }
    }
}