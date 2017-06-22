using IQMStarterKit.Models.Core;

namespace IQMStarterKit.Models.Forms
{
    public class cheese
    {
        public string answer1 { get; set; }
        public string answer2 { get; set; }
        public string answer3 { get; set; }
        public string answer4 { get; set; }
        public string answer5 { get; set; }
        public string answer6 { get; set; }
        public string answer7 { get; set; }

        public StudentActivity StudentActivity { get; set; }

        public cheese()
        {
            StudentActivity = new StudentActivity();
        }

    }
}