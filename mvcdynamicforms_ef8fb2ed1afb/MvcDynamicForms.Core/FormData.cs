﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcDynamicForms
{
    public class FormData : ContentBase
    {
        public string Content { get; set; }
        public string ContentId { get; set; }
        public List<string> Tags { get; set; }
    }
}
