﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMStarterKit.Models.Core;

namespace IQMStarterKit.Models
{
    public class TempModuleViewModels
    {
       
        public byte TempModuleId { get; set; }

       
        public string Title { get; set; }

       
        public string Description { get; set; }

       
        public int SortOrder { get; set; }

       
        public byte TempWorkbookId { get; set; }

        public IEnumerable<TempWorkbook> TempWorkbooks { get; set; }

       


       

       
    }
}