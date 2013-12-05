using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace MVCDynamicForms
{
    [Serializable]
    public class ContentBase
    {
        public object Id { get; set; }
        public Guid ContentId { get; set; }
    }
}
