using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcDynamicForms
{
    public class Site : ContentBase
    {
        public string SiteName { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }
}
