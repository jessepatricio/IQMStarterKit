using IQMStarterKit.Models.Core;

namespace IQMStarterKit.Models.Forms
{
    public class PersonalValues
    {


        public StudentActivity StudentActivity { get; set; }

        public PersonalValues()
        {
            StudentActivity = new StudentActivity();

        }
    }
}