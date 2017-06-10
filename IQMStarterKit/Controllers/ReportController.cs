using IQMStarterKit.DAL;
using IQMStarterKit.Models;
using IQMStarterKit.Models.Report;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace IQMStarterKit.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult VARK()
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
                    FullName = item["FullName"].ToString(),
                    GroupName = item["GroupName"].ToString()
                };

                modelList.Add(model);

            }


            return View(modelList);
        }

        public ActionResult DOPE()
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
                    FullName = item["FullName"].ToString(),
                    GroupName = item["GroupName"].ToString()
                };

                modelList.Add(model);

            }


            return View(modelList);
        }

        public ActionResult DISC()
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
                    FullName = item["FullName"].ToString(),
                    GroupName = item["GroupName"].ToString()
                };

                modelList.Add(model);

            }


            return View(modelList);
        }

        public ActionResult StudentActivityPercentage()
        {

            // activity 9 is DISC
            var dt = DataLayer.GetActivityPercentage();
            var modelList = new List<StudentActivityPercentage>();


            foreach (DataRow item in dt.Rows)
            {
                double std = int.Parse(item["StudentTotalActivities"].ToString());
                double ttl = int.Parse(item["TotalActivities"].ToString());

                double pValue = System.Math.Round((std / ttl)*100,2);


                var model = new StudentActivityPercentage
                {

                    UserId = item["UserId"].ToString(),
                    FullName = item["FullName"].ToString(),
                    GroupName = item["GroupName"].ToString(),
                    StudentTotalActivities = item["StudentTotalActivities"].ToString(),
                    TotalActivities = item["TotalActivities"].ToString(),
                    ProgressValue = pValue

                };

                modelList.Add(model);

            }


            return View(modelList);





        }


    }
}