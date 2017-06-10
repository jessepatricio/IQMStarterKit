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
                // note: should indicate temp workbook id no
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
            if (User.IsInRole("Tutor") || User.IsInRole("Administrator")) { }
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
            var owner2 = GetSessionUserId();
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

                            return View("Page23").WithInfo("Congratulation! You have completed the Class Contract activity!");
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

                return RedirectToAction("Page23").WithSuccess("Saved successfully! " + $"You are {yourVark}");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Page23").WithError(ex.Message);
            }

        }


        public ActionResult Page24()
        {
            FilePath filePath = new FilePath();

            //get id in tempActivity
            var activityID = GetTempActivityID("Human Bingo - Activity");
            //validate in studentActivity
            var stdAct2 = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityID);

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
            FilePath filePath = new FilePath();

            //get id in tempActivity
            var activityID = GetTempActivityID("Photo Scavenger Hunt");
            //validate in studentActivity
            var stdAct2 = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityID);

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
        public ActionResult Page26(FormCollection fc)
        {
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
        public ActionResult Page28(FormCollection fc)
        {

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

                return RedirectToAction("Page28").WithSuccess("Saved successfully!");
            }
            catch (Exception ex)
            {
                return View(kiwiana).WithError(ex.Message);
            }

        }






        public ActionResult Page29()
        {
            var objSlang = new SlangClass();

            return View(objSlang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Page29(SlangClass slang)
        {
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

        private int GetMatchedWords(SlangClass slang)
        {
            return 0;
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