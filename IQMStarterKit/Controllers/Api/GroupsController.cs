using IQMStarterKit.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace IQMStarterKit.Controllers.Api
{
    public class GroupsController : ApiController
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        // GET /api/groups
        public IEnumerable<GroupModel> GetGroup()
        {
            return _context.GroupModels.Where(m => m.IsRemoved == false).ToList();
        }

        // GET /api/groups/1
        public GroupModel GetGroup(int id)
        {
            var group = _context.GroupModels.SingleOrDefault(g => g.GroupId == id);

            if (group == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return group;

        }

        // POST /api/groups
        [HttpPost]
        public GroupModel CreateGroup(GroupModel group)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            _context.GroupModels.Add(group);
            _context.SaveChanges();

            return group;

        }

        // PUT /api/groups/1
        [HttpPut]
        public GroupModel UpdateGroup(int id, GroupModel group)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);


            var groupInDb = _context.GroupModels.SingleOrDefault(g => g.GroupId == id && g.IsRemoved != false);

            if (groupInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            groupInDb.GroupName = group.GroupName;
            groupInDb.Description = group.Description;
            groupInDb.TutorId = group.TutorId;

            groupInDb.ModifiedBy = User.Identity.GetUserId();
            groupInDb.ModifiedDateTime = DateTime.Now;

            _context.SaveChanges();

            return group;

        }


    }
}