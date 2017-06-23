using IQMStarterKit.Models.Core;
using System.ComponentModel.DataAnnotations;

namespace IQMStarterKit.Models.Forms
{
    //page48
    public class My14HabitsClass
    {
        [DataType(DataType.MultilineText)]
        public string Proactive1 { get; set; }
        public string Proactive2 { get; set; }
        public string EndMind1 { get; set; }
        public string EndMind2 { get; set; }
        public string FirstThings1 { get; set; }
        public string FirstThings2 { get; set; }
        public string WinWin1 { get; set; }
        public string WinWin2 { get; set; }
        public string SeekFirst1 { get; set; }
        public string SeekFirst2 { get; set; }
        public string Synergise1 { get; set; }
        public string Synergise2 { get; set; }
        public string SharpenSaw1 { get; set; }
        public string SharpenSaw2 { get; set; }

        public StudentActivity StudentActivity { get; set; }

        public My14HabitsClass()
        {
            StudentActivity = new StudentActivity();
        }

    }
}