using System.Collections.Generic;

namespace IQMStarterKit.Models
{
    public class GroupTutorModelView
    {
        public GroupModel GroupModel { get; set; }
        public IEnumerable<GroupTutorModel> GroupTutorModel { get; set; }
        public IEnumerable<ApplicationUser> Tutors { get; set; }
    }
}