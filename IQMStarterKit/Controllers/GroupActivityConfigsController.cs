using IQMStarterKit.Models;
using IQMStarterKit.Models.Alert;
using IQMStarterKit.Models.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace IQMStarterKit.Controllers
{
    [Authorize(Roles = "Administrator,Tutor")]
    public class GroupActivityConfigsController : CommonController
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        // GET: GroupActivityConfigs
        public ActionResult Index(string GroupId)
        {
            var configModel = new ConfigViewModel();
            var groupId = byte.Parse(GroupId);
            var tempConfigs = new List<TempActivityConfig>();
            // load target group to configure
            configModel.GroupModel = _context.GroupModels.Where(m => m.GroupId == groupId).FirstOrDefault();
            // load temp activities
            var tempActivities = _context.TempActivities.Where(m => m.IsRemoved == false).OrderBy(m => m.SortOrder).ToList();

            var groupConfig = _context.GroupActivityConfig.Where(m => m.GroupId == groupId).ToList();


            foreach (var item in tempActivities)
            {
                var newActivityConfig = new TempActivityConfig();
                //newActivityConfig
                newActivityConfig.TempActivityId = item.TempActivityId;
                newActivityConfig.TempModuleId = item.TempModuleId;
                newActivityConfig.Title = item.Title;
                newActivityConfig.Description = item.Description;
                newActivityConfig.SortOrder = item.SortOrder;

                var _config = groupConfig.Where(m => m.GroupId == groupId && m.TempActivityId == item.TempActivityId).FirstOrDefault();

                if (_config != null)
                {
                    newActivityConfig.GroupActivityConfigId = _config.GroupActivityConfigId;
                    newActivityConfig.GroupId = _config.GroupId;
                    newActivityConfig.IsLocked = _config.IsLocked;
                    newActivityConfig.Remarks = _config.Remarks;
                    newActivityConfig.CreatedBy = GetFullName(_config.CreatedBy);
                    newActivityConfig.CreatedDateTime = _config.CreatedDateTime;
                    newActivityConfig.ModifiedBy = GetFullName(_config.ModifiedBy);
                    newActivityConfig.ModifiedDateTime = _config.ModifiedDateTime;


                }

                tempConfigs.Add(newActivityConfig);

            }


            configModel.TempActivitiesConfig = tempConfigs.OrderBy(m => m.SortOrder).ToList();


            return View(configModel);
        }

        // UPDATE
        [HttpPost]
        public ActionResult SaveConfig(FormCollection fc)
        {
            var tempConfigs = new List<TempActivityConfig>();
            var groupId = Request.Form["GroupId"];
            var byteGroupId = byte.Parse(groupId);
            bool updateFlag = false;


            // load temp activities
            var tempActivities = _context.TempActivities.Where(m => m.IsRemoved == false).ToList();

            var groupConfig = _context.GroupActivityConfig.Where(m => m.GroupId == byteGroupId).ToList();


            foreach (var item in tempActivities)
            {
                var newActivityConfig = new TempActivityConfig();
                bool locker = false;

                newActivityConfig.GroupId = byteGroupId;
                newActivityConfig.TempActivityId = item.TempActivityId;
                newActivityConfig.TempModuleId = item.TempModuleId;
                newActivityConfig.Title = item.Title;
                newActivityConfig.Description = item.Description;
                newActivityConfig.SortOrder = item.SortOrder;

                var _config = groupConfig.Where(m => m.TempActivityId == item.TempActivityId).FirstOrDefault();

                var new_lock = Request.Form["lock" + item.TempActivityId];
                var new_remark = Request.Form["remark" + item.TempActivityId];


                if (new_lock != "false") locker = true;

                newActivityConfig.IsLocked = locker;
                newActivityConfig.Remarks = new_remark.Trim();

                if (_config != null)
                {
                    updateFlag = true;
                    newActivityConfig.GroupActivityConfigId = _config.GroupActivityConfigId;
                    newActivityConfig.GroupId = _config.GroupId;
                    newActivityConfig.IsLocked = (locker != _config.IsLocked) ? locker : _config.IsLocked;
                    newActivityConfig.Remarks = new_remark.Trim();
                    newActivityConfig.CreatedBy = _config.CreatedBy;
                    newActivityConfig.CreatedDateTime = _config.CreatedDateTime;
                    newActivityConfig.ModifiedBy = _config.ModifiedBy;
                    newActivityConfig.ModifiedDateTime = _config.ModifiedDateTime;


                }

                tempConfigs.Add(newActivityConfig);

            }


            foreach (var item in tempConfigs)
            {


                var model = _context.GroupActivityConfig.Where(m => m.GroupActivityConfigId == item.GroupActivityConfigId).FirstOrDefault();

                if (model != null)
                {
                    model.IsLocked = item.IsLocked;
                    model.Remarks = item.Remarks;
                    model.ModifiedBy = GetSessionUserId();
                    model.ModifiedDateTime = DateTime.Now;
                }
                else
                {
                    model = new GroupActivityConfig();

                    model.GroupId = item.GroupId;
                    model.TempActivityId = item.TempActivityId;
                    model.IsLocked = item.IsLocked;
                    model.Remarks = item.Remarks;

                    model.CreatedBy = (updateFlag) ? item.CreatedBy : GetSessionUserId();
                    model.CreatedDateTime = (updateFlag) ? item.CreatedDateTime : DateTime.Now;
                    model.ModifiedBy = GetSessionUserId();
                    model.ModifiedDateTime = DateTime.Now;
                }

                try
                {
                    if (updateFlag)
                    {
                        _context.Entry(model).State = EntityState.Modified;
                    }
                    else
                    {
                        _context.GroupActivityConfig.Add(model);
                    }

                    _context.SaveChanges();

                }
                catch (Exception ex)
                {

                    return RedirectToAction("Index", new { @GroupId = groupId }).WithError(ex.Message);
                }



            }

            return RedirectToAction("Index", new { @GroupId = groupId }).WithSuccess("Saved successfully!");
        }


    }
}
