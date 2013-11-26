using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCDynamicForms.DBLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcDynamicForms.Demo.Models;
using MvcDynamicForms;
namespace MVCDynamicForms.DBLayer.Tests
{
    [TestClass()]
    public class MongoDBLayerTests
    {
        [TestMethod()]
        public void SaveFormStructureTest()
        {
            MongoDBLayer dblayer = new MongoDBLayer();
            MvcDynamicForms.Form form = FormProvider.GetForm();
            form.ContentId = Guid.NewGuid();
            dblayer.Save<MvcDynamicForms.Form>(form);
            //List<MvcDynamicForms.Response> responses = form.GetResponses(false);
        }

        [TestMethod()]
        public void SaveFormDataTest()
        {
            MongoDBLayer dblayer = new MongoDBLayer();
            MvcDynamicForms.Form form = FormProvider.GetFormWithData();
            form.ContentId = Guid.NewGuid();
            dblayer.Save<FormData>(new FormData { ContentId = form.ContentId, Content = form.ToJson(true) });
        }
    }
}
