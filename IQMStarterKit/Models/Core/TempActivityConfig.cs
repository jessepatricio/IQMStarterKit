using System;

namespace IQMStarterKit.Models.Core
{

    public class TempActivityConfig
    {
        // activity fields
        public byte TempActivityId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int SortOrder { get; set; }

        public byte TempModuleId { get; set; }


        //config fields
        public byte GroupActivityConfigId { get; set; }

        public byte GroupId { get; set; }

        public bool IsLocked { get; set; }

        public string Remarks { get; set; }


        //system fields
        public DateTime CreatedDateTime { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public string ModifiedBy { get; set; }

    }
}