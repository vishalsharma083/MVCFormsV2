using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCDynamicForms.Demo.Models;
using MVCDynamicForms;
using MVCDynamicForms.DBLayer;
using MVCDynamicForms.Fields;

namespace MVCDynamicForms.Demo.Controllers
{
    public class HomeController : Controller
    {
        MongoDBLayer dblayer = new MongoDBLayer();
        public ActionResult Create()
        {
            ViewData["Sites"] = GetSiteList();
            return View( new Form());
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            Form form = ConstructFormDefinition(collection);
            form.ContentId= Guid.NewGuid();
            dblayer.Save<Form>(form);
            ViewData["Sites"] = GetSiteList();
            return View(form);
        }

        private SelectList GetSiteList()
        {
            List<Site> sites = dblayer.GetAll<Site>();
            return new SelectList(sites, "ContentId", "SiteName");
        }
        public ActionResult Index()
        {
            return View(dblayer.GetAll<Form>());
        }

        public ActionResult About()
        {
            return View();
        }
        public ActionResult Demo1()
        {
            var form = FormProvider.GetForm();

            // we are going to store the form and 
            // the field objects on the page across requests
            form.Serialize = true;

            return View("Demo", form);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Demo1(Form form)
        {   //                           ^
            // no need to retrieve the form object from anywhere
            // just use a parameter on the Action method that we are posting to

            if (form.Validate()) //input is valid
                return View("Responses", form);

            // input is not valid
            return View("Demo", form);
        }

        public ActionResult Demo2()
        {
            var form = FormProvider.GetForm();

            // we are going to store the form 
            // in server memory across requests
            TempData["form"] = form;

            return View("Demo", form);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionName("Demo2")]
        public ActionResult Demo2Post()
        {
            // we have to get the form object from
            // server memory and manually perform model binding
            var form = (Form)TempData["form"];
            UpdateModel(form);

            if (form.Validate()) // input is valid
                return View("Responses", form);

            // input is not valid
            TempData["form"] = form;
            return View("Demo", form);
        }             
        
        public ActionResult Demo3()
        {
            // recreate the form and set the keys
            var form = FormProvider.GetForm();
            Demo3SetKeys(form);

            // set user input on recreated form
            UpdateModel(form);

            if (Request.HttpMethod == "POST" && form.Validate()) // input is valid
                return View("Responses", form);

            // input is not valid
            return View("Demo", form);
        }

        void Demo3SetKeys(Form form)
        {
            int key = 1;
            foreach (var field in form.InputFields)
            {
                field.Key = key++.ToString();
            }
        }
        public Form ConstructFormDefinition(FormCollection form_)
        {
            Form dynamicFormDefinition = null;

            if (form_ != null)
            {
                dynamicFormDefinition = new Form();
                dynamicFormDefinition.FormName = form_["FormName"];
                dynamicFormDefinition.IsActive = Convert.ToBoolean(form_["Active"]);
                dynamicFormDefinition.SiteId = Guid.Parse(form_["SiteId"]);
                
                for (int iCounter = 1; iCounter <= 50; iCounter++)
                {
                    //Check whether target field is present for this iteration, if not present consider ending the loop
                    if (form_["fieldnameqrow" + iCounter.ToString()] != null)
                    {
                        switch (Utility.GetFormFieldValue(form_, "fieldtypeqrow" + iCounter.ToString()))
                        {
                            case "TextBox":
                                TextBox field = new TextBox();
                                field.ResponseTitle = Utility.GetFormFieldValue(form_, "fieldnameqrow" + iCounter.ToString()).Replace(" ", "-");
                                dynamicFormDefinition.AddFields(field);
                                break;
                            case "TextArea":
                                TextArea textArea = new TextArea();
                                textArea.ResponseTitle = Utility.GetFormFieldValue(form_, "fieldnameqrow" + iCounter.ToString()).Replace(" ", "-");
                                dynamicFormDefinition.AddFields(textArea);
                                break;
                            case "CheckBox":
                                CheckBox checkBox = new CheckBox();
                                checkBox.ResponseTitle = Utility.GetFormFieldValue(form_, "fieldnameqrow" + iCounter.ToString()).Replace(" ", "-");
                                dynamicFormDefinition.AddFields(checkBox);
                                break;
                            case "CheckBoxList":
                                CheckBoxList checkBoxList = new CheckBoxList();
                                checkBoxList.ResponseTitle = Utility.GetFormFieldValue(form_, "fieldnameqrow" + iCounter.ToString()).Replace(" ", "-");
                                dynamicFormDefinition.AddFields(checkBoxList);
                                break;
                            case "ListBox":
                                Select select = new Select();
                                select.ResponseTitle = Utility.GetFormFieldValue(form_, "fieldnameqrow" + iCounter.ToString()).Replace(" ", "-");
                                dynamicFormDefinition.AddFields(select);
                                break;
                            case "RadioBoxList":
                                RadioList radioList = new RadioList();
                                radioList.ResponseTitle = Utility.GetFormFieldValue(form_, "fieldnameqrow" + iCounter.ToString()).Replace(" ", "-");
                                dynamicFormDefinition.AddFields(radioList);
                                break;
                        }
                    }
                }
            }
            return dynamicFormDefinition;
        }

    }
}
