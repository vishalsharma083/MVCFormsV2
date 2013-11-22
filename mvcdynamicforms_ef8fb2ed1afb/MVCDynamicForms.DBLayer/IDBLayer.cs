using MvcDynamicForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCDynamicForms.DBLayer
{
    interface IDBLayer
    {
        void Save(Form form_);
        Form Get(Guid id_);
        void Delete(Guid id_);
        void Update(Form form_);
    }
}
