using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MealsToGo.Models;
using MealsToGo.Service;
using MealsToGo.ViewModels;
using AutoMapper;
using WebMatrix.WebData;

namespace MealsToGo.Controllers
{
    public class SettingsController : Controller
    {
       private ThreeSixtyTwoEntities db = new ThreeSixtyTwoEntities();
        
        //
        // GET: /UserSetting/

        
         private readonly IUserSettingService _service;


         public SettingsController(IUserSettingService service)
         {
            _service = service;
         }

         public ActionResult Index(int userid,string referrer)
         {

                 if (referrer == "sharemeal")
                 Session[referrer] = "sharemeal";

             UserSetting usersetting = _service.GetById(userid);
             UserSettingsViewModel usvm = Mapper.Map<UserSetting, UserSettingsViewModel>(usersetting);
             
              


             return View(usvm);
         }

        
        //
        // GET: /Settings/Details/5

         private UserSettingsViewModel PopulateDropDown(UserSettingsViewModel usvm, UserSetting ust)
         {
             if (usvm == null)
                 usvm = new UserSettingsViewModel();
             usvm.PrivacySettingDD.PrivacySettingDDList = db.LKUPPrivacySettings.ToList().Select(x => new SelectListItem
             {
                 Value = x.PrivacySettingsID.ToString(),
                 Text = x.PrivacySettings,
                 Selected = (ust != null && ust.PrivacySettingsID == x.PrivacySettingsID)
             });

             usvm.EmailNotificationFrequencyDD.FrequencyDDList = db.NotificationFrequencies.ToList().Select(x => new SelectListItem
             {
                 Value = x.NotificationFrequencyID.ToString(),
                 Text = x.Description,
                 Selected = (ust != null && ust.ReceiveEmailNotificationID == x.NotificationFrequencyID)
             });

             usvm.TextNotificationFrequencyDD.FrequencyDDList = db.NotificationFrequencies.ToList().Select(x => new SelectListItem
             {
                 Value = x.NotificationFrequencyID.ToString(),
                 Text = x.Description,
                 Selected = (ust != null && ust.ReceiveEmailNotificationID == x.NotificationFrequencyID)
             });



             return usvm;
         }

        public ActionResult Details(int id = 0)
        {
            UserSetting usersetting = db.UserSettings.Find(id);
            if (usersetting == null)
            {
                return HttpNotFound();
            }
            return View(usersetting);
        }

        //
        // GET: /Settings/Create

        public ActionResult Create()
        {
            UserSetting ut = new UserSetting();
            ut.UserID = WebSecurity.CurrentUserId;

            UserSettingsViewModel usvm = Mapper.Map<UserSetting, UserSettingsViewModel>(ut);

            usvm = PopulateDropDown(usvm, ut);
            return View();
        }

        //
        // POST: /Settings/Create

        [HttpPost]
        public ActionResult Create(UserSetting usersetting)
        {
            if (ModelState.IsValid)
            {
                db.UserSettings.Add(usersetting);
                db.SaveChanges();
                return RedirectToAction("Index", new { userID = WebSecurity.CurrentUserId });
            }

            return View(usersetting);
        }
        
        public ActionResult Confirm(int userid)
        {


            List<UserSetting> usersettings = db.UserSettings.Where(x=>x.UserID == userid).ToList();
            foreach ( UserSetting us in usersettings)
     
            {
                us.Verified = 1;
                db.Entry(us).State = EntityState.Modified;
            
            }
            
          
                
            
                db.SaveChanges();
                return RedirectToAction("Index","MealAd",routeValues: new { userid = userid });
            

           
        }
        //
        // GET: /Settings/Edit/5

        public ActionResult Edit(int userid)
        {
           
            UserSetting usersetting = _service.GetById(userid);
             
            if (usersetting == null)
            {
                return HttpNotFound();
            }

            UserSettingsViewModel usvm = Mapper.Map<UserSetting, UserSettingsViewModel>(usersetting);

            usvm = PopulateDropDown(usvm, usersetting);
            
                        
            return View(usvm);
            
           
        }

        //
        // POST: /Settings/Edit/5

        [HttpPost]
        public ActionResult Edit(UserSettingsViewModel usvm)
        {

            if (ModelState.IsValid)
            {
                UserSetting usersetting = db.UserSettings.Find(usvm.UserSettingsID);
                usersetting.ReceiveEmailNotificationID = Convert.ToInt32(usvm.EmailNotificationFrequencyDD.SelectedFrequency);
                usersetting.ReceiveMobileTextNotificationID = Convert.ToInt32(usvm.TextNotificationFrequencyDD.SelectedFrequency);
                usersetting.PrivacySettingsID = Convert.ToInt32(usvm.PrivacySettingDD.SelectedPrivacySetting);

                UserSetting us = Mapper.Map<UserSettingsViewModel, UserSetting>(usvm);
                db.Entry(usersetting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { userID = WebSecurity.CurrentUserId });
            }
            else
            {
                usvm = PopulateDropDown(usvm, null);
               

                return View(usvm);
            }

           
        }

        //
        // GET: /Settings/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserSetting usersetting = db.UserSettings.Find(id);
            if (usersetting == null)
            {
                return HttpNotFound();
            }
            return View(usersetting);
        }

        //
        // POST: /Settings/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserSetting usersetting = db.UserSettings.Find(id);
            db.UserSettings.Remove(usersetting);
            db.SaveChanges();
            return RedirectToAction("Index", new { userID = WebSecurity.CurrentUserId });
        }

         public ActionResult PrivacyRules()
        {
            return View();
         }
        
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        
    }
}