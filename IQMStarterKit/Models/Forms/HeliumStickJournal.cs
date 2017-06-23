using IQMStarterKit.Models.Core;
using System.ComponentModel.DataAnnotations;

namespace IQMStarterKit.Models.Forms
{
    //page52
    public class HeliumStickJournalClass
    {
        [DataType(DataType.MultilineText)]
        public string HeliumStickJournalAns1 { get; set; }
        public string HeliumStickJournalAns2a { get; set; }
        public string HeliumStickJournalAns2b { get; set; }
        public string HeliumStickJournalAns2c { get; set; }
        public string HeliumStickJournalAns3a { get; set; }
        public string HeliumStickJournalAns3b { get; set; }
        public string HeliumStickJournalAns3c { get; set; }
        public string HeliumStickJournalAns4 { get; set; }

        public StudentActivity StudentActivity { get; set; }

        public HeliumStickJournalClass()
        {
            StudentActivity = new StudentActivity();
        }
    }
}