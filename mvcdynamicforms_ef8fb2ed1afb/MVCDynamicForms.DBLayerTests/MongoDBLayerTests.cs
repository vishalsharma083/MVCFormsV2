using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCDynamicForms.DBLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCDynamicForms.Demo.Models;
using MVCDynamicForms;
namespace MVCDynamicForms.DBLayer.Tests
{
    [TestClass()]
    public class MongoDBLayerTests
    {
        [TestMethod()]
        public void SaveFormStructureTest()
        {
            MongoDBLayer dblayer = new MongoDBLayer();
            MVCDynamicForms.Form form = FormProvider.GetForm();
            form.ContentId = Guid.NewGuid();
            form.SiteId = Guid.NewGuid();
            dblayer.Save<MVCDynamicForms.Form>(form);
        }

        [TestMethod()]
        public void SaveFormDataTest()
        {
            MongoDBLayer dblayer = new MongoDBLayer();
            MVCDynamicForms.Form form = FormProvider.GetFormWithData();
            form.ContentId = Guid.NewGuid();
            form.SiteId = Guid.NewGuid();
            dblayer.Save<FormData>(new FormData { ContentId = form.ContentId, Content = form.ToJson(true), SiteId = form.SiteId });
        }

        [TestMethod()]
        public void SaveFormDataWithTagsTest()
        {
            MongoDBLayer dblayer = new MongoDBLayer();
            MVCDynamicForms.Form form = FormProvider.GetFormWithData();
            form.ContentId = Guid.NewGuid();
            FormData formData = new FormData { ContentId = form.ContentId, Content = form.ToJson(true) };
            formData.Tags = new List<string>();
            formData.Tags.Add("Bar1");
            formData.Tags.Add("Bar2");
            formData.Tags.Add("Bar3");
            dblayer.Save<FormData>(formData);

            formData = new FormData { ContentId = form.ContentId, Content = form.ToJson(true) };
            formData.Tags = new List<string>();
            formData.Tags.Add("foo1");
            formData.Tags.Add("foo2");
            formData.Tags.Add("foo3");
            dblayer.Save<FormData>(formData);

            formData = new FormData { ContentId = form.ContentId, Content = form.ToJson(true) };
            formData.Tags = new List<string>();
            formData.Tags.Add("test1");
            formData.Tags.Add("test2");
            formData.Tags.Add("test3");
            dblayer.Save<FormData>(formData);
        }

        [TestMethod()]
        public void FormDataGetTest()
        {
            MVCDynamicForms.Form form = FormProvider.GetFormWithData();
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
            List<FormData> actual = new List<FormData>();
            actual = dblayer.GetByTagAndContentId<FormData>(Guid.Parse("19c91d63-e3a2-4dae-bb3c-01efc0334b34"), "foo1");
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);
            Assert.IsNotNull(actual.Where(x => x.Tags.Contains("foo1")).FirstOrDefault<FormData>());
            Assert.AreEqual(actual.Where(x => x.Tags.Contains("foo1")).FirstOrDefault<FormData>().ContentId, Guid.Parse("19c91d63-e3a2-4dae-bb3c-01efc0334b34"));
        }

        [TestMethod()]
        public void GetWithTagsTest2()
        {
            MongoDBLayer dblayer = new MongoDBLayer();
            List<FormData> actual = new List<FormData>();
            actual = dblayer.GetByTagAndContentId<FormData>(Guid.Parse("19c91d63-e3a2-4dae-bb3c-01efc0334b34"), "foo1");
        }
    }
}
