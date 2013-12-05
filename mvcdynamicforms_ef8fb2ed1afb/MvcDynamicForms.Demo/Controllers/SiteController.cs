using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcDynamicForms;
using MVCDynamicForms.DBLayer;

namespace MVCDynamicForms.Demo.Controllers
{ 
    public class SiteController : Controller
    {
        MongoDBLayer dbLayer = new MongoDBLayer();
        //
        // GET: /Site/

        public ViewResult Index()
        {
            return View(dbLayer.GetAll<Site>());
        }

        //
        // GET: /Site/Details/5

        public ViewResult Details(Guid id)
        {
            Site site = dbLayer.Get<Site>(id);
            return View(site);
        }

        //
        // GET: /Site/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Site/Create

        [HttpPost]
        public ActionResult Create(Site site)
        {
            if (ModelState.IsValid)
            {
                site.ContentId = Guid.NewGuid();
                site.CreatedOn = DateTime.Now;
                site.CreatedBy = -1;
                dbLayer.Save<Site>(site);
                return RedirectToAction("Index");  
            }

            return View(site);
        }
        
        //
        // GET: /Site/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            Site site = dbLayer.Get<Site>(id);
            return View(site);
        }

        //
        // POST: /Site/Edit/5

        [HttpPost]
        public ActionResult Edit(Site site)
        {
            if (ModelState.IsValid)
            {
                dbLayer.Save<Site>(site);
                return RedirectToAction("Index");
            }
            return View(site);
        }

        //
        // GET: /Site/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            Site site = dbLayer.Get<Site>(id);
            return View(site);
        }

        //
        // POST: /Site/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            dbLayer.Delete<Site>(id);
            return RedirectToAction("Index");
        }
    }
}