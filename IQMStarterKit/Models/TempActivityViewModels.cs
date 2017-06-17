using IQMStarterKit.Models.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public bool IsActivity { get; set; }

        public IEnumerable<TempModule> TempModules { get; set; }
    }
}