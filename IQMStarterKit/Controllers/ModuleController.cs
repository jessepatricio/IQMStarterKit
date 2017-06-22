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

        public ActionResult CourseDemo()
        {
            Session["email"] = null;
            return RedirectToAction("ViewStudentActivities", "Module");
        }

        public ActionResult ViewStudentActivities(string email)
        {
            var toc = new TOCViewModels
            {
                // note: should indicate temp workbook id number
                TempWorkbook =
                    _context.TempWorkbooks.FirstOrDefault(m => m.IsRemoved == false && m.TempWorkbookId == 4),
                TempModules = _context.TempModules.ToList()
            };

            //empty email && null session (student)
            if (string.IsNullOrEmpty(email) && Session["email"] == null)
            {
                email = User.Identity.Name;
                Session["email"] = null;
            }
            //not empty email && null session (admin/tutor)
            else if (!string.IsNullOrEmpty(email) && Session["email"] == null)
            {
                Session["email"] = email;
            }
            else if (Session["email"] != null)
            {
                email = Session["email"].ToString();
            }


            var student = UserManager.FindByEmail(email);
            toc.StudentName = student.FullName;

            if (User.IsInRole("Student")) toc.IsDemo = false;
            else toc.IsDemo = true;

            toc.OverallProgressValue = student.OverallProgress;

            //load all activities to each module
            foreach (var item in toc.TempModules)
            {
                //should get student activities here
                var actView = new List<TempActivity>();
                var tmpActs = _context.TempActivities.Where(m => m.TempModuleId == item.TempModuleId && m.IsRemoved == false).ToList();
                foreach (var row in tmpActs)
                {
                    var stdAct = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == row.TempActivityId && m.IsRemoved == false && m.CreatedBy == student.Id);

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

            //add the config
            toc.GroupActivityConfigs = _context.GroupActivityConfig.Where(m => m.GroupId == student.GroupId).ToList();

            return View("Index", toc);
        }

        public ActionResult Page1()
        {
            // title: Introduction
            // type: slide
            return View();
        }

        public ActionResult Page2()
        {
            // title: Programme Objectives
            // type: slide
            return View();
        }

        public ActionResult Page3()
        {
            // title: Chance favours the prepared mind
            // type: slide
            return View();
        }

        public ActionResult Page4()
        {
            // title: Is yours a 'prepared mind?'
            // type: slide
            return View();
        }

        public ActionResult Page5()
        {
            // title:  You will be experiencing many new things
            // type: slide
            return View();
        }

        public ActionResult Page6()
        {
            // title: Course Requirements
            // type: slide
            return View();
        }

        public ActionResult Page7()
        {
            // title: Attendance Issues
            // type: slide
            return View();
        }

        public ActionResult Page8()
        {
            // title: Student Services Contact Details
            // type: slide
            return View();
        }

        public ActionResult Page9()
        {
            // title: About the Programme
            // type: slide
            return View();
        }

        public ActionResult Page10()
        {
            // title: Topics
            // type: slide
            return View();
        }

        public ActionResult Page12()
        {
            // title: Effective Soft Skill
            // type: slide
            return View();
        }

        public ActionResult Page13()
        {
            // title: You may be interested
            // type: slide
            TempData["Page13Completed"] = true;

            return View();
        }

        public ActionResult Page14()
        {
            // title: Personal Coat of Arms
            // type: Submission

            #region this code is for page 13
            if (User.IsInRole("Tutor") || User.IsInRole("Administrator"))
            {
                //do nothing if not student

            }
            else
            {
                if (TempData["Page13Completed"] != null)
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
                        if (!stdAct.Any())
                        {

                            var studentActivity = new StudentActivity();
                            studentActivity.CreatedDateTime = DateTime.Now;
                            studentActivity.CreatedBy = GetSessionUserId();

                            studentActivity.ProgressValue = 100;
                            studentActivity.Type = ActivityCategory.Slide;
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

                            TempData["Page13Completed"] = null;
                            // exclude fields not to be saved

                            _context.StudentActivities.Add(studentActivity);
                            _context.SaveChanges();

                            // record overall progress
                            ComputeOverallProgress();


                            return View("Page14").WithInfo("Congratulation! You have completed the Introduction activity!");
                        }
                    }
                    catch (EntityException e)
                    {
                        throw new Exception(e.Message);
                    }

                }
            }
            #endregion



            #region start of Page14
            FilePath filePath = new FilePath();

            //get id in tempActivity
            var activityID = GetTempActivityID("My Shield");
            //get owner
            string owner2 = string.Empty;
            //check if viewing other user
            if (Session["email"] == null)
            {
                owner2 = GetSessionUserId();
            }
            else
            {
                var email = Session["email"].ToString();
                var sel_user = UserManager.FindByEmail(email);
                owner2 = sel_user.Id;
            }
            //validate in studentActivity
            var stdAct2 = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityID && m.CreatedBy == owner2);

            if (stdAct2 != null)
            {
                filePath = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == stdAct2.StudentActivityId);
            }

            return View(filePath);
            #endregion
        }

        public FileResult Page14DownloadFile(string Id)
        {
            var stdId = byte.Parse(Id);

            var fileToRetrieve = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == stdId);

            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page14UploadFile(FilePath filepath, UploadFileModel fileModel)
        {

            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page14").WithInfo("This is just a demo.");
            }

            var studentActivity = new StudentActivity();

            try
            {


                if (fileModel.File != null && fileModel.File.ContentLength > 0)
                {
                    // extract the file content to byte array
                    var content = new byte[fileModel.File.ContentLength];
                    // reads the content from stream
                    fileModel.File.InputStream.Read(content, 0, fileModel.File.ContentLength);

                    var newImage = new FilePath
                    {
                        FileName = System.IO.Path.GetFileName(fileModel.File.FileName),
                        FileType = FileType.Photo,
                        ContentType = fileModel.File.ContentType,
                        Content = content,
                        CreatedBy = GetSessionUserId(),
                        CreatedDateTime = DateTime.Now

                    };


                    //get temp activity id by title
                    //note: title should be the same with the search keyword when using lamda expression
                    byte activityId = GetTempActivityID("My Shield");
                    //get module id
                    var moduleId = GetTempModuleIdByActivityID(activityId);
                    //get current user
                    var owner = GetSessionUserId();
                    //check if record existed
                    StudentActivity oldRec =
                        _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId &&
                                                              m.CreatedBy == owner);
                    if (oldRec != null)
                    {
                        //update
                        oldRec.ModifiedBy = GetSessionUserId();
                        oldRec.ModifiedDateTime = DateTime.Now;
                        //update studentActivity table
                        _context.Entry(oldRec).State = EntityState.Modified;

                        //since there are studentactivity record there should be a file
                        var oldFile = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == oldRec.StudentActivityId);

                        //update new content
                        oldFile.Content = newImage.Content;
                        _context.Entry(oldFile).State = EntityState.Modified;
                        //save all changes
                        _context.SaveChanges();


                    }
                    else
                    {
                        //create
                        studentActivity.TempActivityId = activityId;
                        studentActivity.TempModuleId = moduleId;

                        studentActivity.CreatedDateTime = DateTime.Now;
                        studentActivity.CreatedBy = GetSessionUserId();

                        studentActivity.ProgressValue = 100;
                        studentActivity.Type = ActivityCategory.FileSubmission;
                        studentActivity.ModifiedBy = GetSessionUserId();
                        studentActivity.ModifiedDateTime = DateTime.Now;

                        studentActivity.Context = string.Empty;
                        studentActivity.CFDominantFirst = string.Empty;
                        studentActivity.CFDominantSecond = string.Empty;
                        studentActivity.VarkResult = string.Empty;
                        studentActivity.DopeResult = string.Empty;
                        studentActivity.DiscResult = string.Empty;
                        studentActivity.Top3PersonalValues = string.Empty;


                        _context.StudentActivities.Add(studentActivity);

                        _context.SaveChanges();

                        // get generated id for new activity
                        newImage.StudentActivityId = studentActivity.StudentActivityId;

                        _context.FilePaths.Add(newImage);

                        _context.SaveChanges();

                        // record overall progress
                        ComputeOverallProgress();


                    }

                }
                return RedirectToAction("Page14").WithSuccess("File uploaded successfully!");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Page14").WithError("File upload failed!" + ex.Message);
            }
        }

        public ActionResult Page15()
        {
            // title: About Me
            // type: FormSubmission

            var aboutMe = new AboutMeClass();
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
                aboutMe = JsonConvert.DeserializeObject<AboutMeClass>(rec.Context);
                aboutMe.StudentActivity = rec;
            }
            return View(aboutMe);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page15(FormCollection fc)
        {
            var aboutMe = new AboutMeClass();

            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page15").WithInfo("This is just a demo.");
            }

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("About Me");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();

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
                var studentRecord = _context.StudentActivities
                    .FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == owner);

                if (studentRecord == null)
                {
                    var newRecord = new StudentActivity();
                    newRecord.TempActivityId = activityId;
                    newRecord.TempModuleId = moduleId;
                    newRecord.Context = context;
                    newRecord.ProgressValue = 100;
                    newRecord.Type = ActivityCategory.FormSubmission;

                    //system fields
                    newRecord.CreatedBy = GetSessionUserId();
                    newRecord.CreatedDateTime = DateTime.Now;
                    newRecord.ModifiedBy = GetSessionUserId();
                    newRecord.ModifiedDateTime = DateTime.Now;

                    //insert record
                    _context.StudentActivities.Add(newRecord);

                    // record overall progress
                    ComputeOverallProgress();
                }
                else
                {
                    studentRecord.Context = context;
                    studentRecord.ModifiedBy = GetSessionUserId();
                    studentRecord.ModifiedDateTime = DateTime.Now;

                    // modify record
                    _context.Entry(studentRecord).State = EntityState.Modified;

                }

                // save record
                _context.SaveChanges();

                return RedirectToAction("Page15").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {
                return View(aboutMe).WithError(ex.Message);
            }


        }

        public ActionResult Page16()
        {
            // title: Tutor-Student Contract
            // type: Slide
            return View();
        }

        public ActionResult Page17()
        {
            // title: Classroom Rules
            // type: slide
            return View();
        }

        public ActionResult Page18()
        {
            // title: Classroom Rules
            // type: slide
            return View();
        }

        public ActionResult Page18b()
        {
            // title: Classroom Rules
            // type: slide
            return View();
        }

        public ActionResult Page19()
        {
            // title: Classroom Rules
            // type: slide
            return View();
        }

        public ActionResult Page20()
        {
            // title: Electronics Etiquette
            // type: slide
            return View();
        }

        public ActionResult Page21()
        {
            // title: Classroom Rules
            // type: slide
            return View();
        }

        public ActionResult Page22()
        {
            // title: Classroom Rules
            // type: slide
            TempData["Page22Completed"] = true;
            return View();
        }

        public ActionResult Page23()
        {
            // title: VARK
            // type: result
            var vark = new List<Vark>();
            ViewBag.ActDone = false;

            #region for Page22
            if (User.IsInRole("Tutor") || User.IsInRole("Administrator")) { }
            else
            {
                if (TempData["Page22Completed"] != null)
                {
                    try
                    {
                        //save class contract as completed
                        //validate if record already existed
                        var tempAct = _context.TempActivities.FirstOrDefault(m => m.Title == "Class Contract");

                        var owner = GetSessionUserId();
                        // check if record already existed
                        var stdAct =
                            _context.StudentActivities.Where(m => m.TempActivityId == tempAct.TempActivityId &&
                                                                  m.CreatedBy == owner);
                        //go directly to view
                        if (!stdAct.Any())
                        {

                            var studentActivity = new StudentActivity();
                            studentActivity.CreatedDateTime = DateTime.Now;
                            studentActivity.CreatedBy = GetSessionUserId();

                            studentActivity.ProgressValue = 100;
                            studentActivity.Type = ActivityCategory.Slide;
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

                            TempData["Page22Completed"] = null;
                            // exclude fields not to be saved

                            _context.StudentActivities.Add(studentActivity);
                            _context.SaveChanges();

                            // record overall progress
                            ComputeOverallProgress();

                            return RedirectToAction("Page23").WithInfo("Congratulation! You have completed the Class Contract activity!");
                        }
                    }
                    catch (EntityException e)
                    {
                        throw new Exception(e.Message);
                    }

                }
            }
            //////////// for page 22 code //////////////////////////
            #endregion


            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("VARK");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                //get context and deserialize
                //vark = JsonConvert.DeserializeObject<List<Vark>>(rec.Context);

                ViewBag.ActDone = true;
                ViewBag.Vark = rec.VarkResult;

            }

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page23(FormCollection fc)
        {

            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page23").WithInfo("This is just a demo.");
            }
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("VARK");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();


            try
            {
                // get vark list from collection
                var varkList = GetVarkList(fc);
                // get string result from list
                var yourVark = GetVarkResult(varkList);

                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(varkList);

                //validate if record already existed
                var studentRecord = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == owner);


                if (studentRecord == null)
                {
                    //insert record

                    var newRecord = new StudentActivity();
                    newRecord.TempActivityId = activityId;
                    newRecord.TempModuleId = moduleId;
                    newRecord.Context = context;
                    newRecord.VarkResult = yourVark;
                    newRecord.ProgressValue = 100;
                    newRecord.Type = ActivityCategory.Result;

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
                    studentRecord.VarkResult = yourVark;
                    studentRecord.ModifiedBy = GetSessionUserId();
                    studentRecord.ModifiedDateTime = DateTime.Now;
                    //modify record
                    _context.Entry(studentRecord).State = EntityState.Modified;



                }

                //save record
                _context.SaveChanges();

                // record overall progress
                ComputeOverallProgress();

                return RedirectToAction("Page23").WithSuccess("Saved successfully! " + $"You are {yourVark}");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Page23").WithError(ex.Message);
            }

        }


        public ActionResult Page24()
        {

            // title: Human Bingo - Activity
            // type: FileSubmission

            FilePath filePath = new FilePath();

            //get id in tempActivity
            var activityID = GetTempActivityID("Human Bingo - Activity");
            //get owner
            var owner = GetSessionUserId();
            //validate in studentActivity
            var stdAct2 = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityID && m.CreatedBy == owner);


            if (stdAct2 != null)
            {
                filePath = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == stdAct2.StudentActivityId);
            }

            return View(filePath);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page24UploadFile(FilePath filepath, UploadFileModel fileModel)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page24").WithInfo("This is just a demo.");
            }
            var studentActivity = new StudentActivity();
            try
            {


                if (fileModel.File != null && fileModel.File.ContentLength > 0)
                {
                    // extract the file content to byte array
                    var content = new byte[fileModel.File.ContentLength];
                    // reads the content from stream
                    fileModel.File.InputStream.Read(content, 0, fileModel.File.ContentLength);

                    var newImage = new FilePath
                    {
                        FileName = System.IO.Path.GetFileName(fileModel.File.FileName),
                        FileType = FileType.Photo,
                        ContentType = fileModel.File.ContentType,
                        Content = content,
                        CreatedBy = GetSessionUserId(),
                        CreatedDateTime = DateTime.Now

                    };



                    //get temp activity id by title
                    //note: title should be the same with the search keyword when using lamda expression
                    byte activityId = GetTempActivityID("Human Bingo - Activity");
                    //get module id
                    var moduleId = GetTempModuleIdByActivityID(activityId);
                    //get current user
                    var owner = GetSessionUserId();
                    //check if record existed
                    StudentActivity oldRec =
                        _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId &&
                                                              m.CreatedBy == owner);
                    if (oldRec != null)
                    {
                        //update
                        oldRec.ModifiedBy = GetSessionUserId();
                        oldRec.ModifiedDateTime = DateTime.Now;
                        //update studentActivity table
                        _context.Entry(oldRec).State = EntityState.Modified;

                        //since there are studentactivity record there should be a file
                        var oldFile = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == oldRec.StudentActivityId);

                        //update new content
                        oldFile.Content = newImage.Content;
                        _context.Entry(oldFile).State = EntityState.Modified;
                        //save all changes
                        _context.SaveChanges();


                    }
                    else
                    {
                        //create
                        studentActivity.TempActivityId = activityId;
                        studentActivity.TempModuleId = moduleId;

                        studentActivity.CreatedDateTime = DateTime.Now;
                        studentActivity.CreatedBy = GetSessionUserId();

                        studentActivity.ProgressValue = 100;
                        studentActivity.Type = ActivityCategory.FileSubmission;
                        studentActivity.ModifiedBy = GetSessionUserId();
                        studentActivity.ModifiedDateTime = DateTime.Now;

                        studentActivity.Context = string.Empty;
                        studentActivity.CFDominantFirst = string.Empty;
                        studentActivity.CFDominantSecond = string.Empty;
                        studentActivity.VarkResult = string.Empty;
                        studentActivity.DopeResult = string.Empty;
                        studentActivity.DiscResult = string.Empty;
                        studentActivity.Top3PersonalValues = string.Empty;


                        _context.StudentActivities.Add(studentActivity);

                        _context.SaveChanges();

                        //studentActivityId should have unique id after saving
                        newImage.StudentActivityId = studentActivity.StudentActivityId;

                        _context.FilePaths.Add(newImage);

                        _context.SaveChanges();

                        // record overall progress
                        ComputeOverallProgress();


                    }

                }
                return RedirectToAction("Page24").WithSuccess("File uploaded successfully!");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Page24").WithError("File upload failed!" + ex.Message);
            }
        }

        public FileResult Page24DownloadFile(string Id)
        {
            var stdId = byte.Parse(Id);

            var fileToRetrieve = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == stdId);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }

        public ActionResult Page25()
        {
            // title: Photo Scavenger Hunt
            // type: FileSubmission

            FilePath filePath = new FilePath();

            //get id in tempActivity
            var activityID = GetTempActivityID("Photo Scavenger Hunt");
            //get owner
            var owner = GetSessionUserId();
            //validate in studentActivity
            var stdAct2 = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityID && m.CreatedBy == owner);

            if (stdAct2 != null)
            {
                filePath = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == stdAct2.StudentActivityId);
            }

            return View(filePath);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page25UploadFile(FilePath filepath, UploadFileModel fileModel)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page25").WithInfo("This is just a demo.");
            }
            var studentActivity = new StudentActivity();
            try
            {


                if (fileModel.File != null && fileModel.File.ContentLength > 0)
                {
                    // extract the file content to byte array
                    var content = new byte[fileModel.File.ContentLength];
                    // reads the content from stream
                    fileModel.File.InputStream.Read(content, 0, fileModel.File.ContentLength);

                    var newImage = new FilePath
                    {
                        FileName = System.IO.Path.GetFileName(fileModel.File.FileName),
                        FileType = FileType.Photo,
                        ContentType = fileModel.File.ContentType,
                        Content = content,
                        CreatedBy = GetSessionUserId(),
                        CreatedDateTime = DateTime.Now

                    };



                    //get temp activity id by title
                    //note: title should be the same with the search keyword when using lamda expression
                    byte activityId = GetTempActivityID("Photo Scavenger Hunt");
                    //get module id
                    var moduleId = GetTempModuleIdByActivityID(activityId);
                    //get current user
                    var owner = GetSessionUserId();
                    //check if record existed
                    StudentActivity oldRec =
                        _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId &&
                                                              m.CreatedBy == owner);
                    if (oldRec != null)
                    {
                        //update
                        oldRec.ModifiedBy = GetSessionUserId();
                        oldRec.ModifiedDateTime = DateTime.Now;
                        //update studentActivity table
                        _context.Entry(oldRec).State = EntityState.Modified;

                        //since there are studentactivity record there should be a file
                        var oldFile = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == oldRec.StudentActivityId);

                        //update new content
                        oldFile.Content = newImage.Content;
                        _context.Entry(oldFile).State = EntityState.Modified;
                        //save all changes
                        _context.SaveChanges();


                    }
                    else
                    {
                        //create
                        studentActivity.TempActivityId = activityId;
                        studentActivity.TempModuleId = moduleId;

                        studentActivity.CreatedDateTime = DateTime.Now;
                        studentActivity.CreatedBy = GetSessionUserId();

                        studentActivity.ProgressValue = 100;
                        studentActivity.Type = ActivityCategory.FileSubmission;
                        studentActivity.ModifiedBy = GetSessionUserId();
                        studentActivity.ModifiedDateTime = DateTime.Now;

                        studentActivity.Context = string.Empty;
                        studentActivity.CFDominantFirst = string.Empty;
                        studentActivity.CFDominantSecond = string.Empty;
                        studentActivity.VarkResult = string.Empty;
                        studentActivity.DopeResult = string.Empty;
                        studentActivity.DiscResult = string.Empty;
                        studentActivity.Top3PersonalValues = string.Empty;


                        _context.StudentActivities.Add(studentActivity);

                        _context.SaveChanges();

                        //studentActivityId should have unique id after saving
                        newImage.StudentActivityId = studentActivity.StudentActivityId;

                        _context.FilePaths.Add(newImage);

                        _context.SaveChanges();

                        // record overall progress
                        ComputeOverallProgress();


                    }

                }
                return RedirectToAction("Page25").WithSuccess("File uploaded successfully!");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Page25").WithError("File upload failed!" + ex.Message);
            }
        }

        public FileResult Page25DownloadFile(string Id)
        {
            var stdId = byte.Parse(Id);

            var fileToRetrieve = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == stdId);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }


        public ActionResult Page26()
        {

            // title: Personal Profiling - DOPE
            // type: Result

            ViewBag.ActDone = false;

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Personal Profiling - D.O.P.E.");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                //get context and deserialize

                ViewBag.ActDone = true;
                ViewBag.Dope = rec.DopeResult;

            }



            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page26(FormCollection fc)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page26").WithInfo("This is just a demo.");
            }
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Personal Profiling - D.O.P.E.");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();

            string dopeResult = GetDopeResult(fc);

            if (string.IsNullOrEmpty(dopeResult))
            {
                return RedirectToAction("Page26").WithError("No result was given. Please make sure you have selected any checkboxes.");
            }

            var studentActivity = new StudentActivity();

            try
            {

                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(fc.Get("DOPE"));

                //insert record

                var newRecord = new StudentActivity();
                newRecord.TempActivityId = activityId;
                newRecord.TempModuleId = moduleId;
                newRecord.Context = context;
                newRecord.DopeResult = dopeResult;
                newRecord.ProgressValue = 100;
                newRecord.Type = ActivityCategory.Result;

                //system fields
                newRecord.CreatedBy = GetSessionUserId();
                newRecord.CreatedDateTime = DateTime.Now;
                newRecord.ModifiedBy = GetSessionUserId();
                newRecord.ModifiedDateTime = DateTime.Now;

                _context.StudentActivities.Add(newRecord);

                //save record
                _context.SaveChanges();

                // record overall progress
                ComputeOverallProgress();

                return RedirectToAction("Page26").WithSuccess("Saved successfully! " + $"You are {dopeResult}");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Page26").WithError(ex.Message);
            }

        }

        public ActionResult Page27()
        {
            // title: DISC
            // type: Result

            ViewBag.ActDone = false;

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Personal Profiling - D.I.S.C.");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                //get context and deserialize
                ViewBag.ActDone = true;
                ViewBag.Disc = rec.DiscResult;

            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page27(FormCollection fc)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page27").WithInfo("This is just a demo.");
            }
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Personal Profiling - D.I.S.C.");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();

            string discResult = GetDiscResult(fc);

            if (string.IsNullOrEmpty(discResult))
            {
                return RedirectToAction("Page27").WithError("No result was given. Please make sure you have chosen any options.");
            }

            var studentActivity = new StudentActivity();

            try
            {

                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(fc.Get("DOPE"));

                //insert record

                var newRecord = new StudentActivity();
                newRecord.TempActivityId = activityId;
                newRecord.TempModuleId = moduleId;
                newRecord.Context = context;
                newRecord.DiscResult = discResult;
                newRecord.ProgressValue = 100;
                newRecord.Type = ActivityCategory.Result;

                //system fields
                newRecord.CreatedBy = GetSessionUserId();
                newRecord.CreatedDateTime = DateTime.Now;
                newRecord.ModifiedBy = GetSessionUserId();
                newRecord.ModifiedDateTime = DateTime.Now;

                _context.StudentActivities.Add(newRecord);

                //save record
                _context.SaveChanges();

                // record overall progress
                ComputeOverallProgress();

                return RedirectToAction("Page27").WithSuccess("Saved successfully! " + $"You are {discResult}");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Page27").WithError(ex.Message);
            }


        }

        public ActionResult Page28()
        {
            // title: Kiwiana
            // type: FormSubmission

            var kiwiana = new KiwianaClass();

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Kiwiana - Activity");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                //get context and deserialize
                kiwiana = JsonConvert.DeserializeObject<KiwianaClass>(rec.Context);
                kiwiana.StudentActivity = rec;
            }
            return View(kiwiana);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page28(FormCollection fc)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page28").WithInfo("This is just a demo.");
            }
            var kiwiana = new KiwianaClass();
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Kiwiana - Activity");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();

            try
            {
                //generate context string from form collection

                kiwiana.Paua = fc.Get("Paua").ToString();
                kiwiana.Pavlova = fc.Get("Pavlova").ToString();
                kiwiana.HokeyPokey = fc.Get("HokeyPokey").ToString();
                kiwiana.FishNChips = fc.Get("FishNChips").ToString();
                kiwiana.Jandals = fc.Get("Jandals").ToString();
                kiwiana.Swandri = fc.Get("Swandri").ToString();
                kiwiana.BuzzyBee = fc.Get("BuzzyBee").ToString();
                kiwiana.BlackSinglet = fc.Get("BlackSinglet").ToString();
                kiwiana.Rugby = fc.Get("Rugby").ToString();
                kiwiana.KiwiFruit = fc.Get("KiwiFruit").ToString();
                kiwiana.PineappleLumps = fc.Get("PineappleLumps").ToString();
                kiwiana.LnP = fc.Get("LnP").ToString();

                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(kiwiana);

                //validate if record already existed
                var studentRecord = _context.StudentActivities
                    .FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == owner);

                if (studentRecord == null)
                {
                    var newRecord = new StudentActivity();
                    newRecord.TempActivityId = activityId;
                    newRecord.TempModuleId = moduleId;
                    newRecord.Context = context;
                    newRecord.ProgressValue = 100;
                    newRecord.Type = ActivityCategory.FormSubmission;

                    //system fields
                    newRecord.CreatedBy = GetSessionUserId();
                    newRecord.CreatedDateTime = DateTime.Now;
                    newRecord.ModifiedBy = GetSessionUserId();
                    newRecord.ModifiedDateTime = DateTime.Now;

                    //insert record
                    _context.StudentActivities.Add(newRecord);
                }
                else
                {
                    studentRecord.Context = context;
                    studentRecord.ModifiedBy = GetSessionUserId();
                    studentRecord.ModifiedDateTime = DateTime.Now;

                    // modify record
                    _context.Entry(studentRecord).State = EntityState.Modified;

                }

                // save record
                _context.SaveChanges();

                // record overall progress
                ComputeOverallProgress();

                return RedirectToAction("Page28").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {
                return View(kiwiana).WithError(ex.Message);
            }

        }

        public ActionResult Page29()
        {


            // title: NZSlang
            // type: FormSubmission

            var objSlang = new SlangClass();

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("NZ Slang and Saying");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                //get context and deserialize
                objSlang = JsonConvert.DeserializeObject<SlangClass>(rec.Context);
                objSlang.StudentActivity = rec;
            }


            return View(objSlang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page29(SlangClass slang)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page29").WithInfo("This is just a demo.");
            }
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("NZ Slang and Saying");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();

            try
            {

                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(slang);

                //validate if record already existed
                var studentRecord = _context.StudentActivities
                    .FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == owner);

                if (studentRecord == null)
                {
                    var newRecord = new StudentActivity();
                    newRecord.TempActivityId = activityId;
                    newRecord.TempModuleId = moduleId;
                    newRecord.Context = context;
                    newRecord.ProgressValue = 100;
                    newRecord.NoMatchedWords = GetMatchedWords(slang);
                    newRecord.Type = ActivityCategory.FormSubmission;

                    //system fields
                    newRecord.CreatedBy = GetSessionUserId();
                    newRecord.CreatedDateTime = DateTime.Now;
                    newRecord.ModifiedBy = GetSessionUserId();
                    newRecord.ModifiedDateTime = DateTime.Now;

                    //insert record
                    _context.StudentActivities.Add(newRecord);

                    //update overall progress
                    ComputeOverallProgress();


                }
                else
                {
                    studentRecord.Context = context;
                    studentRecord.ModifiedBy = GetSessionUserId();
                    studentRecord.ModifiedDateTime = DateTime.Now;

                    // modify record
                    _context.Entry(studentRecord).State = EntityState.Modified;

                }

                // save record
                _context.SaveChanges();

                return RedirectToAction("Page29").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {
                return View(slang).WithError(ex.Message);
            }
        }


        public ActionResult Page30()
        {
            // title: Cheese
            // type: FormSubmission

            var cheese = new cheese();
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Cheese");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                //get context and deserialize
                cheese = JsonConvert.DeserializeObject<cheese>(rec.Context);
                cheese.StudentActivity = rec;
            }

            return View(cheese);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page30(FormCollection fc)
        {
            var cheese = new cheese();

            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page30").WithInfo("This is just a demo.");
            }

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("cheese");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();

            try
            {
                //generate context string from form collection

                cheese.answer1 = fc.Get("answer1").ToString();
                cheese.answer2 = fc.Get("answer2").ToString();
                cheese.answer3 = fc.Get("answer3").ToString();
                cheese.answer4 = fc.Get("answer4").ToString();
                cheese.answer5 = fc.Get("answer5").ToString();
                cheese.answer6 = fc.Get("answer6").ToString();
                cheese.answer7 = fc.Get("answer7").ToString();


                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(cheese);

                //validate if record already existed
                var studentRecord = _context.StudentActivities
                    .FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == owner);

                if (studentRecord == null)
                {
                    var newRecord = new StudentActivity();
                    newRecord.TempActivityId = activityId;
                    newRecord.TempModuleId = moduleId;
                    newRecord.Context = context;
                    newRecord.ProgressValue = 100;
                    newRecord.Type = ActivityCategory.FormSubmission;

                    //system fields
                    newRecord.CreatedBy = GetSessionUserId();
                    newRecord.CreatedDateTime = DateTime.Now;
                    newRecord.ModifiedBy = GetSessionUserId();
                    newRecord.ModifiedDateTime = DateTime.Now;

                    //insert record
                    _context.StudentActivities.Add(newRecord);

                    // record overall progress
                    ComputeOverallProgress();
                }
                else
                {
                    studentRecord.Context = context;
                    studentRecord.ModifiedBy = GetSessionUserId();
                    studentRecord.ModifiedDateTime = DateTime.Now;

                    // modify record
                    _context.Entry(studentRecord).State = EntityState.Modified;

                }

                // save record
                _context.SaveChanges();

                return RedirectToAction("Page30").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {
                return View(cheese).WithError(ex.Message);
            }


        }


        public ActionResult Page31()
        {
            return View();
        }

        public ActionResult Page32()
        {
            // title: think like ceo
            // type: FormSubmission

            var thinkceo = new thinkCeo();
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Think Like a CEO");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                //get context and deserialize
                thinkceo = JsonConvert.DeserializeObject<thinkCeo>(rec.Context);
                thinkceo.StudentActivity = rec;
            }

            return View(thinkceo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page32(FormCollection fc)
        {
            var thinkceo = new thinkCeo();

            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page32").WithInfo("This is just a demo.");
            }

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Think Like a CEO");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();

            try
            {
                //generate context string from form collection

                thinkceo.answer1 = fc.Get("answer1").ToString();
                thinkceo.answer2 = fc.Get("answer2").ToString();
                thinkceo.answer3 = fc.Get("answer3").ToString();
                thinkceo.answer4 = fc.Get("answer4").ToString();
                thinkceo.answer5 = fc.Get("answer5").ToString();
                thinkceo.answer6 = fc.Get("answer6").ToString();
                thinkceo.answer7 = fc.Get("answer7").ToString();
                thinkceo.answer8 = fc.Get("answer8").ToString();
                thinkceo.answer9 = fc.Get("answer9").ToString();
                thinkceo.answer10 = fc.Get("answer10").ToString();


                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(thinkceo);

                //validate if record already existed
                var studentRecord = _context.StudentActivities
                    .FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == owner);

                if (studentRecord == null)
                {
                    var newRecord = new StudentActivity();
                    newRecord.TempActivityId = activityId;
                    newRecord.TempModuleId = moduleId;
                    newRecord.Context = context;
                    newRecord.ProgressValue = 100;
                    newRecord.Type = ActivityCategory.FormSubmission;

                    //system fields
                    newRecord.CreatedBy = GetSessionUserId();
                    newRecord.CreatedDateTime = DateTime.Now;
                    newRecord.ModifiedBy = GetSessionUserId();
                    newRecord.ModifiedDateTime = DateTime.Now;

                    //insert record
                    _context.StudentActivities.Add(newRecord);

                    // record overall progress
                    ComputeOverallProgress();
                }
                else
                {
                    studentRecord.Context = context;
                    studentRecord.ModifiedBy = GetSessionUserId();
                    studentRecord.ModifiedDateTime = DateTime.Now;

                    // modify record
                    _context.Entry(studentRecord).State = EntityState.Modified;

                }

                // save record
                _context.SaveChanges();

                return RedirectToAction("Page32").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {
                return View(thinkceo).WithError(ex.Message);
            }


        }


        public ActionResult Page33()
        {
            // title: Personal Values
            // type: Result

            ViewBag.ActDone = false;

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Personal Values");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                //get context and deserialize
                ViewBag.ActDone = true;
                ViewBag.PersonalValues = rec.Top3PersonalValues;

            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page33(FormCollection fc)
        {



            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page23").WithInfo("This is just a demo.");
            }
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Personal Values");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();


            try
            {
                // get vark list from collection
                var personalValues = GetPersonalValues(fc);

                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(personalValues);


                //insert record

                var newRecord = new StudentActivity();
                newRecord.TempActivityId = activityId;
                newRecord.TempModuleId = moduleId;
                newRecord.Context = context;
                newRecord.Top3PersonalValues = personalValues;
                newRecord.ProgressValue = 100;
                newRecord.Type = ActivityCategory.Result;

                //system fields
                newRecord.CreatedBy = GetSessionUserId();
                newRecord.CreatedDateTime = DateTime.Now;
                newRecord.ModifiedBy = GetSessionUserId();
                newRecord.ModifiedDateTime = DateTime.Now;

                _context.StudentActivities.Add(newRecord);


                //save record
                _context.SaveChanges();

                // record overall progress
                ComputeOverallProgress();

                return RedirectToAction("Page33").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Page33").WithError(ex.Message);
            }



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
            // title: Winning Lottery
            // type: FormSubmission

            var winLot = new WinningLotteryClass();
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Goal Setting (Winning Lottery)");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                //get context and deserialize
                winLot = JsonConvert.DeserializeObject<WinningLotteryClass>(rec.Context);
                winLot.StudentActivity = rec;
            }
            return View(winLot);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page36(FormCollection fc)
        {
            var winLot = new WinningLotteryClass();

            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page36").WithInfo("This is just a demo.");
            }

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Goal Setting (Winning Lottery)");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();

            try
            {
                //generate context string from form collection

                winLot.WinLot1 = fc.Get("WinLot1").ToString();
                winLot.WinLot2 = fc.Get("WinLot2").ToString();
                winLot.WinLot3 = fc.Get("WinLot3").ToString();
                winLot.WinLot4 = fc.Get("WinLot4").ToString();
                winLot.WinLot5 = fc.Get("WinLot5").ToString();
                winLot.WinLot6 = fc.Get("WinLot6").ToString();
                winLot.WinLot7 = fc.Get("WinLot7").ToString();
                winLot.WinLot8 = fc.Get("WinLot8").ToString();
                winLot.WinLot9 = fc.Get("WinLot9").ToString();
                winLot.WinLot10 = fc.Get("WinLot10").ToString();

                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(winLot);

                //validate if record already existed
                var studentRecord = _context.StudentActivities
                    .FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == owner);

                if (studentRecord == null)
                {
                    var newRecord = new StudentActivity();
                    newRecord.TempActivityId = activityId;
                    newRecord.TempModuleId = moduleId;
                    newRecord.Context = context;
                    newRecord.ProgressValue = 100;
                    newRecord.Type = ActivityCategory.FormSubmission;

                    //system fields
                    newRecord.CreatedBy = GetSessionUserId();
                    newRecord.CreatedDateTime = DateTime.Now;
                    newRecord.ModifiedBy = GetSessionUserId();
                    newRecord.ModifiedDateTime = DateTime.Now;

                    //insert record
                    _context.StudentActivities.Add(newRecord);

                    // record overall progress
                    ComputeOverallProgress();
                }
                else
                {
                    studentRecord.Context = context;
                    studentRecord.ModifiedBy = GetSessionUserId();
                    studentRecord.ModifiedDateTime = DateTime.Now;

                    // modify record
                    _context.Entry(studentRecord).State = EntityState.Modified;

                }

                // save record
                _context.SaveChanges();

                return RedirectToAction("Page36").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {
                return View(winLot).WithError(ex.Message);
            }


        }



        public ActionResult Page37()
        {
            return View();
        }


        public ActionResult Page38()
        {

            // title: First Thing First
            // type: FormSubmission

            FilePath filePath = new FilePath();

            //get id in tempActivity
            var activityID = GetTempActivityID("First Thing First");
            //get owner
            var owner = GetSessionUserId();
            //validate in studentActivity
            var stdAct2 = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityID && m.CreatedBy == owner);

            if (stdAct2 != null)
            {
                filePath = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == stdAct2.StudentActivityId);
            }

            return View(filePath);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page38UploadFile(FilePath filepath, UploadFileModel fileModel)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page38").WithInfo("This is just a demo.");
            }
            var studentActivity = new StudentActivity();
            try
            {
                if (fileModel.File != null && fileModel.File.ContentLength > 0)
                {
                    // extract the file content to byte array
                    var content = new byte[fileModel.File.ContentLength];
                    // reads the content from stream
                    fileModel.File.InputStream.Read(content, 0, fileModel.File.ContentLength);

                    var newImage = new FilePath
                    {
                        FileName = System.IO.Path.GetFileName(fileModel.File.FileName),
                        FileType = FileType.Photo,
                        ContentType = fileModel.File.ContentType,
                        Content = content,
                        CreatedBy = GetSessionUserId(),
                        CreatedDateTime = DateTime.Now

                    };

                    //get temp activity id by title
                    //note: title should be the same with the search keyword when using lamda expression
                    byte activityId = GetTempActivityID("First Thing First");
                    //get module id
                    var moduleId = GetTempModuleIdByActivityID(activityId);
                    //get current user
                    var owner = GetSessionUserId();
                    //check if record existed
                    StudentActivity oldRec =
                        _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId &&
                                                              m.CreatedBy == owner);
                    if (oldRec != null)
                    {
                        //update
                        oldRec.ModifiedBy = GetSessionUserId();
                        oldRec.ModifiedDateTime = DateTime.Now;
                        //update studentActivity table
                        _context.Entry(oldRec).State = EntityState.Modified;

                        //since there are studentactivity record there should be a file
                        var oldFile = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == oldRec.StudentActivityId);

                        //update new content
                        oldFile.Content = newImage.Content;
                        _context.Entry(oldFile).State = EntityState.Modified;
                        //save all changes
                        _context.SaveChanges();


                    }
                    else
                    {
                        //create
                        studentActivity.TempActivityId = activityId;
                        studentActivity.TempModuleId = moduleId;

                        studentActivity.CreatedDateTime = DateTime.Now;
                        studentActivity.CreatedBy = GetSessionUserId();

                        studentActivity.ProgressValue = 100;
                        studentActivity.Type = ActivityCategory.FileSubmission;
                        studentActivity.ModifiedBy = GetSessionUserId();
                        studentActivity.ModifiedDateTime = DateTime.Now;

                        studentActivity.Context = string.Empty;
                        studentActivity.CFDominantFirst = string.Empty;
                        studentActivity.CFDominantSecond = string.Empty;
                        studentActivity.VarkResult = string.Empty;
                        studentActivity.DopeResult = string.Empty;
                        studentActivity.DiscResult = string.Empty;
                        studentActivity.Top3PersonalValues = string.Empty;


                        _context.StudentActivities.Add(studentActivity);

                        _context.SaveChanges();

                        //studentActivityId should have unique id after saving
                        newImage.StudentActivityId = studentActivity.StudentActivityId;

                        _context.FilePaths.Add(newImage);

                        _context.SaveChanges();

                        // record overall progress
                        ComputeOverallProgress();


                    }

                }
                return RedirectToAction("Page38").WithSuccess("File uploaded successfully!");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Page38").WithError("File upload failed!" + ex.Message);
            }
        }

        public FileResult Page38DownloadFile(string Id)
        {
            var stdId = byte.Parse(Id);

            var fileToRetrieve = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == stdId);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
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

            // title: Problem Solving - Dots
            // type: FileSubmission

            FilePath filePath = new FilePath();

            //get id in tempActivity
            var activityID = GetTempActivityID("Problem Solving - Dots");
            //get owner
            var owner = GetSessionUserId();
            //validate in studentActivity
            var stdAct2 = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityID && m.CreatedBy == owner);

            if (stdAct2 != null)
            {
                filePath = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == stdAct2.StudentActivityId);
            }

            return View(filePath);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page42UploadFile(FilePath filepath, UploadFileModel fileModel)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page42").WithInfo("This is just a demo.");
            }
            var studentActivity = new StudentActivity();
            try
            {
                if (fileModel.File != null && fileModel.File.ContentLength > 0)
                {
                    // extract the file content to byte array
                    var content = new byte[fileModel.File.ContentLength];
                    // reads the content from stream
                    fileModel.File.InputStream.Read(content, 0, fileModel.File.ContentLength);

                    var newImage = new FilePath
                    {
                        FileName = System.IO.Path.GetFileName(fileModel.File.FileName),
                        FileType = FileType.Photo,
                        ContentType = fileModel.File.ContentType,
                        Content = content,
                        CreatedBy = GetSessionUserId(),
                        CreatedDateTime = DateTime.Now

                    };

                    //get temp activity id by title
                    //note: title should be the same with the search keyword when using lamda expression
                    byte activityId = GetTempActivityID("Problem Solving - Dots");
                    //get module id
                    var moduleId = GetTempModuleIdByActivityID(activityId);
                    //get current user
                    var owner = GetSessionUserId();
                    //check if record existed
                    StudentActivity oldRec =
                        _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId &&
                                                              m.CreatedBy == owner);
                    if (oldRec != null)
                    {
                        //update
                        oldRec.ModifiedBy = GetSessionUserId();
                        oldRec.ModifiedDateTime = DateTime.Now;
                        //update studentActivity table
                        _context.Entry(oldRec).State = EntityState.Modified;

                        //since there are studentactivity record there should be a file
                        var oldFile = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == oldRec.StudentActivityId);

                        //update new content
                        oldFile.Content = newImage.Content;
                        _context.Entry(oldFile).State = EntityState.Modified;
                        //save all changes
                        _context.SaveChanges();


                    }
                    else
                    {
                        //create
                        studentActivity.TempActivityId = activityId;
                        studentActivity.TempModuleId = moduleId;

                        studentActivity.CreatedDateTime = DateTime.Now;
                        studentActivity.CreatedBy = GetSessionUserId();

                        studentActivity.ProgressValue = 100;
                        studentActivity.Type = ActivityCategory.FileSubmission;
                        studentActivity.ModifiedBy = GetSessionUserId();
                        studentActivity.ModifiedDateTime = DateTime.Now;

                        studentActivity.Context = string.Empty;
                        studentActivity.CFDominantFirst = string.Empty;
                        studentActivity.CFDominantSecond = string.Empty;
                        studentActivity.VarkResult = string.Empty;
                        studentActivity.DopeResult = string.Empty;
                        studentActivity.DiscResult = string.Empty;
                        studentActivity.Top3PersonalValues = string.Empty;


                        _context.StudentActivities.Add(studentActivity);

                        _context.SaveChanges();

                        //studentActivityId should have unique id after saving
                        newImage.StudentActivityId = studentActivity.StudentActivityId;

                        _context.FilePaths.Add(newImage);

                        _context.SaveChanges();

                        // record overall progress
                        ComputeOverallProgress();


                    }

                }
                return RedirectToAction("Page42").WithSuccess("File uploaded successfully!");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Page42").WithError("File upload failed!" + ex.Message);
            }
        }

        public FileResult Page42DownloadFile(string Id)
        {
            var stdId = byte.Parse(Id);

            var fileToRetrieve = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == stdId);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }





        public ActionResult Page43()
        {

            // title: Problem Solving - Cake
            // type: FileSubmission

            FilePath filePath = new FilePath();

            //get id in tempActivity
            var activityID = GetTempActivityID("Problem Solving - Cake");
            //get owner
            var owner = GetSessionUserId();
            //validate in studentActivity
            var stdAct2 = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityID && m.CreatedBy == owner);

            if (stdAct2 != null)
            {
                filePath = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == stdAct2.StudentActivityId);
            }

            return View(filePath);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page43UploadFile(FilePath filepath, UploadFileModel fileModel)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page43").WithInfo("This is just a demo.");
            }
            var studentActivity = new StudentActivity();
            try
            {
                if (fileModel.File != null && fileModel.File.ContentLength > 0)
                {
                    // extract the file content to byte array
                    var content = new byte[fileModel.File.ContentLength];
                    // reads the content from stream
                    fileModel.File.InputStream.Read(content, 0, fileModel.File.ContentLength);

                    var newImage = new FilePath
                    {
                        FileName = System.IO.Path.GetFileName(fileModel.File.FileName),
                        FileType = FileType.Photo,
                        ContentType = fileModel.File.ContentType,
                        Content = content,
                        CreatedBy = GetSessionUserId(),
                        CreatedDateTime = DateTime.Now

                    };

                    //get temp activity id by title
                    //note: title should be the same with the search keyword when using lamda expression
                    byte activityId = GetTempActivityID("Problem Solving - Cake");
                    //get module id
                    var moduleId = GetTempModuleIdByActivityID(activityId);
                    //get current user
                    var owner = GetSessionUserId();
                    //check if record existed
                    StudentActivity oldRec =
                        _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId &&
                                                              m.CreatedBy == owner);
                    if (oldRec != null)
                    {
                        //update
                        oldRec.ModifiedBy = GetSessionUserId();
                        oldRec.ModifiedDateTime = DateTime.Now;
                        //update studentActivity table
                        _context.Entry(oldRec).State = EntityState.Modified;

                        //since there are studentactivity record there should be a file
                        var oldFile = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == oldRec.StudentActivityId);

                        //update new content
                        oldFile.Content = newImage.Content;
                        _context.Entry(oldFile).State = EntityState.Modified;
                        //save all changes
                        _context.SaveChanges();


                    }
                    else
                    {
                        //create
                        studentActivity.TempActivityId = activityId;
                        studentActivity.TempModuleId = moduleId;

                        studentActivity.CreatedDateTime = DateTime.Now;
                        studentActivity.CreatedBy = GetSessionUserId();

                        studentActivity.ProgressValue = 100;
                        studentActivity.Type = ActivityCategory.FileSubmission;
                        studentActivity.ModifiedBy = GetSessionUserId();
                        studentActivity.ModifiedDateTime = DateTime.Now;

                        studentActivity.Context = string.Empty;
                        studentActivity.CFDominantFirst = string.Empty;
                        studentActivity.CFDominantSecond = string.Empty;
                        studentActivity.VarkResult = string.Empty;
                        studentActivity.DopeResult = string.Empty;
                        studentActivity.DiscResult = string.Empty;
                        studentActivity.Top3PersonalValues = string.Empty;


                        _context.StudentActivities.Add(studentActivity);

                        _context.SaveChanges();

                        //studentActivityId should have unique id after saving
                        newImage.StudentActivityId = studentActivity.StudentActivityId;

                        _context.FilePaths.Add(newImage);

                        _context.SaveChanges();

                        // record overall progress
                        ComputeOverallProgress();


                    }

                }
                return RedirectToAction("Page43").WithSuccess("File uploaded successfully!");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Page43").WithError("File upload failed!" + ex.Message);
            }
        }

        public FileResult Page43DownloadFile(string Id)
        {
            var stdId = byte.Parse(Id);

            var fileToRetrieve = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == stdId);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }



        public ActionResult Page44()
        {
            return View();
        }


        public ActionResult Page45()
        {

            // title: Draw Mind Map
            // type: FileSubmission

            FilePath filePath = new FilePath();

            //get id in tempActivity
            var activityID = GetTempActivityID("Draw Mind Map");
            //get owner
            var owner = GetSessionUserId();
            //validate in studentActivity
            var stdAct2 = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityID && m.CreatedBy == owner);

            if (stdAct2 != null)
            {
                filePath = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == stdAct2.StudentActivityId);
            }

            return View(filePath);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page45UploadFile(FilePath filepath, UploadFileModel fileModel)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page45").WithInfo("This is just a demo.");
            }
            var studentActivity = new StudentActivity();
            try
            {
                if (fileModel.File != null && fileModel.File.ContentLength > 0)
                {
                    // extract the file content to byte array
                    var content = new byte[fileModel.File.ContentLength];
                    // reads the content from stream
                    fileModel.File.InputStream.Read(content, 0, fileModel.File.ContentLength);

                    var newImage = new FilePath
                    {
                        FileName = System.IO.Path.GetFileName(fileModel.File.FileName),
                        FileType = FileType.Photo,
                        ContentType = fileModel.File.ContentType,
                        Content = content,
                        CreatedBy = GetSessionUserId(),
                        CreatedDateTime = DateTime.Now

                    };

                    //get temp activity id by title
                    //note: title should be the same with the search keyword when using lamda expression
                    byte activityId = GetTempActivityID("Draw Mind Map");
                    //get module id
                    var moduleId = GetTempModuleIdByActivityID(activityId);
                    //get current user
                    var owner = GetSessionUserId();
                    //check if record existed
                    StudentActivity oldRec =
                        _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId &&
                                                              m.CreatedBy == owner);
                    if (oldRec != null)
                    {
                        //update
                        oldRec.ModifiedBy = GetSessionUserId();
                        oldRec.ModifiedDateTime = DateTime.Now;
                        //update studentActivity table
                        _context.Entry(oldRec).State = EntityState.Modified;

                        //since there are studentactivity record there should be a file
                        var oldFile = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == oldRec.StudentActivityId);

                        //update new content
                        oldFile.Content = newImage.Content;
                        _context.Entry(oldFile).State = EntityState.Modified;
                        //save all changes
                        _context.SaveChanges();


                    }
                    else
                    {
                        //create
                        studentActivity.TempActivityId = activityId;
                        studentActivity.TempModuleId = moduleId;

                        studentActivity.CreatedDateTime = DateTime.Now;
                        studentActivity.CreatedBy = GetSessionUserId();

                        studentActivity.ProgressValue = 100;
                        studentActivity.Type = ActivityCategory.FileSubmission;
                        studentActivity.ModifiedBy = GetSessionUserId();
                        studentActivity.ModifiedDateTime = DateTime.Now;

                        studentActivity.Context = string.Empty;
                        studentActivity.CFDominantFirst = string.Empty;
                        studentActivity.CFDominantSecond = string.Empty;
                        studentActivity.VarkResult = string.Empty;
                        studentActivity.DopeResult = string.Empty;
                        studentActivity.DiscResult = string.Empty;
                        studentActivity.Top3PersonalValues = string.Empty;


                        _context.StudentActivities.Add(studentActivity);

                        _context.SaveChanges();

                        //studentActivityId should have unique id after saving
                        newImage.StudentActivityId = studentActivity.StudentActivityId;

                        _context.FilePaths.Add(newImage);

                        _context.SaveChanges();

                        // record overall progress
                        ComputeOverallProgress();


                    }

                }
                return RedirectToAction("Page45").WithSuccess("File uploaded successfully!");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Page45").WithError("File upload failed!" + ex.Message);
            }
        }

        public FileResult Page45DownloadFile(string Id)
        {
            var stdId = byte.Parse(Id);

            var fileToRetrieve = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == stdId);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
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
        public ActionResult Page55()
        {
            return View();
        }


        // redirect to survey controller actions

        public ActionResult ProgramSurvey()
        {
            return RedirectToAction("ProgramSurvey", "Survey", null);
        }

        public ActionResult TutorSurvey()
        {
            return RedirectToAction("TutorSurvey", "Survey", null);
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


        // update overall progress of student
        private void ComputeOverallProgress()
        {
            try
            {


                //get logged user id
                var cur_user = GetSessionUserId();
                //get logged user activities count
                var totalStudentActivities = _context.StudentActivities.Where(m => m.CreatedBy == cur_user).Count();
                //get total activities
                var totalActivities = _context.TempActivities.Where(m => m.IsActivity == true && m.IsRemoved == false).Count();
                //compute overall progress
                var value = ((double)totalStudentActivities / totalActivities) * 100;
                var percentage = Math.Round(value, 2);
                //save to user table
                var user = _context.Users.Where(m => m.Id == cur_user).FirstOrDefault();
                if (user != null)
                {
                    user.OverallProgress = percentage;
                }
                else
                {
                    throw new Exception("User not found!");
                }

                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }


        }

        // logic for each custom activity
        private int GetMatchedWords(SlangClass slang)
        {
            int matched = 0;
            try
            {



                var correctAns = _context.MatchLookup.OrderBy(m => m.QuestionNumber).ToList();

                if (slang.Answer1 != null && slang.Answer1.Trim() == correctAns[0].CorrectAnswer.Trim()) matched++;
                if (slang.Answer2 != null && slang.Answer2.Trim() == correctAns[1].CorrectAnswer.Trim()) matched++;
                if (slang.Answer3 != null && slang.Answer3.Trim() == correctAns[2].CorrectAnswer.Trim()) matched++;
                if (slang.Answer4 != null && slang.Answer4.Trim() == correctAns[3].CorrectAnswer.Trim()) matched++;
                if (slang.Answer5 != null && slang.Answer5.Trim() == correctAns[4].CorrectAnswer.Trim()) matched++;
                if (slang.Answer6 != null && slang.Answer6.Trim() == correctAns[5].CorrectAnswer.Trim()) matched++;
                if (slang.Answer7 != null && slang.Answer7.Trim() == correctAns[6].CorrectAnswer.Trim()) matched++;
                if (slang.Answer8 != null && slang.Answer8.Trim() == correctAns[7].CorrectAnswer.Trim()) matched++;
                if (slang.Answer9 != null && slang.Answer9.Trim() == correctAns[8].CorrectAnswer.Trim()) matched++;
                if (slang.Answer10 != null && slang.Answer10.Trim() == correctAns[9].CorrectAnswer.Trim()) matched++;
                if (slang.Answer11 != null && slang.Answer11.Trim() == correctAns[10].CorrectAnswer.Trim()) matched++;
                if (slang.Answer12 != null && slang.Answer12.Trim() == correctAns[11].CorrectAnswer.Trim()) matched++;
                if (slang.Answer13 != null && slang.Answer13.Trim() == correctAns[12].CorrectAnswer.Trim()) matched++;
                if (slang.Answer14 != null && slang.Answer14.Trim() == correctAns[13].CorrectAnswer.Trim()) matched++;
                if (slang.Answer15 != null && slang.Answer15.Trim() == correctAns[14].CorrectAnswer.Trim()) matched++;
                if (slang.Answer16 != null && slang.Answer16.Trim() == correctAns[15].CorrectAnswer.Trim()) matched++;
                if (slang.Answer17 != null && slang.Answer17.Trim() == correctAns[16].CorrectAnswer.Trim()) matched++;
                if (slang.Answer18 != null && slang.Answer18.Trim() == correctAns[17].CorrectAnswer.Trim()) matched++;
                if (slang.Answer19 != null && slang.Answer19.Trim() == correctAns[18].CorrectAnswer.Trim()) matched++;
                if (slang.Answer20 != null && slang.Answer20.Trim() == correctAns[19].CorrectAnswer.Trim()) matched++;
                if (slang.Answer21 != null && slang.Answer21.Trim() == correctAns[20].CorrectAnswer.Trim()) matched++;
                if (slang.Answer22 != null && slang.Answer22.Trim() == correctAns[21].CorrectAnswer.Trim()) matched++;
                if (slang.Answer23 != null && slang.Answer23.Trim() == correctAns[22].CorrectAnswer.Trim()) matched++;
                if (slang.Answer24 != null && slang.Answer24.Trim() == correctAns[23].CorrectAnswer.Trim()) matched++;
                if (slang.Answer25 != null && slang.Answer25.Trim() == correctAns[24].CorrectAnswer.Trim()) matched++;
                if (slang.Answer26 != null && slang.Answer26.Trim() == correctAns[25].CorrectAnswer.Trim()) matched++;
                if (slang.Answer27 != null && slang.Answer27.Trim() == correctAns[26].CorrectAnswer.Trim()) matched++;
                if (slang.Answer28 != null && slang.Answer28.Trim() == correctAns[27].CorrectAnswer.Trim()) matched++;
                if (slang.Answer29 != null && slang.Answer29.Trim() == correctAns[28].CorrectAnswer.Trim()) matched++;
                if (slang.Answer30 != null && slang.Answer30.Trim() == correctAns[29].CorrectAnswer.Trim()) matched++;
                if (slang.Answer31 != null && slang.Answer31.Trim() == correctAns[30].CorrectAnswer.Trim()) matched++;
                if (slang.Answer32 != null && slang.Answer32.Trim() == correctAns[31].CorrectAnswer.Trim()) matched++;
                if (slang.Answer33 != null && slang.Answer33.Trim() == correctAns[32].CorrectAnswer.Trim()) matched++;
                if (slang.Answer34 != null && slang.Answer34.Trim() == correctAns[33].CorrectAnswer.Trim()) matched++;
                if (slang.Answer35 != null && slang.Answer35.Trim() == correctAns[34].CorrectAnswer.Trim()) matched++;
                if (slang.Answer36 != null && slang.Answer36.Trim() == correctAns[35].CorrectAnswer.Trim()) matched++;
                if (slang.Answer37 != null && slang.Answer37.Trim() == correctAns[36].CorrectAnswer.Trim()) matched++;
                if (slang.Answer38 != null && slang.Answer38.Trim() == correctAns[37].CorrectAnswer.Trim()) matched++;
                if (slang.Answer39 != null && slang.Answer39.Trim() == correctAns[38].CorrectAnswer.Trim()) matched++;
                if (slang.Answer40 != null && slang.Answer40.Trim() == correctAns[39].CorrectAnswer.Trim()) matched++;
                if (slang.Answer41 != null && slang.Answer41.Trim() == correctAns[40].CorrectAnswer.Trim()) matched++;
                if (slang.Answer42 != null && slang.Answer42.Trim() == correctAns[41].CorrectAnswer.Trim()) matched++;
                if (slang.Answer43 != null && slang.Answer43.Trim() == correctAns[42].CorrectAnswer.Trim()) matched++;
                if (slang.Answer44 != null && slang.Answer44.Trim() == correctAns[43].CorrectAnswer.Trim()) matched++;
                if (slang.Answer45 != null && slang.Answer45.Trim() == correctAns[44].CorrectAnswer.Trim()) matched++;
                if (slang.Answer46 != null && slang.Answer46.Trim() == correctAns[45].CorrectAnswer.Trim()) matched++;

            }
            catch (Exception)
            {

                throw;
            }

            return matched;
        }

        private List<Vark> GetVarkList(FormCollection fc)
        {
            int rows = 11;
            int cur_row = 0;
            var varkList = new List<Vark>();

            for (int i = 0; i < rows; i++)
            {
                cur_row = i + 1;
                var row = fc.Get("VARK" + cur_row);

                if (row == null)
                {
                    break;
                }

                var vark = new Vark();
                switch (row.ToString())
                {
                    case "V":
                        vark.Visual = true;
                        break;
                    case "A":
                        vark.Auditory = true;
                        break;
                    case "R":
                        vark.ReadingWriting = true;
                        break;
                    case "K":
                        vark.Kinesthetic = true;
                        break;
                }

                varkList.Add(vark);

            }

            return varkList;
        }

        private string GetVarkResult(List<Vark> varkList)
        {

            //check count
            var keyValPair = new Dictionary<string, int>();

            keyValPair.Add("Visual", varkList.Count(m => m.Visual == true));
            keyValPair.Add("Auditory", varkList.Count(m => m.Auditory == true));
            keyValPair.Add("Reading/Writing", varkList.Count(m => m.ReadingWriting == true));
            keyValPair.Add("Kinesthetic", varkList.Count(m => m.Kinesthetic == true));

            keyValPair.OrderByDescending(x => x.Value);


            //get the value of highest key
            int biggest = 0;
            string yourVark = string.Empty;

            foreach (var item in keyValPair)
            {

                if (item.Value > biggest)
                {
                    biggest = item.Value;
                    yourVark = item.Key;
                }
                else if (item.Value == biggest)
                {
                    yourVark += "," + item.Key;
                }
            }

            return yourVark;
        }

        private string GetDiscResult(FormCollection fc)
        {
            int d = 0;
            int i = 0;
            int s = 0;
            int c = 0;


            var disc = new Dictionary<string, int>();

            if (fc.Count == 1) return string.Empty;

            for (int x = 1; x < 16; x++)
            {
                var item = fc.Get($"DISC{x}");

                switch (item)
                {
                    case "D": d++; break;
                    case "I": i++; break;
                    case "S": s++; break;
                    case "C": c++; break;
                }

            }

            disc.Add("Dominant", d);
            disc.Add("Influential", i);
            disc.Add("Steady", s);
            disc.Add("Conscientious", c);

            var discResult = disc.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;


            return discResult;
        }

        private string GetDopeResult(FormCollection fc)
        {
            var dope = new Dictionary<string, int>();

            if (fc.Count == 1) return String.Empty;

            var checkboxes = fc.Get("DOPE").Split(',');

            int d = checkboxes.Where(m => m == "D").Count();
            dope.Add("Dove", d);

            int o = checkboxes.Where(m => m == "O").Count();
            dope.Add("Owl", o);

            int p = checkboxes.Where(m => m == "P").Count();
            dope.Add("Peacock", p);

            int e = checkboxes.Where(m => m == "E").Count();
            dope.Add("Eagle", e);


            var dopeResult = dope.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

            return dopeResult;
        }

        private string GetPersonalValues(FormCollection fc)
        {
            var personals = fc.Get("PersonalValues");
            //custom values
            var chk1 = fc.Get("chk1");
            var customval1 = fc.Get("chk1value");
            if (chk1 == "on") personals += "," + customval1;

            var chk2 = fc.Get("chk2");
            var customval2 = fc.Get("chk2value");
            if (chk2 == "on") personals += "," + customval2;

            var chk3 = fc.Get("chk3");
            var customval3 = fc.Get("chk3value");
            if (chk3 == "on") personals += "," + customval3;

            var chk4 = fc.Get("chk4");
            var customval4 = fc.Get("chk4value");
            if (chk4 == "on") personals += "," + customval4;

            var chk5 = fc.Get("chk5");
            var customval5 = fc.Get("chk5value");
            if (chk5 == "on") personals += "," + customval5;

            var chk6 = fc.Get("chk6");
            var customval6 = fc.Get("chk6value");
            if (chk6 == "on") personals += "," + customval6;

            return personals;
        }


    }
}