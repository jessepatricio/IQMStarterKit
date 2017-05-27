using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMStarterKit.Models.Core;

namespace IQMStarterKit.Models
{
    public class TempWorkbookViewModels
    {
        public TempWorkbook TempWorkbook { get; set; }

        public IEnumerable<TempModule> TempModules{ get; set; }

        public IEnumerable<TempActivity> TempActivities { get; set; }


    }
}