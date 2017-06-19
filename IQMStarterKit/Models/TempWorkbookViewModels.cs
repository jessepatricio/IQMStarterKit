using IQMStarterKit.Models.Core;
using System.Collections.Generic;

namespace IQMStarterKit.Models
{
    public class TempWorkbookViewModels
    {
        public TempWorkbook TempWorkbook { get; set; }

        public IEnumerable<TempModule> TempModules{ get; set; }

        public IEnumerable<TempActivity> TempActivities { get; set; }


    }
}