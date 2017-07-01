using IQMStarterKit.Models.Core;
using System.ComponentModel.DataAnnotations;


namespace IQMStarterKit.Models.Forms
{
    //page48
    public class MaturityContinuumClass
    {
        public string Int { get; set; }
        public string Habit5 { get; set; }
        public string Habit6 { get; set; }
        public string Habit4 { get; set; }

        public string In { get; set; }

        public string Habit3 { get; set; }
        public string Habit1 { get; set; }
        public string Habit2 { get; set; }

        public string De { get; set; }

        public string Habit7 { get; set; }
        public string Habit8 { get; set; }


        public StudentActivity StudentActivity { get; set; }

        public MaturityContinuumClass()
        {
            StudentActivity = new StudentActivity();
        }


    }
}