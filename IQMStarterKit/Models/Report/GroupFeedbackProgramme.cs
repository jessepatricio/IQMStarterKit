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
        public float GroupAveRating { get; set; }
        public float OverallRating { get; set; }
        public float TimeAllocatedRating { get; set; }
        public float ClassSizeRating { get; set; }
        public float ClassRoomRating { get; set; }
    }

    public class GroupComment
    {
        public string FullName { get; set; }
        public string Comment { get; set; }
    }


    public class GroupRatingJson
    {

        public string GroupName { get; set; }
        public float GroupAveRating { get; set; }

    }
}