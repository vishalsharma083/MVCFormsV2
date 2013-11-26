﻿using MvcDynamicForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCDynamicForms.DBLayer
{
    interface IDBLayer
    {
        void Save<T>(T val_) where T : ContentBase;
        T Get<T>(Guid id_) where T : ContentBase;
        void Delete<T>(Guid id_) where T : ContentBase;
        void Update<T>(T val_) where T : ContentBase;
    }
}