using MvcDynamicForms;
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
        List<T> GetAll<T>() where T : ContentBase;
        List<T> GetByTag<T>(string tag_) where T : ContentBase;
        List<T> GetByTagAndContentId<T>(Guid id_, string tag_) where T : ContentBase;
    }
}
