using IQMStarterKit.Models.Core;

namespace IQMStarterKit.Models.Forms
{
    public class Vark
    {
        public bool Visual { get; set; }
        public bool Auditory { get; set; }
        public bool ReadingWriting { get; set; }
        public bool Kinesthetic { get; set; }

        public StudentActivity StudentActivity { get; set; }

        public Vark()
        {
            StudentActivity = new StudentActivity();

        }
    }
}