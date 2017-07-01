﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IQMStarterKit.Models
{
    public class TutorSurveyModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte TutorSurveyId { get; set; }

        public string TutorId { get; set; }
        public int T1 { get; set; }
        public int T2 { get; set; }
        public int T3 { get; set; }
        public int T4 { get; set; }
        public int T5 { get; set; }
        public int T6 { get; set; }
        public int T7 { get; set; }
        public int T8 { get; set; }
        public int TOverall { get; set; }
        public string TComment { get; set; }

        public byte GroupId { get; set; }

        //system fields
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}