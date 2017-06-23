using IQMStarterKit.Models.Core;
using System.ComponentModel.DataAnnotations;

namespace IQMStarterKit.Models.Forms
{
    //page50
    public class PassTheBallJournalClass
    {
        [DataType(DataType.MultilineText)]
        public string PassBallJournalAns1 { get; set; }
        public string PassBallJournalAns2a { get; set; }
        public string PassBallJournalAns2b { get; set; }
        public string PassBallJournalAns2c { get; set; }
        public string PassBallJournalAns3a { get; set; }
        public string PassBallJournalAns3b { get; set; }
        public string PassBallJournalAns3c { get; set; }
        public string PassBallJournalAns4 { get; set; }

        public StudentActivity StudentActivity { get; set; }

        public PassTheBallJournalClass()
        {
            StudentActivity = new StudentActivity();
        }
    }
}