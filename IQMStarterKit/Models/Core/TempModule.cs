﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQMStarterKit.Models.Core
{
    public class TempModule
    {
     

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte TempModuleId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public int SortOrder { get; set; }
        
        public byte TempWorkbookId { get; set; }

        public ICollection<TempActivity> TempActivities { get; set; }




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