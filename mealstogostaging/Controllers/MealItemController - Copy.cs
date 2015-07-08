using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MealsToGo.Models;
using System.IO;
using MealsToGo.ViewModels;
using AutoMapper;
using MealsToGo.Service;
using WebMatrix.WebData;


namespace MealsToGo.Controllers
{
    public class MealItemController : Controller
    {

        private readonly IMealItemService _service;


        public MealItemController(IMealItemService service)
        {
            _service = service;
        }



        // GET: /MealItem/

        public ActionResult Index(int UserID)
        {

            IEnumerable<MealItem> mt = _service.FindByUser(UserID);
            IEnumerable<MealItemViewModel> mtvm = Mapper.Map<IEnumerable<MealItem>, IEnumerable<MealItemViewModel>>(mt);
            //mtvm = PopulateDropDown(mtvm);

            return View(mtvm);
        }

        //
        // GET: /MealItem/Details/5

        public ActionResult Details(int id)
        {
            MealItem mealitem = _service.GetById(id);
            if (mealitem == null)
            {
                mealitem = new MealItem();
            }
            MealItemViewModel mtvm = Mapper.Map<MealItem, MealItemViewModel>(mealitem);

            return View(mtvm);
        }

        //
        // GET: /MealItem/Create

        public ActionResult Create()
        {

            MealItem mt = new MealItem();
            mt.UserId = WebSecurity.CurrentUserId;

            MealItemViewModel mtvm = Mapper.Map<MealItem, MealItemViewModel>(mt);

            mtvm = PopulateDropDown(mtvm);




            return View(mtvm);
        }

        private MealItemViewModel PopulateDropDown(MealItemViewModel mtvm)
        {
            mtvm.ServingUnitDDList = _service.GetServingUnitDDList().Select(x => new SelectListItem
            {
                Value = x.ServingUnitID.ToString(),
                Text = x.ServingUnit
            });



            mtvm.MealTypeDD.MealTypeDDList = _service.MealTypeDDList().Select(x => new SelectListItem
            {
                Value = x.MealTypeID.ToString(),
                Text = x.Name
            });
            mtvm.CusineTypeDD.CuisineDDList = _service.CuisineTypeDDList().Select(x => new SelectListItem
            {
                Value = x.CuisineTypeID.ToString(),
                Text = x.Name
            });

            mtvm.DietTypeDD.DietTypeDDList = _service.DietTypeDDList().Select(x => new SelectListItem
            {
                Value = x.DietTypeID.ToString(),
                Text = x.Name
            });
            mtvm.AllergenDD = _service.AllergenicFoodsDDList().Select(x => new Allergen
            {
                AllergenName = x.AllergenicFood.ToString(),
                AllergenID = x.AllergenicFoodID,
                Selected = false
            }).ToList();
            return mtvm;
        }
        //
        // POST: /MealItem/Create

        [HttpPost]
        public ActionResult Create(MealItemViewModel mtvms, HttpPostedFileBase[] Photos)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    MealItem mealitem = Mapper.Map<MealItemViewModel, MealItem>(mtvms);

                    foreach (var fileBase in Photos)
                    {
                        if (fileBase != null)
                        {

                            if (fileBase.ContentLength > 0)
                            {
                                var path = Path.Combine(Server.MapPath("~/MealPhotos"), WebSecurity.CurrentUserId + '-' + Path.GetRandomFileName().Replace(".", "").Substring(0, 8) + '-' + Path.GetFileName(fileBase.FileName));


                                fileBase.SaveAs(path);
                                MealItems_Photos mp = new MealItems_Photos();
                                mp.Photo = path;
                                mealitem.MealItems_Photos.Add(mp);
                            }
                        }

                    }


                    foreach (var mealaller in mtvms.AllergenDD)
                    {
                        if (mealaller.Selected)
                        {

                            MealItems_AllergenicFoods aller = new MealItems_AllergenicFoods();
                            aller.AllergenicFoodID = mealaller.AllergenID;
                            mealitem.MealItems_AllergenicFoods.Add(aller);
                        }

                    }





                    mealitem.MealItemId = _service.AddAndReturnID(mealitem);

                    return RedirectToAction("Details", "MealItem", new { id = mealitem.MealItemId });

                }
            }
            catch (Exception ex)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes." + ex.Message.ToString());
            }
            mtvms = PopulateDropDown(mtvms);
            return View(mtvms);
        }




        //
        // GET: /MealItem/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MealItem mealitem = _service.GetById(id);
            MealItemViewModel mtvm = Mapper.Map<MealItem, MealItemViewModel>(mealitem);

            mtvm = PopulateDropDown(mtvm);




            return View(mtvm);
            //if (mealitem == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.CusineTypeID = new SelectList(db.LKUPCuisineTypes, "CuisineTypeID", "Name", mealitem.CusineTypeID);
            //ViewBag.MealTypeID = new SelectList(db.LKUPCuisineTypes, "MealTypeID", "Name", mealitem.MealTypeID);
            //ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", mealitem.UserId);
            // return View(mealitem);
        }

        //
        // POST: /MealItem/Edit/5

        [HttpPost]
        public ActionResult Edit(MealItemViewModel mtvms)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(mealitem).State = EntityState.Modified;
                //db.SaveChanges();
                MealItem mealitem = Mapper.Map<MealItemViewModel, MealItem>(mtvms);
                _service.Update(mealitem);
                return RedirectToAction("Index", new { userID = WebSecurity.CurrentUserId });
            }
            return View(mtvms);
        }

        //
        // GET: /MealItem/Delete/5

        public ActionResult Delete(int id = 0)
        {
            MealItem mealitem = _service.GetById(id);
            if (mealitem == null)
            {
                return HttpNotFound();
            }
            return View(mealitem);
        }

        //
        // POST: /MealItem/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            MealItem mealitem = _service.GetById(id);
            //db.MealItems.Remove(mealitem);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}