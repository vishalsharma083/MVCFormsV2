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
        }

        [TestMethod()]
        public void SaveFormDataTest()
        {
            MongoDBLayer dblayer = new MongoDBLayer();
            MvcDynamicForms.Form form = FormProvider.GetFormWithData();
            form.ContentId = Guid.NewGuid();
            dblayer.Save<FormData>(new FormData { ContentId = form.ContentId, Content = form.ToJson(true) });
        }

        [TestMethod()]
        public void SaveFormDataWithTagsTest()
        {
            MongoDBLayer dblayer = new MongoDBLayer();
            MvcDynamicForms.Form form = FormProvider.GetFormWithData();
            form.ContentId = Guid.NewGuid();
            FormData formData = new FormData { ContentId = Guid.Parse("19c91d63-e3a2-4dae-bb3c-01efc0334b34"), Content = form.ToJson(true) };
            //formData.Tags = new List<string>();
            //formData.Tags.Add("Bar1");
            //formData.Tags.Add("Bar2");
            //formData.Tags.Add("Bar3");
            //dblayer.Save<FormData>(formData);

            //formData = new FormData { ContentId = form.ContentId.ToString(), Content = form.ToJson(true) };
            formData.Tags = new List<string>();
            formData.Tags.Add("foo1");
            formData.Tags.Add("foo2");
            formData.Tags.Add("foo3");
            dblayer.Save<FormData>(formData);

            //formData = new FormData { ContentId = form.ContentId.ToString(), Content = form.ToJson(true) };
            //formData.Tags = new List<string>();
            //formData.Tags.Add("test1");
            //formData.Tags.Add("test2");
            //formData.Tags.Add("test3");
            //dblayer.Save<FormData>(formData);
        }

        [TestMethod()]
        public void FormDataGetTest()
        {
            MvcDynamicForms.Form form = FormProvider.GetFormWithData();
            form.ContentId = Guid.NewGuid();
            MongoDBLayer dblayer = new MongoDBLayer();

            FormData expected = new FormData();
            expected.ContentId = form.ContentId;
            expected.Content = form.ToJson();
            dblayer.Save<FormData>(expected);
            
            FormData actual = new FormData();
            actual = dblayer.Get<FormData>(form.ContentId);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.ContentId, actual.ContentId);

            // now clean up test data.
            dblayer.Delete<FormData>(form.ContentId);
        }

        [TestMethod()]
        public void GetWithTagsTest()
        {
            MongoDBLayer dblayer = new MongoDBLayer();
            MvcDynamicForms.Form form = FormProvider.GetFormWithData();
            form.ContentId = Guid.NewGuid();

            FormData expected = new FormData { ContentId = form.ContentId, Content = form.ToJson(true) };
            expected.Tags = new List<string>();
            expected.Tags.Add("Test1");
            expected.Tags.Add("Test2");
            expected.Tags.Add("Test3");
            dblayer.Save<FormData>(expected);

            List<FormData> actual = new List<FormData>();
            actual = dblayer.GetByTag<FormData>(form.ContentId, "Test3");

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected.ContentId, actual[0].ContentId);
        }
    }


  
}
