using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MealsToGo.Models;

namespace MealsToGo.Controllers
{
    public class MealSearchController : Controller
    {
        private ThreeSixtyTwoEntities db = new ThreeSixtyTwoEntities();

        //
        // GET: /MealSearch/Search

        public ActionResult Search()
        {
            ActiveMealAd searchmealparam = new ActiveMealAd();

            return View(searchmealparam);
        }

        //
        // POST: /MealSearch/Search/5

        [HttpPost]
        public ActionResult Search(ActiveMealAd searchmealparam)
        {
           

            List<ActiveMealAd> activemealadlist = new List<ActiveMealAd>();
            activemealadlist = db.ActiveMealAds.ToList();
            return View(activemealadlist);
        }

        
        //
        // GET: /MealSearch/

        public ActionResult Index()
        {
            return View(db.ActiveMealAds.ToList());
        }

        //
        // GET: /MealSearch/Details/5

        public ActionResult Details(int id = 0)
        {
            ActiveMealAd activemealad = db.ActiveMealAds.Find(id);
            if (activemealad == null)
            {
                return HttpNotFound();
            }
            return View(activemealad);
        }

        //
        // GET: /MealSearch/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MealSearch/Create

        [HttpPost]
        public ActionResult Create(ActiveMealAd activemealad)
        {
            if (ModelState.IsValid)
            {
                db.ActiveMealAds.Add(activemealad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(activemealad);
        }

        //
        // GET: /MealSearch/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ActiveMealAd activemealad = db.ActiveMealAds.Find(id);
            if (activemealad == null)
            {
                return HttpNotFound();
            }
            return View(activemealad);
        }

        //
        // POST: /MealSearch/Edit/5

        [HttpPost]
        public ActionResult Edit(ActiveMealAd activemealad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activemealad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(activemealad);
        }

        //
        // GET: /MealSearch/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ActiveMealAd activemealad = db.ActiveMealAds.Find(id);
            if (activemealad == null)
            {
                return HttpNotFound();
            }
            return View(activemealad);
        }

        //
        // POST: /MealSearch/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ActiveMealAd activemealad = db.ActiveMealAds.Find(id);
            db.ActiveMealAds.Remove(activemealad);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}