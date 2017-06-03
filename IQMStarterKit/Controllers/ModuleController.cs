using IQMStarterKit.Models;
using IQMStarterKit.Models.Alert;
using IQMStarterKit.Models.Core;
using IQMStarterKit.Models.Forms;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Web.Mvc;

namespace IQMStarterKit.Controllers
{
    [Authorize]
    public class ModuleController : CommonController
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        // GET: Module
        public ActionResult Index()
        {
            return RedirectToAction("ViewStudentActivities", "Module");
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

        public ActionResult Page2()
        {
            return View();
        }
        public ActionResult Page3()
        {
            return View();
        }
        public ActionResult Page4()
        {
            return View();
        }
        public ActionResult Page5()
        {
            return View();
        }
        public ActionResult Page6()
        {
            return View();
        }
        public ActionResult Page7()
        {
            return View();
        }
        public ActionResult Page8()
        {
            return View();
        }
        public ActionResult Page9()
        {
            return View();
        }
        public ActionResult Page10()
        {
            return View();
        }
        public ActionResult Page11()
        {
            return View();
        }
        public ActionResult Page12()
        {
            return View();
        }

        public ActionResult Page13()
        {
            Session["Page13Completed"] = true;
            return View();
        }


        public ActionResult Page14()
        {
            if (User.IsInRole("Tutor") || User.IsInRole("Administrator")) { }
            else
            {
                if (Session["Page13Completed"] != null)
                {
                    try
                    {
                        //save introduction as completed
                        //validate if record already existed
                        var tempAct = _context.TempActivities.FirstOrDefault(m => m.Title == "Introduction");

                        var owner = GetSessionUserId();
                        var stdAct =
                            _context.StudentActivities.Where(m => m.TempActivityId == tempAct.TempActivityId &&
                                                                  m.CreatedBy == owner);
                        //go directly to view
                        if (stdAct.Any()) return View();

                        var studentActivity = new StudentActivity();
                        studentActivity.CreatedDateTime = DateTime.Now;
                        studentActivity.CreatedBy = GetSessionUserId();

                        studentActivity.ProgressValue = 100;
                        studentActivity.ModifiedBy = GetSessionUserId();
                        studentActivity.ModifiedDateTime = DateTime.Now;

                        if (tempAct != null)
                        {
                            studentActivity.TempActivityId = tempAct.TempActivityId;
                            studentActivity.TempModuleId = tempAct.TempModuleId;
                        }

                        studentActivity.Context = string.Empty;
                        studentActivity.CFDominantFirst = string.Empty;
                        studentActivity.CFDominantSecond = string.Empty;
                        studentActivity.VarkResult = string.Empty;
                        studentActivity.DopeResult = string.Empty;
                        studentActivity.DiscResult = string.Empty;
                        studentActivity.Top3PersonalValues = string.Empty;

                        Session["Page13Completed"] = null;
                        // exclude fields not to be saved

                        _context.StudentActivities.Add(studentActivity);
                        _context.SaveChanges();

                        return View().WithInfo("Congratulation! You have completed the introduction activity!");
                    }
                    catch (EntityException e)
                    {
                        throw new Exception(e.Message);
                    }

                }
            }

            return View();
        }






        public ActionResult Page15()
        {
            var aboutme = new AboutMeClass();

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("About Me");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                //get context and deserialize
                aboutme = JsonConvert.DeserializeObject<AboutMeClass>(rec.Context);
                aboutme.StudentActivity = rec;
            }
            return View(aboutme);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page15(FormCollection fc)
        {
            var aboutMe = new AboutMeClass();
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("About Me");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);

            try
            {
                //generate context string from form collection

                aboutMe.Name = fc.Get("name").ToString();
                aboutMe.Place = fc.Get("place").ToString();
                aboutMe.Hobby = fc.Get("hobby").ToString();
                aboutMe.Movie = fc.Get("movie").ToString();
                aboutMe.Important = fc.Get("important").ToString();
                aboutMe.Model = fc.Get("model").ToString();
                aboutMe.Happy = fc.Get("happy").ToString();
                aboutMe.Dislike = fc.Get("dislike").ToString();
                aboutMe.Famous = fc.Get("famous").ToString();

                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(aboutMe);

                //validate if record already existed
                var studentRecord = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId);

                if (studentRecord == null)
                {
                    //insert record

                    var newRecord = new StudentActivity();
                    newRecord.TempActivityId = activityId;
                    newRecord.TempModuleId = moduleId;
                    newRecord.Context = context;
                    newRecord.ProgressValue = 100;

                    //system fields
                    newRecord.CreatedBy = GetSessionUserId();
                    newRecord.CreatedDateTime = DateTime.Now;
                    newRecord.ModifiedBy = GetSessionUserId();
                    newRecord.ModifiedDateTime = DateTime.Now;

                    _context.StudentActivities.Add(newRecord);
                }
                else
                {
                    studentRecord.Context = context;
                    studentRecord.ProgressValue = 100;
                    studentRecord.ModifiedBy = GetSessionUserId();
                    studentRecord.ModifiedDateTime = DateTime.Now;
                    //modify record
                    _context.Entry(studentRecord).State = EntityState.Modified;

                    //save record
                    _context.SaveChanges();



                }

                return View(aboutMe).WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {
                return View(aboutMe).WithError(ex.Message);
            }


        }

        public ActionResult Page16()
        {

            return View();
        }

        public ActionResult Page17()
        {

            return View();
        }


        public ActionResult Page18()
        {
            return View();
        }
        public ActionResult Page19()
        {
            return View();
        }
        public ActionResult Page20()
        {
            return View();
        }
        public ActionResult Page21()
        {
            return View();
        }
        public ActionResult Page22()
        {
            return View();
        }
        public ActionResult Page23()
        {
            return View();
        }
        public ActionResult Page24()
        {
            return View();
        }
        public ActionResult Page25()
        {
            return View();
        }
        public ActionResult Page26()
        {
            return View();
        }
        public ActionResult Page27()
        {
            return View();
        }
        public ActionResult Page28()
        {
            return View();
        }
        public ActionResult Page29()
        {
            return View();
        }
        public ActionResult Page30()
        {
            return View();
        }
        public ActionResult Page31()
        {
            return View();
        }
        public ActionResult Page32()
        {
            return View();
        }
        public ActionResult Page33()
        {
            return View();
        }
        public ActionResult Page34()
        {
            return View();
        }
        public ActionResult Page35()
        {
            return View();
        }
        public ActionResult Page36()
        {
            return View();
        }
        public ActionResult Page37()
        {
            return View();
        }
        public ActionResult Page38()
        {
            return View();
        }
        public ActionResult Page39()
        {
            return View();
        }
        public ActionResult Page40()
        {
            return View();
        }
        public ActionResult Page41()
        {
            return View();
        }
        public ActionResult Page42()
        {
            return View();
        }
        public ActionResult Page43()
        {
            return View();
        }
        public ActionResult Page44()
        {
            return View();
        }
        public ActionResult Page45()
        {
            return View();
        }
        public ActionResult Page46()
        {
            return View();
        }
        public ActionResult Page47()
        {
            return View();
        }
        public ActionResult Page48()
        {
            return View();
        }
        public ActionResult Page49()
        {
            return View();
        }
        public ActionResult Page50()
        {
            return View();
        }
        public ActionResult Page51()
        {
            return View();
        }
        public ActionResult Page52()
        {
            return View();
        }
        public ActionResult Page53()
        {
            return View();
        }
        public ActionResult Page54()
        {
            return View();
        }



        //Utility

        private byte GetTempActivityID(string v)
        {
            return _context.TempActivities.FirstOrDefault(m => m.Title == v).TempActivityId;
        }

        private byte GetTempModuleIdByActivityID(byte Id)
        {
            return _context.TempActivities.FirstOrDefault(m => m.TempActivityId == Id).TempModuleId;
        }
    }
}