using IQMStarterKit.Models;
using IQMStarterKit.Models.Alert;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace IQMStarterKit.Controllers
{
    public class GroupTutorModelsController : CommonController
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        // GET: GroupTutorModels
        public ActionResult Index(byte Id)
        {
            var model = new GroupTutorModelView();
            //get group info
            model.GroupModel = _context.GroupModels.FirstOrDefault(m => m.GroupId == Id);
            model.GroupTutorModel = _context.GroupTutorModels.Where(m => m.GroupId == Id).ToList();
            model.Tutors = GetTutors(Id);

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTutor(GroupTutorModelView groupTutorView)
        {
            var groupTutor = new GroupTutorModel();
            string newTutor = Convert.ToString(Request.Form["AddTutor"]);

            if (newTutor == null) return RedirectToAction("Index", new { Id = groupTutorView.GroupModel.GroupId }).WithWarning("No tutor to add");

            try
            {
                groupTutor.CreatedBy = GetSessionUserId();
                groupTutor.CreatedDateTime = DateTime.Now;
                groupTutor.ModifiedBy = GetSessionUserId();
                groupTutor.ModifiedDateTime = DateTime.Now;

                groupTutor.GroupId = groupTutorView.GroupModel.GroupId;
                groupTutor.TutorId = newTutor;
                groupTutor.TutorName = GetFullName(newTutor);

                //save tutor
                _context.GroupTutorModels.Add(groupTutor);
                _context.SaveChanges();



                return RedirectToAction("Index", new { Id = groupTutorView.GroupModel.GroupId }).WithSuccess("Tutor added successfully!");

            }
            catch (Exception ex)
            {

                return View(groupTutorView).WithError("Insert Error: " + ex.Message);
            }




        }


        // Utility


        public ActionResult Delete(int id, string groupId)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupTutorModel groupModelTutor = _context.GroupTutorModels.Where(m => m.GroupTutorId == id).FirstOrDefault();
            if (groupModelTutor == null)
            {
                return HttpNotFound();
            }
            _context.GroupTutorModels.Remove(groupModelTutor);
            _context.SaveChanges();

            return RedirectToAction("Index", new { Id = groupId }).WithSuccess("Deleted successfully!");
        }


    }
}
