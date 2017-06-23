using IQMStarterKit.Models;
using IQMStarterKit.Models.Alert;
using IQMStarterKit.Models.Forms;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;


namespace IQMStarterKit.Controllers
{
    public class SurveyController : CommonController
    {

        ApplicationDbContext _context = new ApplicationDbContext();



        //list of program survey
        public ActionResult ProgramSurveyList()
        {

            var model = _context.ProgramSurveyModel;
            foreach (var item in model)
            {
                item.CreatedBy = GetFullName(item.CreatedBy);
            }

            return View(model);
        }

        //list of program survey
        public ActionResult TutorSurveyList()
        {

            var model = _context.TutorSurveyModel;
            foreach (var item in model)
            {
                item.CreatedBy = GetFullName(item.CreatedBy);
                item.TutorId = GetFullName(item.TutorId);
            }

            return View(model);
        }


        // GET: Survey Program
        public ActionResult ProgramSurvey()
        {
            return View();
        }
        // POST: Survey Program
        [HttpPost]
        public ActionResult ProgramSurvey(FormCollection fc)
        {
            //validate is user already submitted a survey
            var cur_user = GetSessionUserId();

            var rec = _context.ProgramSurveyModel.Where(m => m.CreatedBy == cur_user).FirstOrDefault();
            if (rec != null) return RedirectToAction("ProgramSurvey").WithInfo("You already submitted a programme survey!");



            var programSurvey = new ProgramSurveyModel();

            try
            {

                var r1 = fc.Get("r1");
                programSurvey.P1 = int.Parse(r1);
                var r2 = fc.Get("r2");
                programSurvey.P2 = int.Parse(r2);
                var r3 = fc.Get("r3");
                programSurvey.P3 = int.Parse(r3);
                var r4 = fc.Get("r4");
                programSurvey.P4 = int.Parse(r4);
                var r5 = fc.Get("r5");
                programSurvey.P5 = int.Parse(r5);
                var r6 = fc.Get("r6");
                programSurvey.P6 = int.Parse(r6);
                var r7 = fc.Get("r7");
                programSurvey.P7 = int.Parse(r7);
                var r8 = fc.Get("r8");
                programSurvey.POverall = r8;
                var r9 = fc.Get("r9");
                programSurvey.PTimeAllocated = r9;
                var r10 = fc.Get("r10");
                programSurvey.PClassSize = r10;
                var r11 = fc.Get("r11");
                programSurvey.PClassroom = r11;

                var ProgramComment = fc.Get("ProgramComment");
                programSurvey.PComment = (ProgramComment == "") ? string.Empty : ProgramComment;

                //systems field
                programSurvey.CreatedBy = User.Identity.GetUserId();
                programSurvey.ModifiedBy = User.Identity.GetUserId();
                programSurvey.CreatedDateTime = DateTime.Now;
                programSurvey.ModifiedDateTime = DateTime.Now;

                _context.ProgramSurveyModel.Add(programSurvey);

                _context.SaveChanges();

                return View(programSurvey).WithSuccess("Thank you, your survey has been submitted!");
            }
            catch (Exception ex)
            {
                return View(programSurvey).WithError("Error: " + ex.Message);

            }

        }

        // GET: Survey Tutor
        public ActionResult TutorSurvey()
        {
            var model = new TutorSurveyViewModel();
            var user = UserManager.FindByEmail(User.Identity.Name);

            model.TutorSurvey = _context.TutorSurveyModel;
            foreach (var item in model.TutorSurvey)
            {
                item.CreatedBy = GetFullName(item.CreatedBy);
                item.TutorId = GetFullName(item.TutorId);
            }

            //should get only the tutors assigned in student group 
            model.Tutors = GetGroupTutors(user.GroupId);

            return View(model);
        }
        // POST: Survey Tutor
        [HttpPost]
        public ActionResult TutorSurvey(FormCollection fc)
        {
            //validate if user submitting duplicate survey
            var cur_user = GetSessionUserId();

            string newTutor = Convert.ToString(Request.Form["AddTutor"]);

            var rec = _context.TutorSurveyModel.Where(m => m.CreatedBy == cur_user && m.TutorId == newTutor).FirstOrDefault();
            if (rec != null) return RedirectToAction("TutorSurvey").WithInfo("You already submitted a survey for this tutor!");


            var tutorSurvey = new TutorSurveyModel();

            var r1 = fc.Get("r1");
            tutorSurvey.T1 = int.Parse(r1);
            var r2 = fc.Get("r2");
            tutorSurvey.T2 = int.Parse(r2);
            var r3 = fc.Get("r3");
            tutorSurvey.T3 = int.Parse(r3);
            var r4 = fc.Get("r4");
            tutorSurvey.T4 = int.Parse(r4);
            var r5 = fc.Get("r5");
            tutorSurvey.T5 = int.Parse(r5);
            var r6 = fc.Get("r6");
            tutorSurvey.T6 = int.Parse(r6);
            var r7 = fc.Get("r7");
            tutorSurvey.T7 = int.Parse(r7);
            var r8 = fc.Get("r8");
            tutorSurvey.T8 = int.Parse(r8);
            var r9 = fc.Get("r9");
            tutorSurvey.TOverall = r9;

            var TutorComment = fc.Get("TutorComment");
            tutorSurvey.TutorId = newTutor;
            tutorSurvey.TComment = (TutorComment == "") ? string.Empty : TutorComment;

            //systems field
            tutorSurvey.CreatedBy = cur_user;
            tutorSurvey.ModifiedBy = cur_user;
            tutorSurvey.CreatedDateTime = DateTime.Now;
            tutorSurvey.ModifiedDateTime = DateTime.Now;

            _context.TutorSurveyModel.Add(tutorSurvey);

            _context.SaveChanges();

            return RedirectToAction("TutorSurvey").WithSuccess("Thank you, your survey has been submitted.");
        }
    }
}