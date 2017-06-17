using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IQMStarterKit.Models.Core
{
    public class GroupActivityConfig
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte GroupActivityConfigId { get; set; }

        [Required]
        public byte GroupId { get; set; }

        [Required]
        public byte TempActivityId { get; set; }

        [DefaultValue(0)]
        public bool IsLocked { get; set; }

        [StringLength(1000)]
        public string Remarks { get; set; }


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




    }
}