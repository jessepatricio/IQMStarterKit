using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IQMStarterKit.Models
{
    public class PresentationEvaluationModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte PresentationId { get; set; }

        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public string Topic { get; set; }
        public string Grooming { get; set; }
        public string Content { get; set; }
        public string Delivery { get; set; }
        public string Visual { get; set; }
        public string Knowledge { get; set; }
        public string Timing { get; set; }
        public string Comment { get; set; }
        public string TutorId { get; set; }
        public string TutorName { get; set; }


        //system fields
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }




    }
}