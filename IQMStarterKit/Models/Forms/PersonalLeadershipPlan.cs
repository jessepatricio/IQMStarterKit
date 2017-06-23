using IQMStarterKit.Models.Core;
using System.ComponentModel.DataAnnotations;

namespace IQMStarterKit.Models.Forms
{
    //page35
    public class PersonalLeadershipPlanClass
    {
        [DataType(DataType.MultilineText)]
        public string Goal1 { get; set; }
        public string ActionPlan1 { get; set; }
        public string Goal2 { get; set; }
        public string ActionPlan2 { get; set; }
        public string Goal3 { get; set; }
        public string ActionPlan3 { get; set; }
        public string Goal4 { get; set; }
        public string ActionPlan4 { get; set; }

        public StudentActivity StudentActivity { get; set; }

        public PersonalLeadershipPlanClass()
        {
            StudentActivity = new StudentActivity();
        }
    }
}