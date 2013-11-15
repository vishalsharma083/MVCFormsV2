using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcDynamicForms.Demo.Models;
using MvcDynamicForms;

namespace MVCDynamicForms.Demo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
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
    }
}
