﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IQMStarterKit.Models.Core
{

    public class TempActivity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte TempActivityId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }


        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [StringLength(3)]
        public string Code { get; set; }


        [StringLength(255)]
        public string PageName { get; set; }

        [Required]
        public int SortOrder { get; set; }

        [Display(Name = "Module Name")]
        public byte TempModuleId { get; set; }

        [NotMapped]
        public int ProgressValue { get; set; }

        [NotMapped]
        public string Context { get; set; }

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

        [DefaultValue(1)]
        public bool IsActivity { get; set; }

        [DefaultValue(0)]
        public bool IsRemoved { get; set; }
    }
}