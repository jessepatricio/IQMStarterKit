using IQMStarterKit.Models.Core;
using System.Collections.Generic;

namespace IQMStarterKit.Models
{
    public class ConfigViewModel
    {
        public GroupModel GroupModel { get; set; }
        public IEnumerable<TempActivityConfig> TempActivitiesConfig { get; set; }
    }
}