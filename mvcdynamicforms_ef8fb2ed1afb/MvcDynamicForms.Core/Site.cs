using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCDynamicForms
{
    public class Site : ContentBase
    {
        public string SiteName { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }
}
