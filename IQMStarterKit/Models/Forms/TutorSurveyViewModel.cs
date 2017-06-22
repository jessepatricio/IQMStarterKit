using System.Collections.Generic;

namespace IQMStarterKit.Models.Forms
{
    public class TutorSurveyViewModel
    {

        public IEnumerable<TutorSurveyModel> TutorSurvey { get; set; }

        public IEnumerable<ApplicationUser> Tutors { get; set; }
    }
}