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

            ////////////  this code is for page 13 /////////////////
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
            //////////// for page 13 code //////////////////////////

            FilePath filePath = new FilePath();

            //get id in tempActivity
            var activityID = GetTempActivityID("My Shield");
            //validate in studentActivity
            var stdAct2 = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityID);

            if (stdAct2 != null)
            {
                filePath = _context.FilePaths.FirstOrDefault(m => m.StudentActivityId == stdAct2.StudentActivityId);
            }

            return View(filePath);
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
                        Content = content

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
                return RedirectToAction("Page14").WithSuccess("File uploaded successfully!");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Page14").WithError("File upload failed!" + ex.Message);
            }
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
                var studentRecord = _context.StudentActivities.FirstOrDefault(m => m.TempActivityId == activityId && m.CreatedBy == owner);

                if (studentRecord == null)
                {
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

                    //insert record
                    _context.StudentActivities.Add(newRecord);
                }
                else
                {
                    studentRecord.Context = context;
                    studentRecord.ModifiedBy = GetSessionUserId();
                    studentRecord.ModifiedDateTime = DateTime.Now;

                    //modify record
                    _context.Entry(studentRecord).State = EntityState.Modified;

                }

                //save record
                _context.SaveChanges();

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
            Session["Page22Completed"] = true;
            return View();
        }


        public ActionResult Page23()
        {

            #region for Page22
            ////////////  this code is for page 22 /////////////////
            if (User.IsInRole("Tutor") || User.IsInRole("Administrator")) { }
            else
            {
                if (Session["Page22Completed"] != null)
                {
                    try
                    {
                        //save class contract as completed
                        //validate if record already existed
                        var tempAct = _context.TempActivities.FirstOrDefault(m => m.Title == "Class Contract");

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

                        Session["Page22Completed"] = null;
                        // exclude fields not to be saved

                        _context.StudentActivities.Add(studentActivity);
                        _context.SaveChanges();

                        return View().WithInfo("Congratulation! You have completed the class contract activity!");
                    }
                    catch (EntityException e)
                    {
                        throw new Exception(e.Message);
                    }

                }
            }
            //////////// for page 22 code //////////////////////////
            #endregion


            var vark = new List<Vark>();
            ViewBag.ActDone = false;

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

            //get temp activity id by title
            //note: title should be the same with the search keyword when using lamda expression
            byte activityId = GetTempActivityID("VARK");
            //get module id
            var moduleId = GetTempModuleIdByActivityID(activityId);
            //get current user
            var owner = GetSessionUserId();


            try
            {

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

                return View("Page23").WithError(ex.Message);
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
                        Content = content

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
                        Content = content

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