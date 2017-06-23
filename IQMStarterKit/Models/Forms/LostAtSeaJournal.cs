using IQMStarterKit.Models.Core;
using System.ComponentModel.DataAnnotations;

namespace IQMStarterKit.Models.Forms
{
    //page53
    public class LostAtSeaJournalClass
    {
        [DataType(DataType.MultilineText)]
        public string LostSeaJournalAns1 { get; set; }
        public string LostSeaJournalAns2 { get; set; }
        public string LostSeaJournalAns3 { get; set; }
        public string LostSeaJournalAns4 { get; set; }
        public string LostSeaJournalAns5 { get; set; }
        public string LostSeaJournalAns6 { get; set; }
    

        public StudentActivity StudentActivity { get; set; }

        public LostAtSeaJournalClass()
        {
            StudentActivity = new StudentActivity();
        }
    }
}