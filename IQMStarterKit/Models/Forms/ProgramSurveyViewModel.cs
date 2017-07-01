using System;

namespace IQMStarterKit.Models.Forms
{
    public class ProgramSurveyViewModel
    {

        public byte ProgramSurveyId { get; set; }

        public int P1 { get; set; }
        public int P2 { get; set; }
        public int P3 { get; set; }
        public int P4 { get; set; }
        public int P5 { get; set; }
        public int P6 { get; set; }
        public int P7 { get; set; }
        public int POverall { get; set; }
        public int PTimeAllocated { get; set; }
        public int PClassSize { get; set; }
        public int PClassroom { get; set; }
        public string PComment { get; set; }

        public ApplicationUser Users { get; set; }

        //system fields
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}