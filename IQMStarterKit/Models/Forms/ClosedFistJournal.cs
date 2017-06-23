using IQMStarterKit.Models.Core;
using System.ComponentModel.DataAnnotations;

namespace IQMStarterKit.Models.Forms
{
    //page51
    public class ClosedFistJournalClass
    {
        [DataType(DataType.MultilineText)]
        public string ClosedFistJournalAns1 { get; set; }
        public string ClosedFistJournalAns2a { get; set; }
        public string ClosedFistJournalAns2b { get; set; }
        public string ClosedFistJournalAns2c { get; set; }
        public string ClosedFistJournalAns3a { get; set; }
        public string ClosedFistJournalAns3b { get; set; }
        public string ClosedFistJournalAns3c { get; set; }
        public string ClosedFistJournalAns4 { get; set; }

        public StudentActivity StudentActivity { get; set; }

        public ClosedFistJournalClass()
        {
            StudentActivity = new StudentActivity();
        }
    }
}