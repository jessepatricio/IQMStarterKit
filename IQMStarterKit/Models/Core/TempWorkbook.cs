using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using IQMStarterKit.Models.Core;

namespace IQMStarterKit.Models    
{
    public interface ITempWorkbook
    {
         byte TempWorkbookId { get; set; }

         string Title { get; set; }
       
         string Description { get; set; }
        
         string Version { get; set; }

    }

    public class TempWorkbook : ITempWorkbook
    {
        
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte TempWorkbookId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [StringLength(100)]
        public string Version { get; set; }

        public ICollection<TempModule> TempModules  { get; set; }

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