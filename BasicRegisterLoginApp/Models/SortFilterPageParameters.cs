using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicRegisterLoginApp.Models
{
    public class SortFilterPageParameters
    {
        public string sortOrder { get; set; }
        public string currentFilter { get; set; }
        public string searchNameString { get; set; }
        public string searchEmailString { get; set; }
        public int? page { get; set; }
    }
}