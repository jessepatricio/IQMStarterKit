using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using IQMStarterKit.Models.Core;

namespace IQMStarterKit.Models
{
    public class TempActivityViewModels
    {
        public byte TempActivityId { get; set; }
        
        public string Title { get; set; }
       
        public string Description { get; set; }
        
        public string PageName { get; set; }
        
        public int SortOrder { get; set; }
        
        public int ProgressValue { get; set; }

        [Display(Name = "Module Name")]
        public byte TempModuleId { get; set; }

        public IEnumerable<TempModule> TempModules { get; set; }
    }
}