using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCDynamicForms
{
    public class FormData : ContentBase
    {
        public string Content { get; set; }
        public List<string> Tags { get; set; }
        public Guid SiteId { get; set; }
    }
}
