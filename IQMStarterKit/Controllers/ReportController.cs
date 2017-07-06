
using IQMStarterKit.DAL;
using IQMStarterKit.Models;
using IQMStarterKit.Models.Report;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IQMStarterKit.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        #region Report Call

        public ActionResult VARK()
        {
            Session["email"] = null;
            var modelList = GetVarkModel();
            return View(modelList);
        }

        public ActionResult VARKExportToPDF(string reportName)
        {

            Session["email"] = null;

            var sffx = DateTime.Now.ToString("ddMMyyyyHHmm");
            var fileName = reportName + "_" + sffx + ".pdf";

            var modelList = GetVarkModel();

            return new ViewAsPdf("VARK", modelList) { FileName = fileName };

        }

        public ActionResult DISC()
        {

            Session["email"] = null;
            var modelList = GetDiscModel();

            return View(modelList);
        }

        public ActionResult DISCExportToPDF(string reportName)
        {

            Session["email"] = null;

            var sffx = DateTime.Now.ToString("ddMMyyyyHHmm");
            var fileName = reportName + "_" + sffx + ".pdf";
            var modelList = GetDiscModel();

            return new ViewAsPdf("DISC", modelList) { FileName = fileName };

        }

        public ActionResult DOPE()
        {
            Session["email"] = null;
            var modelList = GetDopeModel();

            return View(modelList);
        }

        public ActionResult DOPEExportToPDF(string reportName)
        {

            Session["email"] = null;

            var sffx = DateTime.Now.ToString("ddMMyyyyHHmm");
            var fileName = reportName + "_" + sffx + ".pdf";
            var modelList = GetDopeModel();

            return new ViewAsPdf("DOPE", modelList) { FileName = fileName };

        }

        public ActionResult MatchedWords()
        {

            Session["email"] = null;
            var modelList = GetMatchedWordViewModel();

            return View(modelList);
        }

        public ActionResult MatchedWordsExportToPDF(string reportName)
        {

            Session["email"] = null;

            var sffx = DateTime.Now.ToString("ddMMyyyyHHmm");
            var fileName = reportName + "_" + sffx + ".pdf";
            var modelList = GetMatchedWordViewModel();

            return new ViewAsPdf("MatchedWords", modelList) { FileName = fileName };

        }

        public ActionResult Top3Values()
        {

            Session["email"] = null;
            var modelList = GetTop3ValuesViewModel();

            return View(modelList);
        }

        public ActionResult Top3ValuesExportToPDF(string reportName)
        {

            Session["email"] = null;

            var sffx = DateTime.Now.ToString("ddMMyyyyHHmm");
            var fileName = reportName + "_" + sffx + ".pdf";
            var modelList = GetTop3ValuesViewModel();

            return new ViewAsPdf("Top3Values", modelList) { FileName = fileName };

        }

        public ActionResult PersonalLeaderShip()
        {

            Session["email"] = null;
            var modelList = GetPersonalLeadership();

            return View(modelList);
        }

        public ActionResult PersonalLeadershipExportToPDF(string reportName)
        {

            Session["email"] = null;

            var sffx = DateTime.Now.ToString("ddMMyyyyHHmm");
            var fileName = reportName + "_" + sffx + ".pdf";
            var modelList = GetPersonalLeadership();

            return new ViewAsPdf("PersonalLeadership", modelList) { FileName = fileName };

        }

        public ActionResult SelfManagement()
        {

            Session["email"] = null;
            var modelList = GetSelfManagement();

            return View(modelList);
        }

        public ActionResult SelfManagementExportToPDF(string reportName)
        {

            Session["email"] = null;

            var sffx = DateTime.Now.ToString("ddMMyyyyHHmm");
            var fileName = reportName + "_" + sffx + ".pdf";
            var modelList = GetSelfManagement();

            return new ViewAsPdf("SelfManagement", modelList) { FileName = fileName };

        }

        public ActionResult Assertiveness()
        {

            Session["email"] = null;
            var modelList = GetAssertiveness();

            return View(modelList);
        }

        public ActionResult AssertivenesstExportToPDF(string reportName)
        {

            Session["email"] = null;

            var sffx = DateTime.Now.ToString("ddMMyyyyHHmm");
            var fileName = reportName + "_" + sffx + ".pdf";
            var modelList = GetAssertiveness();

            return new ViewAsPdf("Assertiveness", modelList) { FileName = fileName };

        }

        public ActionResult ConflictManagement()
        {

            Session["email"] = null;
            var modelList = GetConflictManagement();

            return View(modelList);
        }

        public ActionResult ConflictManagementExportToPDF(string reportName)
        {

            Session["email"] = null;

            var sffx = DateTime.Now.ToString("ddMMyyyyHHmm");
            var fileName = reportName + "_" + sffx + ".pdf";
            var modelList = GetConflictManagement();

            return new ViewAsPdf("ConflictManagement", modelList) { FileName = fileName };

        }

        public ActionResult ReviewQuiz()
        {
            Session["email"] = null;
            var modelList = GetReviewQuiz();

            return View(modelList);
        }

        public ActionResult ReviewQuizExportToPDF(string reportName)
        {

            Session["email"] = null;

            var sffx = DateTime.Now.ToString("ddMMyyyyHHmm");
            var fileName = reportName + "_" + sffx + ".pdf";
            var modelList = GetReviewQuiz();

            return new ViewAsPdf("ReviewQuiz", modelList) { FileName = fileName };

        }




        //progress

        public ActionResult StudentActivityPercentage()
        {
            Session["email"] = null;

            var dt = DataLayer.GetActivityPercentage();
            var modelList = new List<StudentActivityPercentage>();


            foreach (DataRow item in dt.Rows)
            {
                var model = new StudentActivityPercentage
                {

                    UserId = item["UserId"].ToString(),
                    FullName = item["StudentName"].ToString(),
                    GroupName = item["GroupName"].ToString(),
                    StudentTotalActivities = item["StudentTotalActivities"].ToString(),
                    TotalActivities = item["TotalActivities"].ToString(),
                    ProgressValue = item["PercentageCompletion"].ToString()

                };

                modelList.Add(model);

            }


            return View(modelList);

        }

        public ActionResult GroupActivityPercentage()
        {
            Session["email"] = null;

            var dt = DataLayer.GetGroupActivityPercentage();
            var modelList = new List<GroupActivityPercentage>();


            foreach (DataRow item in dt.Rows)
            {
                var model = new GroupActivityPercentage
                {
                    GroupName = item["GroupName"].ToString(),
                    ProgressValue = item["PercentageCompletion"].ToString()

                };

                modelList.Add(model);

            }


            return View(modelList);

        }

        //surveys

        public ActionResult GroupFeedbackProgramme()
        {
            Session["email"] = null;

            //get rating

            var dt = DataLayer.GetGroupFeedbackProgramme();
            var modelList = new List<GroupRating>();


            foreach (DataRow item in dt.Rows)
            {
                var model = new GroupRating
                {

                    GroupId = item["GroupId"].ToString(),
                    GroupName = item["GroupName"].ToString(),
                    GroupAveRating = int.Parse(item["GroupAverageRating"].ToString()),
                    OverallRating = int.Parse(item["OverallRating"].ToString()),
                    TimeAllocatedRating = int.Parse(item["TimeAllocatedRating"].ToString()),
                    ClassSizeRating = int.Parse(item["ClassSizeRating"].ToString()),
                    ClassRoomRating = int.Parse(item["ClassRoomRating"].ToString())
                };

                modelList.Add(model);

            }


            //get comments
            dt = DataLayer.GetGroupFeedbackComments();
            var modelComment = new List<GroupComment>();


            foreach (DataRow item in dt.Rows)
            {
                var _model = new GroupComment
                {

                    FullName = item["FullName"].ToString(),
                    Comment = item["PComment"].ToString(),

                };

                modelComment.Add(_model);

            }

            var progmodel = new GroupFeedbackProgramme();
            progmodel.GroupRatings = modelList;
            progmodel.GroupComments = modelComment;


            return View(progmodel);

        }

        public GroupFeedbackProgramme ExportGroupFeedbackProgramme()
        {
            Session["email"] = null;

            //get rating

            var dt = DataLayer.GetGroupFeedbackProgramme();
            var modelList = new List<GroupRating>();


            foreach (DataRow item in dt.Rows)
            {
                var model = new GroupRating
                {

                    GroupId = item["GroupId"].ToString(),
                    GroupName = item["GroupName"].ToString(),
                    GroupAveRating = int.Parse(item["GroupAverageRating"].ToString()),
                    OverallRating = int.Parse(item["OverallRating"].ToString()),
                    TimeAllocatedRating = int.Parse(item["TimeAllocatedRating"].ToString()),
                    ClassSizeRating = int.Parse(item["ClassSizeRating"].ToString()),
                    ClassRoomRating = int.Parse(item["ClassRoomRating"].ToString())
                };

                modelList.Add(model);

            }


            //get comments
            dt = DataLayer.GetGroupFeedbackComments();
            var modelComment = new List<GroupComment>();


            foreach (DataRow item in dt.Rows)
            {
                var _model = new GroupComment
                {

                    FullName = item["FullName"].ToString(),
                    Comment = item["PComment"].ToString(),

                };

                modelComment.Add(_model);

            }

            var progmodel = new GroupFeedbackProgramme();
            progmodel.GroupRatings = modelList;
            progmodel.GroupComments = modelComment;


            return progmodel;

        }

        public ActionResult ProgramSurveyExportToPDF(string reportName)
        {

            Session["email"] = null;

            var sffx = DateTime.Now.ToString("ddMMyyyyHHmm");
            var fileName = reportName + "_" + sffx + ".pdf";
            var modelList = ExportGroupFeedbackProgramme();


            return new ViewAsPdf("GroupFeedbackProgramme", modelList) { FileName = fileName };

        }

        public ActionResult GroupFeedbackTutor()
        {
            Session["email"] = null;

            //get rating

            var dt = DataLayer.GetGroupFeedbackTutor();
            var modelList = new List<TutorRating>();


            foreach (DataRow item in dt.Rows)
            {
                var model = new TutorRating
                {

                    GroupId = item["GroupId"].ToString(),
                    GroupName = item["GroupName"].ToString(),
                    TutorId = item["TutorId"].ToString(),
                    FullName = item["FullName"].ToString(),
                    GroupAveRating = int.Parse(item["GroupAverageRating"].ToString()),
                    OverallRating = int.Parse(item["OverallRating"].ToString()),

                };

                modelList.Add(model);

            }


            //get comments
            dt = DataLayer.GetGroupFeedbackTutorComments();
            var modelComment = new List<GroupTutorComment>();


            foreach (DataRow item in dt.Rows)
            {
                var _model = new GroupTutorComment
                {

                    FullName = item["FullName"].ToString(),
                    TutorName = item["TutorName"].ToString(),
                    Comment = item["TComment"].ToString(),

                };

                modelComment.Add(_model);

            }

            var progmodel = new GroupFeedbackTutor();
            progmodel.TutorRatings = modelList;
            progmodel.GroupTutorComments = modelComment;


            return View(progmodel);

        }

        public GroupFeedbackTutor ExportGroupFeedbackTutor()
        {
            Session["email"] = null;

            //get rating

            var dt = DataLayer.GetGroupFeedbackTutor();
            var modelList = new List<TutorRating>();


            foreach (DataRow item in dt.Rows)
            {
                var model = new TutorRating
                {

                    GroupId = item["GroupId"].ToString(),
                    GroupName = item["GroupName"].ToString(),
                    TutorId = item["TutorId"].ToString(),
                    FullName = item["FullName"].ToString(),
                    GroupAveRating = int.Parse(item["GroupAverageRating"].ToString()),
                    OverallRating = int.Parse(item["OverallRating"].ToString()),

                };

                modelList.Add(model);

            }


            //get comments
            dt = DataLayer.GetGroupFeedbackTutorComments();
            var modelComment = new List<GroupTutorComment>();


            foreach (DataRow item in dt.Rows)
            {
                var _model = new GroupTutorComment
                {

                    FullName = item["FullName"].ToString(),
                    TutorName = item["TutorName"].ToString(),
                    Comment = item["TComment"].ToString(),

                };

                modelComment.Add(_model);

            }

            var progmodel = new GroupFeedbackTutor();
            progmodel.TutorRatings = modelList;
            progmodel.GroupTutorComments = modelComment;


            return progmodel;

        }

        public ActionResult TutorSurveyExportToPDF(string reportName)
        {

            Session["email"] = null;

            var sffx = DateTime.Now.ToString("ddMMyyyyHHmm");
            var fileName = reportName + "_" + sffx + ".pdf";
            var modelList = ExportGroupFeedbackTutor();

            return new ViewAsPdf("GroupFeedbackTutor", modelList) { FileName = fileName };

        }


        #endregion


        #region Report Generation

        public ActionResult ExportToExcel(string reportName)
        {
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            var dataSrc = GetFormattedReportDataSource(reportName);

            var sffx = DateTime.Now.ToString("ddMMyyyyHHmm");
            var fileName = reportName + "_" + sffx + ".xls";


            var gv = new GridView();
            gv.DataSource = dataSrc;
            gv.DataBind();

            //design gridview
            gv = FormatGridView(gv);



            //style to format numbers to string
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";

            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();

            return View();

        }

        public DataTable GetFormattedReportDataSource(string reportName)
        {
            DataTable dataSrc = new DataTable();

            //get data
            switch (reportName)
            {
                case "VarkReport":
                    dataSrc = DataLayer.GetActivityResult(5);
                    //remove some columns
                    dataSrc.Columns.Remove("GroupId");
                    dataSrc.Columns.Remove("TempActivityId");
                    dataSrc.Columns.Remove("CreatedBy");
                    break;
                case "DopeReport":
                    dataSrc = DataLayer.GetActivityResult(8);
                    //remove some columns
                    dataSrc.Columns.Remove("GroupId");
                    dataSrc.Columns.Remove("TempActivityId");
                    dataSrc.Columns.Remove("CreatedBy");
                    break;
                case "DiscReport":
                    dataSrc = DataLayer.GetActivityResult(9);
                    //remove some columns
                    dataSrc.Columns.Remove("GroupId");
                    dataSrc.Columns.Remove("TempActivityId");
                    dataSrc.Columns.Remove("CreatedBy");
                    break;
                case "MatchingWordReport":
                    dataSrc = DataLayer.GetActivityResult(13);
                    //remove some columns
                    dataSrc.Columns.Remove("GroupId");
                    dataSrc.Columns.Remove("TempActivityId");
                    dataSrc.Columns.Remove("CreatedBy");
                    break;
                case "Top3ValuesReport":
                    dataSrc = DataLayer.GetActivityResult(19);
                    //remove some columns
                    dataSrc.Columns.Remove("GroupId");
                    dataSrc.Columns.Remove("TempActivityId");
                    dataSrc.Columns.Remove("CreatedBy");
                    break;

                case "PersonalLeadershipReport":
                    dataSrc = DataLayer.GetActivityResult(20);
                    //remove some columns
                    dataSrc.Columns.Remove("GroupId");
                    dataSrc.Columns.Remove("TempActivityId");
                    dataSrc.Columns.Remove("CreatedBy");
                    break;

                case "SelfManagementReport":
                    dataSrc = DataLayer.GetActivityResult(24);
                    //remove some columns
                    dataSrc.Columns.Remove("GroupId");
                    dataSrc.Columns.Remove("TempActivityId");
                    dataSrc.Columns.Remove("CreatedBy");
                    break;


                case "AssertivenessReport":
                    dataSrc = DataLayer.GetActivityResult(30);
                    //remove some columns
                    dataSrc.Columns.Remove("GroupId");
                    dataSrc.Columns.Remove("TempActivityId");
                    dataSrc.Columns.Remove("CreatedBy");
                    break;

                case "ConflictManagementReport":
                    dataSrc = DataLayer.GetActivityResult(41);
                    //remove some columns
                    dataSrc.Columns.Remove("GroupId");
                    dataSrc.Columns.Remove("TempActivityId");
                    dataSrc.Columns.Remove("CreatedBy");
                    break;

                case "ActivityPercentageReport":
                    dataSrc = DataLayer.GetActivityPercentage();
                    //remove some column
                    dataSrc.Columns.Remove("UserId");
                    //rename some columns
                    dataSrc.Columns["StudentTotalActivities"].ColumnName = "Activities Completed";
                    dataSrc.Columns["TotalActivities"].ColumnName = "Total Activities";
                    dataSrc.Columns["PercentageCompletion"].ColumnName = "% Completed";
                    break;

                case "ProgramSurveyReport":
                    dataSrc = DataLayer.GetGroupFeedbackProgramme();

                    //remove some column
                    dataSrc.Columns.Remove("GroupId");
                    break;

                case "TutorSurveyReport":
                    dataSrc = DataLayer.GetGroupFeedbackTutor();

                    //remove some column
                    dataSrc.Columns.Remove("GroupId");
                    dataSrc.Columns.Remove("TutorId");
                    break;

            }

            return dataSrc;
        }

        public GridView FormatGridView(GridView gv)
        {
            //Change the Header Row back to white color
            gv.HeaderRow.Style.Add("background-color", "#FFFFFF");

            //Apply style to Individual Cells
            for (int i = 0; i < gv.HeaderRow.Cells.Count; i++)
            {
                gv.HeaderRow.Cells[i].Style.Add("background-color", "#5DADE2");
            }


            for (int i = 0; i < gv.Rows.Count; i++)
            {
                GridViewRow row = gv.Rows[i];

                //Change Color back to white
                row.BackColor = System.Drawing.Color.White;

                //Apply text style to each Row
                row.Attributes.Add("class", "textmode");

                //Apply style to Individual Cells of Alternating Row
                if (i % 2 != 0)
                {
                    for (int j = 0; i < row.Cells.Count; i++)
                    {
                        row.Cells[j].Style.Add("backround-color", "#AED6F1");
                    }

                }
            }




            return gv;
        }

        #endregion








        public ActionResult PercentageExportToPDF(string reportName)
        {

            Session["email"] = null;

            var sffx = DateTime.Now.ToString("ddMMyyyyHHmm");
            var fileName = reportName + "_" + sffx + ".pdf";



            var dt = DataLayer.GetActivityPercentage();
            var modelList = new List<StudentActivityPercentage>();


            foreach (DataRow item in dt.Rows)
            {
                var model = new StudentActivityPercentage
                {

                    UserId = item["UserId"].ToString(),
                    FullName = item["StudentName"].ToString(),
                    GroupName = item["GroupName"].ToString(),
                    StudentTotalActivities = item["StudentTotalActivities"].ToString(),
                    TotalActivities = item["TotalActivities"].ToString(),
                    ProgressValue = item["PercentageCompletion"].ToString()

                };

                modelList.Add(model);

            }





            return new ViewAsPdf("StudentActivityPercentage", modelList) { FileName = fileName };

        }


        // logic
        public List<VarkViewModel> GetVarkModel()
        {
            // activity 5 is VARK
            var dt = DataLayer.GetActivityResult(5);
            var modelList = new List<VarkViewModel>();


            foreach (DataRow item in dt.Rows)
            {
                var model = new VarkViewModel
                {

                    TempActivityId = item["TempActivityId"].ToString(),
                    VarkResult = item["VarkResult"].ToString(),
                    CreatedBy = item["CreatedBy"].ToString(),
                    CreatedDateTime = item["CreatedDateTime"].ToString(),
                    GroupId = item["GroupId"].ToString(),
                    FullName = item["StudentName"].ToString(),
                    GroupName = item["GroupName"].ToString()
                };

                modelList.Add(model);

            }

            return modelList;
        }

        public List<DiscViewModel> GetDiscModel()
        {
            // activity 9 is DISC
            var dt = DataLayer.GetActivityResult(9);
            var modelList = new List<DiscViewModel>();


            foreach (DataRow item in dt.Rows)
            {
                var model = new DiscViewModel
                {

                    TempActivityId = item["TempActivityId"].ToString(),
                    DiscResult = item["DiscResult"].ToString(),
                    CreatedBy = item["CreatedBy"].ToString(),
                    CreatedDateTime = item["CreatedDateTime"].ToString(),
                    GroupId = item["GroupId"].ToString(),
                    FullName = item["StudentName"].ToString(),
                    GroupName = item["GroupName"].ToString()
                };

                modelList.Add(model);

            }

            return modelList;
        }

        public List<DopeViewModel> GetDopeModel()
        {
            // activity 8 is DOPE
            var dt = DataLayer.GetActivityResult(8);
            var modelList = new List<DopeViewModel>();


            foreach (DataRow item in dt.Rows)
            {
                var model = new DopeViewModel
                {

                    TempActivityId = item["TempActivityId"].ToString(),
                    DopeResult = item["DopeResult"].ToString(),
                    CreatedBy = item["CreatedBy"].ToString(),
                    CreatedDateTime = item["CreatedDateTime"].ToString(),
                    GroupId = item["GroupId"].ToString(),
                    FullName = item["StudentName"].ToString(),
                    GroupName = item["GroupName"].ToString()
                };

                modelList.Add(model);

            }

            return modelList;
        }

        public List<MatchedWordViewModel> GetMatchedWordViewModel()
        {
            // activity 13 NZ Slang Matching Words
            var dt = DataLayer.GetActivityResult(13);
            var modelList = new List<MatchedWordViewModel>();


            foreach (DataRow item in dt.Rows)
            {
                var model = new MatchedWordViewModel
                {

                    TempActivityId = item["TempActivityId"].ToString(),
                    NoMatchedWords = item["NoMatchedWords"].ToString(),
                    CreatedBy = item["CreatedBy"].ToString(),
                    CreatedDateTime = item["CreatedDateTime"].ToString(),
                    GroupId = item["GroupId"].ToString(),
                    FullName = item["StudentName"].ToString(),
                    GroupName = item["GroupName"].ToString()
                };

                modelList.Add(model);

            }

            return modelList;
        }

        public List<Top3ValuesViewModel> GetTop3ValuesViewModel()
        {
            // activity 19 top 3 values
            var dt = DataLayer.GetActivityResult(19);
            var modelList = new List<Top3ValuesViewModel>();


            foreach (DataRow item in dt.Rows)
            {
                var model = new Top3ValuesViewModel
                {

                    TempActivityId = item["TempActivityId"].ToString(),
                    Top3PersonalValues = item["Top3PersonalValues"].ToString(),
                    CreatedBy = item["CreatedBy"].ToString(),
                    CreatedDateTime = item["CreatedDateTime"].ToString(),
                    GroupId = item["GroupId"].ToString(),
                    FullName = item["StudentName"].ToString(),
                    GroupName = item["GroupName"].ToString()
                };

                modelList.Add(model);

            }

            return modelList;
        }

        public List<PersonalLeadershipViewModel> GetPersonalLeadership()
        {
            // activity 19 Personal Leadership
            var dt = DataLayer.GetActivityResult(20);
            var modelList = new List<PersonalLeadershipViewModel>();


            foreach (DataRow item in dt.Rows)
            {
                var model = new PersonalLeadershipViewModel
                {

                    TempActivityId = item["TempActivityId"].ToString(),
                    PersonalLeadershipScore = item["PersonalLeadershipScore"].ToString(),
                    CreatedBy = item["CreatedBy"].ToString(),
                    CreatedDateTime = item["CreatedDateTime"].ToString(),
                    GroupId = item["GroupId"].ToString(),
                    FullName = item["StudentName"].ToString(),
                    GroupName = item["GroupName"].ToString()
                };

                modelList.Add(model);

            }

            return modelList;
        }

        public List<SelfManagementViewModel> GetSelfManagement()
        {
            // activity 24 self management
            var dt = DataLayer.GetActivityResult(24);
            var modelList = new List<SelfManagementViewModel>();


            foreach (DataRow item in dt.Rows)
            {
                var model = new SelfManagementViewModel
                {

                    TempActivityId = item["TempActivityId"].ToString(),
                    SelfManagementScore = item["SelfManagementScore"].ToString(),
                    CreatedBy = item["CreatedBy"].ToString(),
                    CreatedDateTime = item["CreatedDateTime"].ToString(),
                    GroupId = item["GroupId"].ToString(),
                    FullName = item["StudentName"].ToString(),
                    GroupName = item["GroupName"].ToString()
                };

                modelList.Add(model);

            }

            return modelList;
        }

        public List<AssertivenessViewModel> GetAssertiveness()
        {
            // activity 30 assertiveness
            var dt = DataLayer.GetActivityResult(30);
            var modelList = new List<AssertivenessViewModel>();


            foreach (DataRow item in dt.Rows)
            {
                var model = new AssertivenessViewModel
                {

                    TempActivityId = item["TempActivityId"].ToString(),
                    AssertiveScore = item["AssertiveScore"].ToString(),
                    CreatedBy = item["CreatedBy"].ToString(),
                    CreatedDateTime = item["CreatedDateTime"].ToString(),
                    GroupId = item["GroupId"].ToString(),
                    FullName = item["StudentName"].ToString(),
                    GroupName = item["GroupName"].ToString()
                };

                modelList.Add(model);

            }

            return modelList;
        }

        public List<ConflictManagementViewModel> GetConflictManagement()
        {
            // activity 41 conflict management
            var dt = DataLayer.GetActivityResult(41);
            var modelList = new List<ConflictManagementViewModel>();


            foreach (DataRow item in dt.Rows)
            {
                var model = new ConflictManagementViewModel
                {

                    TempActivityId = item["TempActivityId"].ToString(),
                    CFDominantFirst = item["CFDominantFirst"].ToString(),
                    CFDominantSecond = item["CFDominantSecond"].ToString(),
                    CreatedBy = item["CreatedBy"].ToString(),
                    CreatedDateTime = item["CreatedDateTime"].ToString(),
                    GroupId = item["GroupId"].ToString(),
                    FullName = item["StudentName"].ToString(),
                    GroupName = item["GroupName"].ToString()
                };

                modelList.Add(model);

            }

            return modelList;
        }

        public List<ReviewQuizViewModel> GetReviewQuiz()
        {
            // activity 48 review quiz
            var dt = DataLayer.GetActivityResult(48);
            var modelList = new List<ReviewQuizViewModel>();


            foreach (DataRow item in dt.Rows)
            {
                var model = new ReviewQuizViewModel
                {

                    TempActivityId = item["TempActivityId"].ToString(),
                    ReviewQuizScore = item["ReviewQuizScore"].ToString(),
                    CreatedBy = item["CreatedBy"].ToString(),
                    CreatedDateTime = item["CreatedDateTime"].ToString(),
                    GroupId = item["GroupId"].ToString(),
                    FullName = item["StudentName"].ToString(),
                    GroupName = item["GroupName"].ToString()
                };

                modelList.Add(model);

            }

            return modelList;
        }


    }
}