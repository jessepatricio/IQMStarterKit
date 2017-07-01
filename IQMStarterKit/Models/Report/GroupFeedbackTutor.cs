using System.Collections.Generic;

namespace IQMStarterKit.Models.Report
{
    public class GroupFeedbackTutor
    {
        public IEnumerable<TutorRating> TutorRatings { get; set; }
        public IEnumerable<GroupTutorComment> GroupTutorComments { get; set; }
    }

    public class TutorRating
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public string TutorId { get; set; }
        public string FullName { get; set; }
        public int GroupAveRating { get; set; }
        public int OverallRating { get; set; }

    }

    public class GroupTutorComment
    {
        public string FullName { get; set; }
        public string TutorName { get; set; }
        public string Comment { get; set; }
    }


}