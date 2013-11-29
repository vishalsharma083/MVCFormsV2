using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcDynamicForms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcDynamicForms.Demo.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using MVCDynamicForms.DBLayer;
namespace MvcDynamicForms.Tests
{
    [TestClass()]
    public class FormTests
    {
        [TestMethod()]
        public void RenderJsonScript_NoFormData_Test()
        {
            MvcDynamicForms.Form form = FormProvider.GetForm();
            form.ContentId = Guid.NewGuid();
            var jsonString = form.ToJson();
            Assert.IsNotNull(jsonString);
            Assert.IsTrue(IsValidJson(jsonString));
            Debug.WriteLine(jsonString);
        }

        [TestMethod()]
        public void RenderJsonScript_WithFormData_Test()
        {
            MvcDynamicForms.Form form = FormProvider.GetFormWithData();
            form.ContentId = Guid.NewGuid();
            var jsonString = form.ToJson();
            Assert.IsNotNull(jsonString);
            Assert.IsTrue(IsValidJson(jsonString));
            Debug.WriteLine(jsonString);
        }

        [TestMethod()]
        public void RenderJsonScript_NoFormData_PrepareStructure_Test()
        {
            MvcDynamicForms.Form form = FormProvider.GetForm();
            form.ContentId = Guid.NewGuid();
            var jsonString = form.ToJson(true);
            Assert.IsNotNull(jsonString);
            Assert.IsTrue(IsValidJson(jsonString));
            Debug.WriteLine(jsonString);
        }

        [TestMethod()]
        public void RenderJsonScript_WithFormData_PrepareStructure_Test()
        {
            MvcDynamicForms.Form form = FormProvider.GetFormWithData();
            form.ContentId = Guid.NewGuid();
            var jsonString = form.ToJson(true);

            Assert.IsNotNull(jsonString);
            Assert.IsTrue(IsValidJson(jsonString));
            Debug.WriteLine(jsonString);
        }


        [TestMethod()]
        public void RenderDataScript_NoFormData_Test()
        {
            MvcDynamicForms.Form form = FormProvider.GetForm();
            form.ContentId = Guid.NewGuid();
            var jsonString = form.RenderDataScript("temp");
            Assert.IsNotNull(jsonString);
            Debug.WriteLine(jsonString);
        }

        [TestMethod()]
        public void RenderDataScript_WithFormData_Test()
        {
            MvcDynamicForms.Form form = FormProvider.GetFormWithData();
            form.ContentId = Guid.NewGuid();
            var jsonString = form.RenderDataScript("temp");
            Assert.IsNotNull(jsonString);
            Debug.WriteLine(jsonString);
        }


        [TestMethod()]
        public void StringFormatTest()
        {
            Debug.WriteLine(string.Format("{{\"ContentId\":{0},\"FormContent\":{1}}}", 1, "{}"));
        }

       

        [TestMethod()]
        public void SaveFormData_Test()
        {
            MvcDynamicForms.Form form = FormProvider.GetFormWithData();
            form.ContentId = Guid.NewGuid();
            MongoDBLayer dblayer = new MongoDBLayer();

            FormData formdata = new FormData();
            formdata.ContentId = form.ContentId;
            formdata.Content = form.ToJson();
            dblayer.Save<FormData>(formdata);
        }


        private bool IsValidJson(string jsonObject_)
        {
            bool isValidJson = false;
            try
            {
                using (StringReader streader = new StringReader(jsonObject_))
                {
                    using (JsonTextReader jreader = new JsonTextReader(streader))
                    {
                        while (jreader.Read())
                        {

                        }
                    }
                }
                // if code reached here it means json is well formed.
                isValidJson = true;
            }
            catch { isValidJson = false; }
            return isValidJson;
        }
    }
}
