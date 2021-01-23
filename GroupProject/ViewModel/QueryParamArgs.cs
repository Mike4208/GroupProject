using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupProject.ViewModel
{
    public class QueryParamArgs
    {
        public string sortOrder { get; set; }
        public string currentFilter { get; set; }
        public string searchString { get; set; }
        public string selectedCategory { get; set; }
        public string selectedManufacturer { get; set; }
        public int? page { get; set; }
    }
}