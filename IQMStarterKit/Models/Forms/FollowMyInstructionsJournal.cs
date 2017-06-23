using IQMStarterKit.Models.Core;
using System.ComponentModel.DataAnnotations;

namespace IQMStarterKit.Models.Forms
{
    //page53
    public class FollowMyInstructionsJournalClass
    {
        [DataType(DataType.MultilineText)]
        public string FollowInstructionsJournalAns1 { get; set; }
        public string FollowInstructionsJournalAns2a { get; set; }
        public string FollowInstructionsJournalAns2b { get; set; }
        public string FollowInstructionsJournalAns2c { get; set; }
        public string FollowInstructionsJournalAns3a { get; set; }
        public string FollowInstructionsJournalAns3b { get; set; }
        public string FollowInstructionsJournalAns3c { get; set; }
        public string FollowInstructionsJournalAns4 { get; set; }

        public StudentActivity StudentActivity { get; set; }

        public FollowMyInstructionsJournalClass()
        {
            StudentActivity = new StudentActivity();
        }
    }
}