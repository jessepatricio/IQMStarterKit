using IQMStarterKit.Models.Core;

namespace IQMStarterKit.Models.Forms
{
    public class Conflict
    {
        public int Shark { get; set; }
        public int Owl { get; set; }
        public int Turtle { get; set; }
        public int Teddy { get; set; }
        public int Fox { get; set; }

        public string FirstDominant { get; set; }
        public string SecondDominant { get; set; }

        public StudentActivity StudentActivity { get; set; }

        public Conflict()
        {
            StudentActivity = new StudentActivity();

        }
    }
}