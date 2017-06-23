using IQMStarterKit.Models.Core;
using System.ComponentModel.DataAnnotations;

namespace IQMStarterKit.Models.Forms
{
    //page47
    public class PersonalSWOTClass
    {
        [DataType(DataType.MultilineText)]
        public string Strengths { get; set; }
        public string Weaknesses { get; set; }
        public string Opportunities { get; set; }
        public string Threats { get; set; }

        public StudentActivity StudentActivity { get; set; }

        public PersonalSWOTClass()
        {
            StudentActivity = new StudentActivity();
        }

    }
}