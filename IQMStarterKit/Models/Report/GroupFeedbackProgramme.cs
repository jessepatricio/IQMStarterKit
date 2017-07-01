using System.Collections.Generic;

namespace IQMStarterKit.Models.Report
{
    public class GroupFeedbackProgramme
    {
        public IEnumerable<GroupRating> GroupRatings { get; set; }
        public IEnumerable<GroupComment> GroupComments { get; set; }
    }

    public class GroupRating
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public int GroupAveRating { get; set; }
        public int OverallRating { get; set; }
        public int TimeAllocatedRating { get; set; }
        public int ClassSizeRating { get; set; }
        public int ClassRoomRating { get; set; }
    }

    public class GroupComment
    {
        public string FullName { get; set; }
        public string Comment { get; set; }
    }
}