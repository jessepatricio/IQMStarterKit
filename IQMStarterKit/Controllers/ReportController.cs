
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


            return View(modelList);
        }

        public ActionResult DOPE()
        {
            Session["email"] = null;

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


            return View(modelList);
        }

        public ActionResult DISC()
        {

            Session["email"] = null;

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


            return View(modelList);
        }

        public ActionResult MatchedWords()
        {

            Session["email"] = null;

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


            return View(modelList);
        }

        public ActionResult Top3Values()
        {

            Session["email"] = null;

            // activity 19 Personal Values
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


            return View(modelList);
        }


        public ActionResult StudentActivityPercentage()
        {
            Session["email"] = null;
            // activity 9 is DISC
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

        //public ActionResult ExportToPDF(string reportName)
        //{

        //    var dataSrc = GetFormattedReportDataSource(reportName);

        //    var sffx = DateTime.Now.ToString("ddMMyyyyHHmm");
        //    var fileName = reportName + "_" + sffx + ".pdf";

        //    //Create a dummy GridView
        //    GridView gv = new GridView();
        //    gv.AllowPaging = false;
        //    gv.DataSource = dataSrc;
        //    gv.DataBind();

        //    //design gridview
        //    gv = FormatGridView(gv);

        //    // return new RazorPDF.PdfResult(gv, "Index");

        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);

        //    Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter hw = new HtmlTextWriter(sw);
        //    gv.RenderControl(hw);
        //    StringReader sr = new StringReader(sw.ToString());
        //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        //    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //    PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //    pdfDoc.Open();
        //    htmlparser.Parse(sr);
        //    pdfDoc.Close();
        //    Response.Write(pdfDoc);
        //    Response.End();

        //    return View();
        //}

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
                    dataSrc.Columns.Remove("DopeResult");
                    dataSrc.Columns.Remove("DiscResult");
                    dataSrc.Columns.Remove("TempActivityId");
                    dataSrc.Columns.Remove("CreatedBy");
                    break;
                case "DopeReport":
                    dataSrc = DataLayer.GetActivityResult(8);
                    //remove some columns
                    dataSrc.Columns.Remove("GroupId");
                    dataSrc.Columns.Remove("VarkResult");
                    dataSrc.Columns.Remove("DiscResult");
                    dataSrc.Columns.Remove("TempActivityId");
                    dataSrc.Columns.Remove("CreatedBy");
                    break;
                case "DiscReport":
                    dataSrc = DataLayer.GetActivityResult(9);
                    //remove some columns
                    dataSrc.Columns.Remove("GroupId");
                    dataSrc.Columns.Remove("VarkResult");
                    dataSrc.Columns.Remove("DopeResult");
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



        public ActionResult DISCExportToPDF(string reportName)
        {

            Session["email"] = null;

            var sffx = DateTime.Now.ToString("ddMMyyyyHHmm");
            var fileName = reportName + "_" + sffx + ".pdf";


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

            return new ViewAsPdf("DISC", modelList) { FileName = fileName };

        }

        public ActionResult DOPEExportToPDF(string reportName)
        {

            Session["email"] = null;

            var sffx = DateTime.Now.ToString("ddMMyyyyHHmm");
            var fileName = reportName + "_" + sffx + ".pdf";


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

            return new ViewAsPdf("DOPE", modelList) { FileName = fileName };

        }

        public ActionResult VARKExportToPDF(string reportName)
        {

            Session["email"] = null;

            var sffx = DateTime.Now.ToString("ddMMyyyyHHmm");
            var fileName = reportName + "_" + sffx + ".pdf";


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


            return new ViewAsPdf("VARK", modelList) { FileName = fileName };

        }





    }
}