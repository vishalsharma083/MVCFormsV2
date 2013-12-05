using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace MVCDynamicForms
{
    public class Utility
    {
        public static string GetFormFieldValue(NameValueCollection Collection, string FieldName)
        {
            if (Collection[FieldName] != null)
            {
                return Collection[FieldName].Trim();
            }
            return string.Empty;
        }
    }
}
