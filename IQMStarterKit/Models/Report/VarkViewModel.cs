using System.ComponentModel.DataAnnotations;

namespace IQMStarterKit.Models
{
    public class VarkViewModel
    {
        public string TempActivityId { get; set; }
        public string VarkResult { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDateTime { get; set; }
        public string GroupId { get; set; }

        [Display(Name = "StudentName")]
        public string FullName { get; set; }
        public string GroupName { get; set; }
    }
}