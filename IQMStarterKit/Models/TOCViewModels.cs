using IQMStarterKit.Models.Core;
using System.Collections.Generic;

namespace IQMStarterKit.Models
{
    public class TOCViewModels
    {
        public TempWorkbook TempWorkbook { get; set; }
        public IEnumerable<TempModule> TempModules { get; set; }
        public IEnumerable<GroupActivityConfig> GroupActivityConfigs { get; set; }

        public string StudentName { get; set; }
        public bool IsDemo { get; set; }
        public double OverallProgressValue { get; set; }
    }
}