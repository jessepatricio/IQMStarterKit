using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQMStarterKit.Models
{
    public class ProgramSurveyModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte ProgramSurveyId { get; set; }

        public int P1 { get; set; }
        public int P2 { get; set; }
        public int P3 { get; set; }
        public int P4 { get; set; }
        public int P5 { get; set; }
        public int P6 { get; set; }
        public int P7 { get; set; }
        public string POverall{ get; set; }
        public string PTimeAllocated { get; set; }
        public string PClassSize { get; set; }
        public string PClassroom { get; set; }
        public string PComment { get; set; }

        //system fields
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}