﻿using IQMStarterKit.Models;
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

            var cheese = new Cheese();
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
                cheese = JsonConvert.DeserializeObject<Cheese>(rec.Context);
                cheese.StudentActivity = rec;
            }

            return View(cheese);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page30(FormCollection fc)
        {
            var cheese = new Cheese();

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
            // title: My 14 Habits for More Effective Me
            // type: FormSubmission

            var myHabit = new My14HabitsClass();
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("My 14 Habits for More Effective Me");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                //get context and deserialize
                myHabit = JsonConvert.DeserializeObject<My14HabitsClass>(rec.Context);
                myHabit.StudentActivity = rec;
            }

            return View(myHabit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page31(FormCollection fc)
        {
            var myHabit = new My14HabitsClass();

            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page31").WithInfo("This is just a demo.");
            }

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("My 14 Habits for More Effective Me");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();

            try
            {
                //generate context string from form collection

                myHabit.Proactive1 = fc.Get("Proactive1").ToString();
                myHabit.Proactive2 = fc.Get("Proactive2").ToString();
                myHabit.EndMind1 = fc.Get("EndMind1").ToString();
                myHabit.EndMind2 = fc.Get("EndMind2").ToString();
                myHabit.FirstThings1 = fc.Get("FirstThings1").ToString();
                myHabit.FirstThings2 = fc.Get("FirstThings2").ToString();
                myHabit.WinWin1 = fc.Get("WinWin1").ToString();
                myHabit.WinWin2 = fc.Get("WinWin2").ToString();
                myHabit.SeekFirst1 = fc.Get("SeekFirst1").ToString();
                myHabit.SeekFirst2 = fc.Get("SeekFirst2").ToString();
                myHabit.Synergise1 = fc.Get("Synergise1").ToString();
                myHabit.Synergise2 = fc.Get("Synergise2").ToString();
                myHabit.SharpenSaw1 = fc.Get("SharpenSaw1").ToString();
                myHabit.SharpenSaw2 = fc.Get("SharpenSaw2").ToString();

                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(myHabit);

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

                return RedirectToAction("Page31").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {
                return View(myHabit).WithError(ex.Message);
            }
        }

        public ActionResult Page32()
        {
            // title: think like ceo
            // type: FormSubmission

            var thinkceo = new ThinkCEO();
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
                thinkceo = JsonConvert.DeserializeObject<ThinkCEO>(rec.Context);
                thinkceo.StudentActivity = rec;
            }

            return View(thinkceo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page32(FormCollection fc)
        {
            var thinkceo = new ThinkCEO();

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
            // title: Personal Leadership
            // type: Scoring

            ViewBag.ActDone = false;

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Personal Leadership");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                ViewBag.ActDone = true;
                ViewBag.PersonalLeadershipScore = rec.PersonalLeaderShipScore;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page34(FormCollection fc)
        {

            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page34").WithInfo("This is just a demo.");
            }
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Personal Leadership");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();


            try
            {
                // get score from collection
                var personalLeadership = PersonalLeaderShipScore(fc);

                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(personalLeadership);


                //insert record

                var newRecord = new StudentActivity();
                newRecord.TempActivityId = activityId;
                newRecord.TempModuleId = moduleId;
                newRecord.Context = context;
                newRecord.PersonalLeaderShipScore = personalLeadership.TotalScore;
                newRecord.ProgressValue = 100;
                newRecord.Type = ActivityCategory.Score;

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

                return RedirectToAction("Page34").WithSuccess("Saved successfully! " + $"You scored {personalLeadership.TotalScore} points");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Page34").WithError(ex.Message);
            }

        }


        public ActionResult Page35()
        {
            // title: Personal Leadership Plan
            // type: FormSubmission

            var leadershipPlan = new PersonalLeadershipPlanClass();
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Goal Setting (Personal Leadership Plan)");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                //get context and deserialize
                leadershipPlan = JsonConvert.DeserializeObject<PersonalLeadershipPlanClass>(rec.Context);
                leadershipPlan.StudentActivity = rec;
            }

            return View(leadershipPlan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page35(FormCollection fc)
        {
            var leadershipPlan = new PersonalLeadershipPlanClass();

            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page35").WithInfo("This is just a demo.");
            }

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Goal Setting (Personal Leadership Plan)");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();

            try
            {
                //generate context string from form collection

                leadershipPlan.Goal1 = fc.Get("Goal1").ToString();
                leadershipPlan.ActionPlan1 = fc.Get("ActionPlan1").ToString();
                leadershipPlan.Goal2 = fc.Get("Goal2").ToString();
                leadershipPlan.ActionPlan2 = fc.Get("ActionPlan2").ToString();
                leadershipPlan.Goal3 = fc.Get("Goal3").ToString();
                leadershipPlan.ActionPlan3 = fc.Get("ActionPlan3").ToString();
                leadershipPlan.Goal4 = fc.Get("Goal4").ToString();
                leadershipPlan.ActionPlan4 = fc.Get("ActionPlan4").ToString();

                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(leadershipPlan);

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

                return RedirectToAction("Page35").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {
                return View(leadershipPlan).WithError(ex.Message);
            }
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
            // title: self managing
            // type: Scoring

            ViewBag.ActDone = false;

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Self-Management");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                ViewBag.ActDone = true;
                ViewBag.SelfManagementScore = rec.SelfManagementScore;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page37(FormCollection fc)
        {

            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page37").WithInfo("This is just a demo.");
            }
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Self-Management");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();


            try
            {
                // get score from collection
                var selfManagement = GetSelfManagementScore(fc);

                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(selfManagement);


                //insert record

                var newRecord = new StudentActivity();
                newRecord.TempActivityId = activityId;
                newRecord.TempModuleId = moduleId;
                newRecord.Context = context;
                newRecord.PersonalLeaderShipScore = selfManagement.TotalScore;
                newRecord.ProgressValue = 100;
                newRecord.Type = ActivityCategory.Score;

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

                return RedirectToAction("Page34").WithSuccess("Saved successfully! " + $"You scored {selfManagement.TotalScore} points");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Page34").WithError(ex.Message);
            }

        }


        public ActionResult Page38()
        {

            // title: First Thing First
            // type: FormSubmission

            FirstThingFirstViewModel firstThing = new FirstThingFirstViewModel();

            //get id in tempActivity
            var activityID = GetTempActivityID("First Thing First");
            //get owner
            var owner = GetSessionUserId();
            //validate in studentActivity
            var stdAct2 = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityID && m.CreatedBy == owner);

            if (stdAct2 != null)
            {
                firstThing.FilePath = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == stdAct2.StudentActivityId);

                //get context and deserialize
                if (stdAct2.Context != "")
                {
                    firstThing.FirstThingFirst = JsonConvert.DeserializeObject<FirstThingFirst>(stdAct2.Context);
                }
            }

            return View(firstThing);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page38(FormCollection fc)
        {
            var firstThing = new FirstThingFirst();

            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page38").WithInfo("This is just a demo.");
            }

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("First Thing First");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();

            try
            {
                //generate context string from form collection

                firstThing.Activities = fc.Get("FirstThingFirst.Activities").ToString();

                firstThing.Matrix1 = fc.Get("FirstThingFirst.Matrix1").ToString();
                firstThing.Matrix2 = fc.Get("FirstThingFirst.Matrix1").ToString();
                firstThing.Matrix3 = fc.Get("FirstThingFirst.Matrix1").ToString();
                firstThing.Matrix4 = fc.Get("FirstThingFirst.Matrix1").ToString();

                firstThing.LargeTask = fc.Get("FirstThingFirst.LargeTask").ToString();


                firstThing.SmallTask1 = fc.Get("FirstThingFirst.SmallTask1").ToString();
                firstThing.SmallTask2 = fc.Get("FirstThingFirst.SmallTask2").ToString();
                firstThing.SmallTask3 = fc.Get("FirstThingFirst.SmallTask3").ToString();
                firstThing.SmallTask4 = fc.Get("FirstThingFirst.SmallTask4").ToString();
                firstThing.SmallTask5 = fc.Get("FirstThingFirst.SmallTask5").ToString();
                firstThing.SmallTask6 = fc.Get("FirstThingFirst.SmallTask6").ToString();
                firstThing.SmallTask7 = fc.Get("FirstThingFirst.SmallTask7").ToString();
                firstThing.SmallTask8 = fc.Get("FirstThingFirst.SmallTask8").ToString();
                firstThing.SmallTask9 = fc.Get("FirstThingFirst.SmallTask9").ToString();
                firstThing.SmallTask10 = fc.Get("FirstThingFirst.SmallTask10").ToString();


                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(firstThing);

                //validate if record already existed
                var studentRecord = _context.StudentActivities
                    .FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == owner);

                if (studentRecord == null)
                {
                    var newRecord = new StudentActivity();
                    newRecord.TempActivityId = activityId;
                    newRecord.TempModuleId = moduleId;
                    newRecord.Context = context;
                    newRecord.ProgressValue = 50;
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
                    if (studentRecord.ProgressValue == 50) studentRecord.ProgressValue = 100;
                    studentRecord.Context = context;
                    studentRecord.ModifiedBy = GetSessionUserId();
                    studentRecord.ModifiedDateTime = DateTime.Now;

                    // modify record
                    _context.Entry(studentRecord).State = EntityState.Modified;

                }

                // save record
                _context.SaveChanges();

                return RedirectToAction("Page38").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {
                return View(firstThing).WithError(ex.Message);
            }
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
                        if (oldRec.ProgressValue == 50) oldRec.ProgressValue = 100;


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

                        studentActivity.ProgressValue = 50;

                        studentActivity.Type = ActivityCategory.FormSubmission;


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
            // title: Presentations
            // type: FormSubmission

            PresentationEvaluationModel presentation = new PresentationEvaluationModel();

            //get id in tempActivity
            var activityID = GetTempActivityID("Presentations");
            //get owner
            var owner = GetSessionUserId();

            if (User.IsInRole("Student"))
            {
                //validate presentation evaluation record
                presentation = _context.PresentationEvaluationModel.FirstOrDefault(m => m.StudentId == owner);

                if (presentation == null) return RedirectToAction("Index").WithInfo("Your evaluation result is not yet available.");

            }
            else if (User.IsInRole("Tutor"))
            {
                if (Session["email"] != null)
                {
                    var student = UserManager.FindByEmail(Session["email"].ToString());
                    //validate presentation evaluation record
                    presentation = _context.PresentationEvaluationModel.FirstOrDefault(m => m.StudentId == student.Id);

                    if (presentation == null)
                    {
                        presentation = new PresentationEvaluationModel();
                        //start eval fill student records
                        presentation.StudentId = student.Id;
                        presentation.StudentName = student.FullName;
                        presentation.StudentEmail = student.Email;
                        presentation.TutorId = GetSessionUserId();
                        presentation.TutorName = GetFullName(owner);


                    }
                }
            }
            else if (User.IsInRole("Administrator"))
            {
                if (Session["email"] != null)
                {
                    var student = UserManager.FindByEmail(Session["email"].ToString());
                    //validate presentation evaluation record
                    presentation = _context.PresentationEvaluationModel.FirstOrDefault(m => m.StudentId == student.Id);

                    if (presentation == null) return View(presentation).WithInfo("Student evaluation result is not yet available.");

                }
            }


            return View(presentation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page39(PresentationEvaluationModel presentation)
        {


            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Page39").WithInfo("This is just a demo.");
            }




            try
            {


                //system fields
                presentation.CreatedBy = GetSessionUserId();
                presentation.CreatedDateTime = DateTime.Now;
                presentation.ModifiedBy = GetSessionUserId();
                presentation.ModifiedDateTime = DateTime.Now;

                //insert record
                _context.PresentationEvaluationModel.Add(presentation);


                // save record
                _context.SaveChanges();

                return RedirectToAction("Page39").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {
                return View(presentation).WithError(ex.Message);
            }



        }


        public ActionResult Page40()
        {
            return View();
        }
        public ActionResult Page41()
        {
            // title: Lost At Sea - Activity
            // type: FormSubmission

            var lostSea = new LostAtSeaJournalClass();
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Lost At Sea - Journal");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                //get context and deserialize
                lostSea = JsonConvert.DeserializeObject<LostAtSeaJournalClass>(rec.Context);
                lostSea.StudentActivity = rec;
            }

            return View(lostSea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page41(FormCollection fc)
        {
            var lostSea = new LostAtSeaJournalClass();

            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page41").WithInfo("This is just a demo.");
            }

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Lost At Sea - Journal");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();

            try
            {
                //generate context string from form collection

                lostSea.LostSeaJournalAns1 = fc.Get("LostSeaJournalAns1").ToString();
                lostSea.LostSeaJournalAns2 = fc.Get("LostSeaJournalAns2").ToString();
                lostSea.LostSeaJournalAns3 = fc.Get("LostSeaJournalAns3").ToString();
                lostSea.LostSeaJournalAns4 = fc.Get("LostSeaJournalAns4").ToString();
                lostSea.LostSeaJournalAns5 = fc.Get("LostSeaJournalAns5").ToString();
                lostSea.LostSeaJournalAns6 = fc.Get("LostSeaJournalAns6").ToString();


                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(lostSea);

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

                return RedirectToAction("Page41").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {
                return View(lostSea).WithError(ex.Message);
            }
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
            // title: Brainstorming
            // type: FormSubmission

            var brainstorming = new BrainstormingClass();
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Brainstorming");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                //get context and deserialize
                brainstorming = JsonConvert.DeserializeObject<BrainstormingClass>(rec.Context);
                brainstorming.StudentActivity = rec;
            }
            return View(brainstorming);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page44(FormCollection fc)
        {
            var brainstorming = new BrainstormingClass();

            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page44").WithInfo("This is just a demo.");
            }

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Brainstorming");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();

            try
            {
                //generate context string from form collection

                brainstorming.GrpMembers = fc.Get("GrpMembers").ToString();
                brainstorming.UsesFor = fc.Get("UsesFor").ToString();
                brainstorming.Uses1 = fc.Get("Uses1").ToString();
                brainstorming.Uses2 = fc.Get("Uses2").ToString();
                brainstorming.Uses3 = fc.Get("Uses3").ToString();
                brainstorming.Uses4 = fc.Get("Uses4").ToString();
                brainstorming.Uses5 = fc.Get("Uses5").ToString();
                brainstorming.Uses6 = fc.Get("Uses6").ToString();
                brainstorming.Uses7 = fc.Get("Uses7").ToString();
                brainstorming.Uses8 = fc.Get("Uses8").ToString();
                brainstorming.Uses9 = fc.Get("Uses9").ToString();
                brainstorming.Uses10 = fc.Get("Uses10").ToString();
                brainstorming.Uses11 = fc.Get("Uses11").ToString();
                brainstorming.Uses12 = fc.Get("Uses12").ToString();
                brainstorming.Uses13 = fc.Get("Uses13").ToString();
                brainstorming.Uses14 = fc.Get("Uses14").ToString();
                brainstorming.Uses15 = fc.Get("Uses15").ToString();
                brainstorming.Uses16 = fc.Get("Uses16").ToString();
                brainstorming.Uses17 = fc.Get("Uses17").ToString();
                brainstorming.Uses18 = fc.Get("Uses18").ToString();
                brainstorming.Uses19 = fc.Get("Uses19").ToString();
                brainstorming.Uses20 = fc.Get("Uses20").ToString();
                brainstorming.Uses21 = fc.Get("Uses21").ToString();
                brainstorming.Uses22 = fc.Get("Uses22").ToString();
                brainstorming.Uses23 = fc.Get("Uses23").ToString();
                brainstorming.Uses24 = fc.Get("Uses24").ToString();
                brainstorming.Uses25 = fc.Get("Uses25").ToString();
                brainstorming.Uses26 = fc.Get("Uses26").ToString();
                brainstorming.Uses27 = fc.Get("Uses27").ToString();
                brainstorming.Uses28 = fc.Get("Uses28").ToString();
                brainstorming.Uses29 = fc.Get("Uses29").ToString();
                brainstorming.Uses30 = fc.Get("Uses30").ToString();
                brainstorming.Uses31 = fc.Get("Uses31").ToString();
                brainstorming.Uses32 = fc.Get("Uses32").ToString();
                brainstorming.Uses33 = fc.Get("Uses33").ToString();
                brainstorming.Uses34 = fc.Get("Uses34").ToString();
                brainstorming.Uses35 = fc.Get("Uses35").ToString();
                brainstorming.Uses36 = fc.Get("Uses36").ToString();
                brainstorming.Uses37 = fc.Get("Uses37").ToString();
                brainstorming.Uses38 = fc.Get("Uses38").ToString();
                brainstorming.Uses39 = fc.Get("Uses39").ToString();
                brainstorming.Uses40 = fc.Get("Uses40").ToString();
                brainstorming.Uses41 = fc.Get("Uses41").ToString();
                brainstorming.Uses42 = fc.Get("Uses42").ToString();
                brainstorming.Uses43 = fc.Get("Uses43").ToString();
                brainstorming.Uses44 = fc.Get("Uses44").ToString();
                brainstorming.Uses45 = fc.Get("Uses45").ToString();
                brainstorming.Uses46 = fc.Get("Uses46").ToString();
                brainstorming.Uses47 = fc.Get("Uses47").ToString();
                brainstorming.Uses48 = fc.Get("Uses48").ToString();
                brainstorming.Uses49 = fc.Get("Uses49").ToString();
                brainstorming.Uses50 = fc.Get("Uses50").ToString();
                brainstorming.Uses51 = fc.Get("Uses51").ToString();
                brainstorming.Uses52 = fc.Get("Uses52").ToString();
                brainstorming.Uses53 = fc.Get("Uses53").ToString();
                brainstorming.Uses54 = fc.Get("Uses54").ToString();
                brainstorming.Uses55 = fc.Get("Uses55").ToString();
                brainstorming.Uses56 = fc.Get("Uses56").ToString();
                brainstorming.Uses57 = fc.Get("Uses57").ToString();
                brainstorming.Uses58 = fc.Get("Uses58").ToString();
                brainstorming.Uses59 = fc.Get("Uses59").ToString();
                brainstorming.Uses60 = fc.Get("Uses60").ToString();

                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(brainstorming);

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

                return RedirectToAction("Page44").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {
                return View(brainstorming).WithError(ex.Message);
            }
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
            // title: conflict  managing
            // type: Scoring

            ViewBag.ActDone = false;

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Conflict");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                ViewBag.ActDone = true;
                ViewBag.FirstDominant = rec.CFDominantFirst;
                ViewBag.SecondDominant = rec.CFDominantSecond;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page46(FormCollection fc)
        {

            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page46").WithInfo("This is just a demo.");
            }
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Conflict");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();


            try
            {
                // get score from collection
                var conflict = GetConflictManagementScore(fc);

                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(conflict);


                //insert record

                var newRecord = new StudentActivity();
                newRecord.TempActivityId = activityId;
                newRecord.TempModuleId = moduleId;
                newRecord.Context = context;
                newRecord.CFDominantFirst = conflict.FirstDominant;
                newRecord.CFDominantSecond = conflict.SecondDominant;
                newRecord.ProgressValue = 100;
                newRecord.Type = ActivityCategory.Score;

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

                return RedirectToAction("Page46").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Page46").WithError(ex.Message);
            }

        }

        public ActionResult Page47()
        {
            // title: Personal SWOT
            // type: FormSubmission

            var swot = new PersonalSWOTClass();
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Personal SWOT");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                //get context and deserialize
                swot = JsonConvert.DeserializeObject<PersonalSWOTClass>(rec.Context);
                swot.StudentActivity = rec;
            }

            return View(swot);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page47(FormCollection fc)
        {
            var swot = new PersonalSWOTClass();

            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page47").WithInfo("This is just a demo.");
            }

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Personal SWOT");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();

            try
            {
                //generate context string from form collection

                swot.Strengths = fc.Get("Strengths").ToString();
                swot.Weaknesses = fc.Get("Weaknesses").ToString();
                swot.Opportunities = fc.Get("Opportunities").ToString();
                swot.Threats = fc.Get("Threats").ToString();

                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(swot);

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

                return RedirectToAction("Page47").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {
                return View(swot).WithError(ex.Message);
            }
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
            // title: Pass the Ball - Journal
            // type: FormSubmission

            var passBall = new PassTheBallJournalClass();
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Pass the Ball - Journal");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                //get context and deserialize
                passBall = JsonConvert.DeserializeObject<PassTheBallJournalClass>(rec.Context);
                passBall.StudentActivity = rec;
            }

            return View(passBall);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page50(FormCollection fc)
        {
            var passBall = new PassTheBallJournalClass();

            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page50").WithInfo("This is just a demo.");
            }

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Pass the Ball - Journal");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();

            try
            {
                //generate context string from form collection

                passBall.PassBallJournalAns1 = fc.Get("PassBallJournalAns1").ToString();
                passBall.PassBallJournalAns2a = fc.Get("PassBallJournalAns2a").ToString();
                passBall.PassBallJournalAns2b = fc.Get("PassBallJournalAns2b").ToString();
                passBall.PassBallJournalAns2c = fc.Get("PassBallJournalAns2c").ToString();
                passBall.PassBallJournalAns3a = fc.Get("PassBallJournalAns3a").ToString();
                passBall.PassBallJournalAns3b = fc.Get("PassBallJournalAns3b").ToString();
                passBall.PassBallJournalAns3c = fc.Get("PassBallJournalAns3c").ToString();
                passBall.PassBallJournalAns4 = fc.Get("PassBallJournalAns4").ToString();

                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(passBall);

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

                return RedirectToAction("Page50").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {
                return View(passBall).WithError(ex.Message);
            }
        }



        public ActionResult Page51()
        {
            // title: Closed Fist - Journal
            // type: FormSubmission

            var closedFist = new ClosedFistJournalClass();
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Closed Fist - Journal");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                //get context and deserialize
                closedFist = JsonConvert.DeserializeObject<ClosedFistJournalClass>(rec.Context);
                closedFist.StudentActivity = rec;
            }

            return View(closedFist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page51(FormCollection fc)
        {
            var closedFist = new ClosedFistJournalClass();

            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page51").WithInfo("This is just a demo.");
            }

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Closed Fist - Journal");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();

            try
            {
                //generate context string from form collection

                closedFist.ClosedFistJournalAns1 = fc.Get("ClosedFistJournalAns1").ToString();
                closedFist.ClosedFistJournalAns2a = fc.Get("ClosedFistJournalAns2a").ToString();
                closedFist.ClosedFistJournalAns2b = fc.Get("ClosedFistJournalAns2b").ToString();
                closedFist.ClosedFistJournalAns2c = fc.Get("ClosedFistJournalAns2c").ToString();
                closedFist.ClosedFistJournalAns3a = fc.Get("ClosedFistJournalAns3a").ToString();
                closedFist.ClosedFistJournalAns3b = fc.Get("ClosedFistJournalAns3b").ToString();
                closedFist.ClosedFistJournalAns3c = fc.Get("ClosedFistJournalAns3c").ToString();
                closedFist.ClosedFistJournalAns4 = fc.Get("ClosedFistJournalAns4").ToString();

                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(closedFist);

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

                return RedirectToAction("Page51").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {
                return View(closedFist).WithError(ex.Message);
            }
        }


        public ActionResult Page52()
        {
            // title: Helium Stick - Journal
            // type: FormSubmission

            var heliumStick = new HeliumStickJournalClass();
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Helium Stick - Journal");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                //get context and deserialize
                heliumStick = JsonConvert.DeserializeObject<HeliumStickJournalClass>(rec.Context);
                heliumStick.StudentActivity = rec;
            }

            return View(heliumStick);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page52(FormCollection fc)
        {
            var heliumStick = new HeliumStickJournalClass();

            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page52").WithInfo("This is just a demo.");
            }

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Helium Stick - Journal");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();

            try
            {
                //generate context string from form collection

                heliumStick.HeliumStickJournalAns1 = fc.Get("HeliumStickJournalAns1").ToString();
                heliumStick.HeliumStickJournalAns2a = fc.Get("HeliumStickJournalAns2a").ToString();
                heliumStick.HeliumStickJournalAns2b = fc.Get("HeliumStickJournalAns2b").ToString();
                heliumStick.HeliumStickJournalAns2c = fc.Get("HeliumStickJournalAns2c").ToString();
                heliumStick.HeliumStickJournalAns3a = fc.Get("HeliumStickJournalAns3a").ToString();
                heliumStick.HeliumStickJournalAns3b = fc.Get("HeliumStickJournalAns3b").ToString();
                heliumStick.HeliumStickJournalAns3c = fc.Get("HeliumStickJournalAns3c").ToString();
                heliumStick.HeliumStickJournalAns4 = fc.Get("HeliumStickJournalAns4").ToString();

                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(heliumStick);

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

                return RedirectToAction("Page52").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {
                return View(heliumStick).WithError(ex.Message);
            }
        }



        public ActionResult Page53()
        {
            // title: Follow my Instructions - Journal
            // type: FormSubmission

            var followInstructions = new FollowMyInstructionsJournalClass();
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Follow my Instructions - Journal");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                //get context and deserialize
                followInstructions = JsonConvert.DeserializeObject<FollowMyInstructionsJournalClass>(rec.Context);
                followInstructions.StudentActivity = rec;
            }

            return View(followInstructions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page53(FormCollection fc)
        {
            var followInstructions = new FollowMyInstructionsJournalClass();

            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page53").WithInfo("This is just a demo.");
            }

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("Follow my Instructions - Journal");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();

            try
            {
                //generate context string from form collection

                followInstructions.FollowInstructionsJournalAns1 = fc.Get("FollowInstructionsJournalAns1").ToString();
                followInstructions.FollowInstructionsJournalAns2a = fc.Get("FollowInstructionsJournalAns2a").ToString();
                followInstructions.FollowInstructionsJournalAns2b = fc.Get("FollowInstructionsJournalAns2b").ToString();
                followInstructions.FollowInstructionsJournalAns2c = fc.Get("FollowInstructionsJournalAns2c").ToString();
                followInstructions.FollowInstructionsJournalAns3a = fc.Get("FollowInstructionsJournalAns3a").ToString();
                followInstructions.FollowInstructionsJournalAns3b = fc.Get("FollowInstructionsJournalAns3b").ToString();
                followInstructions.FollowInstructionsJournalAns3c = fc.Get("FollowInstructionsJournalAns3c").ToString();
                followInstructions.FollowInstructionsJournalAns4 = fc.Get("FollowInstructionsJournalAns4").ToString();

                // serialize to json format for context store
                string context = JsonConvert.SerializeObject(followInstructions);

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

                return RedirectToAction("Page53").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {
                return View(followInstructions).WithError(ex.Message);
            }
        }

        public ActionResult Page54()
        {
            return View();
        }




        public ActionResult Page55()
        {
            // title: How Assertive are your messages?
            // type: Scoring

            ViewBag.ActDone = false;

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("How Assertive are your messages?");

            // get Current User
            var user = GetSessionUserId();

            // validate if record already existed in student activity table
            var rec = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == user);

            if (rec != null)
            {
                ViewBag.ActDone = true;
                ViewBag.AssertiveScore = rec.AssertiveScore;

            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page55(FormCollection fc)
        {

            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                return RedirectToAction("Page55").WithInfo("This is just a demo.");
            }
            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("How Assertive are your messages?");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();


            try
            {
                // get score from collection
                var _assert = fc.Get("assert").Split(',').Select(int.Parse).ToArray();
                var _score = _assert.Sum();

                //insert record

                var newRecord = new StudentActivity();
                newRecord.TempActivityId = activityId;
                newRecord.TempModuleId = moduleId;
                newRecord.Context = string.Empty;
                newRecord.AssertiveScore = _score;
                newRecord.ProgressValue = 100;
                newRecord.Type = ActivityCategory.Score;

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

                return RedirectToAction("Page55").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Page55").WithError(ex.Message);
            }

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

        private PersonalLeadership PersonalLeaderShipScore(FormCollection fc)
        {
            int rows = 21;
            int cur_row = 0;
            int totalScore = 0;

            var retval = new PersonalLeadership();


            for (int i = 0; i < rows; i++)
            {
                cur_row = i + 1;
                var row = fc.Get("Leader" + cur_row);

                if (row == null) break;

                if (i == 0) retval.Value1 = row;
                if (i == 1) retval.Value2 = row;
                if (i == 2) retval.Value3 = row;
                if (i == 3) retval.Value4 = row;
                if (i == 4) retval.Value5 = row;
                if (i == 5) retval.Value6 = row;
                if (i == 6) retval.Value7 = row;
                if (i == 7) retval.Value8 = row;
                if (i == 9) retval.Value9 = row;
                if (i == 9) retval.Value10 = row;
                if (i == 10) retval.Value11 = row;
                if (i == 11) retval.Value12 = row;
                if (i == 12) retval.Value13 = row;
                if (i == 13) retval.Value14 = row;
                if (i == 14) retval.Value15 = row;
                if (i == 15) retval.Value16 = row;
                if (i == 16) retval.Value17 = row;
                if (i == 17) retval.Value18 = row;
                if (i == 18) retval.Value19 = row;
                if (i == 19) retval.Value20 = row;

                //add points
                switch (row.ToString())
                {
                    case "2":
                        totalScore += 2;
                        break;
                    case "1":
                        totalScore += 1;
                        break;
                }

            }

            retval.TotalScore = totalScore;

            return retval;
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

        private SelfManagement GetSelfManagementScore(FormCollection fc)
        {
            int rows = 21;
            int cur_row = 0;
            int totalScore = 0;

            var retval = new SelfManagement();


            for (int i = 0; i < rows; i++)
            {
                cur_row = i + 1;
                var row = fc.Get("self" + cur_row);

                if (row == null) break;

                if (i == 0) retval.Value1 = row;
                if (i == 1) retval.Value2 = row;
                if (i == 2) retval.Value3 = row;
                if (i == 3) retval.Value4 = row;
                if (i == 4) retval.Value5 = row;
                if (i == 5) retval.Value6 = row;
                if (i == 6) retval.Value7 = row;
                if (i == 7) retval.Value8 = row;
                if (i == 9) retval.Value9 = row;
                if (i == 9) retval.Value10 = row;
                if (i == 10) retval.Value11 = row;
                if (i == 11) retval.Value12 = row;
                if (i == 12) retval.Value13 = row;
                if (i == 13) retval.Value14 = row;
                if (i == 14) retval.Value15 = row;
                if (i == 15) retval.Value16 = row;
                if (i == 16) retval.Value17 = row;
                if (i == 17) retval.Value18 = row;
                if (i == 18) retval.Value19 = row;
                if (i == 19) retval.Value20 = row;

                //add points
                switch (row.ToString())
                {
                    case "4":
                        totalScore += 4;
                        break;
                    case "3":
                        totalScore += 3;
                        break;
                    case "2":
                        totalScore += 2;
                        break;
                    case "1":
                        totalScore += 1;
                        break;
                }

            }

            retval.TotalScore = totalScore;

            return retval;
        }

        private Conflict GetConflictManagementScore(FormCollection fc)
        {
            var dictMo = new Dictionary<string, int>();

            var retval = new Conflict();

            var _shark = fc.Get("Shark").Split(',').Select(int.Parse).ToArray();
            retval.Shark = _shark.Sum();
            dictMo.Add("Competing/Forcing Shark", retval.Shark);


            var _owl = fc.Get("Owl").Split(',').Select(int.Parse).ToArray();
            retval.Owl = _owl.Sum();
            dictMo.Add("Collaborating Owl", retval.Owl);


            var _turtle = fc.Get("Turtle").Split(',').Select(int.Parse).ToArray();
            retval.Turtle = _turtle.Sum();
            dictMo.Add("Avoiding Turtle", retval.Turtle);


            var _teddy = fc.Get("Teddy").Split(',').Select(int.Parse).ToArray();
            retval.Teddy = _teddy.Sum();
            dictMo.Add("Accomodating Teddy", retval.Teddy);

            var _fox = fc.Get("Fox").Split(',').Select(int.Parse).ToArray();
            retval.Fox = _fox.Sum();
            dictMo.Add("Compromising Fox", retval.Fox);


            dictMo.OrderByDescending(x => x.Value);

            var items = from pair in dictMo
                        orderby pair.Value descending
                        select pair;

            int cnt = 0;
            foreach (var item in items)
            {
                if (cnt == 0) retval.FirstDominant = item.Key.ToString();
                else if (cnt == 1) retval.SecondDominant = item.Key.ToString();
                else if (cnt > 1) break;
                cnt++;

            }




            return retval;
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