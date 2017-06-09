using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IQMStarterKit.Models.Core
{
    public class StudentActivity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte StudentActivityId { get; set; }

        [Required]
        public byte TempModuleId { get; set; }

        [Required]
        public byte TempActivityId { get; set; }

        [StringLength(255)]
        public string VarkResult { get; set; }

        [StringLength(255)]
        public string DopeResult { get; set; }

        [StringLength(255)]
        public string DiscResult { get; set; }

        [StringLength(255)]
        public string Type { get; set; }

        public int NoMatchedWords { get; set; }

        [StringLength(255)]
        public string Top3PersonalValues { get; set; }

        public int PersonalLeaderShipScore { get; set; }

        public int SelfManagementScore { get; set; }

        public int AssertiveScore { get; set; }

        [StringLength(255)]
        public string CFDominantFirst { get; set; }

        [StringLength(255)]
        public string CFDominantSecond { get; set; }

        public int ReviewQuizScore { get; set; }

        public int ProgressValue { get; set; }

        [StringLength(5000)]
        public string Context { get; set; }

        public FilePath FilePath { get; set; }


        //system fields
        [Required]
        public DateTime CreatedDateTime { get; set; }

        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime ModifiedDateTime { get; set; }

        [Required]
        [StringLength(100)]
        public string ModifiedBy { get; set; }

        [DefaultValue(0)]
        public bool IsRemoved { get; set; }
    }
}