using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IQMStarterKit.Models
{
    public class GroupTutorModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte GroupTutorId { get; set; }

        [Required]
        public byte GroupId { get; set; }

        [Required]
        public string TutorId { get; set; }

        [Required]
        public string TutorName { get; set; }

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