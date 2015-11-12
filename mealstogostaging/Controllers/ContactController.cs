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
    public class ContactController : Controller
    {
        private UsersContext db = new UsersContext();
        private ThreeSixtyTwoEntities dbmeals = new ThreeSixtyTwoEntities();

       
       

        // GET: /Contact/

        private int HasBoughtFoodFromUser(int UserId, int ContactID)
        {
            bool BoughtFood = dbmeals.OrderDetails.Any(x => x.OrderID == (dbmeals.Orders.Where(n => n.UserId == ContactID)).FirstOrDefault().OrderID && x.MealAdID == (dbmeals.MealAds.Where(n => n.UserId == UserId)).FirstOrDefault().MealAdID);//add a column userid in mealadid
            if (BoughtFood)
                return 1;
            else return 0;
        }

        private int HasSoldFoodToUser(int UserId, int ContactID)
        {
            bool SoldFood = dbmeals.OrderDetails.Any(x => x.OrderID == (dbmeals.Orders.Where(n => n.UserId == UserId)).FirstOrDefault().OrderID && x.MealAdID == (dbmeals.MealAds.Where(n => n.UserId == ContactID)).FirstOrDefault().MealAdID);
            if (SoldFood)
                return 1;
            else return 0;
        }

        private int SharesFood(int ContactID)
        {
            bool SharedFood = dbmeals.MealAds.Any(x => x.MealItemID == (dbmeals.MealItems.Where(n => n.UserId == ContactID)).FirstOrDefault().MealItemId);
            if (SharedFood)
            return 1;
            else return 0;
        }
       
        public ActionResult AcceptRequest(int RecipientUserID, int SenderUserID)
        {
             EmailModel em = new EmailModel();
            string err="";
            try
            {
                ContactList contact = dbmeals.ContactLists.Where(x => (x.UserID == RecipientUserID && x.RecipientUserID == SenderUserID) || (x.UserID == SenderUserID && x.RecipientUserID == RecipientUserID)).First();


                contact.RequestAccepted = 1;
                dbmeals.Entry(contact).State = EntityState.Modified;

                Connection cn1 = new Connection();
                cn1.UserId = SenderUserID;
                cn1.ContactID = RecipientUserID;
                cn1.DegreeOfSeparation = 1;
                cn1.ContactEmail = db.UserProfiles.Where(x => x.UserId == RecipientUserID).First().UserName;
                cn1.ContactStrength = 0;//need to write an algorithm later to define contact strength
                cn1.BoughtFoodFromUser = HasBoughtFoodFromUser(SenderUserID, RecipientUserID);
                cn1.SoldFoodToUser = HasSoldFoodToUser(SenderUserID, RecipientUserID); ;
                cn1.SharesFood = SharesFood(RecipientUserID);
                dbmeals.Connections.Add(cn1);

                Connection cn2 = new Connection();
                cn2.UserId = RecipientUserID;
                cn2.ContactID = SenderUserID;
                cn2.DegreeOfSeparation = 1;
                cn2.ContactEmail = db.UserProfiles.Where(x => x.UserId == RecipientUserID).First().UserName;
                cn2.ContactStrength = 0;
                cn2.BoughtFoodFromUser = HasBoughtFoodFromUser(RecipientUserID, SenderUserID);
                cn2.SoldFoodToUser = HasSoldFoodToUser(RecipientUserID, SenderUserID); ;
                cn2.SharesFood = SharesFood(SenderUserID);
                dbmeals.Connections.Add(cn2);

                dbmeals.SaveChanges();
            }
            catch (Exception e)
            {
                err = e.Message.ToString();
               
               
            }

            if ( err != "")
            {
                em.To = "kanjasaha@gmail.com";
                em.From = "kanjasaha@gmail.com";
                em.EmailBody = err;
                Common.sendeMail(em,true);

            }
            return RedirectToAction("Index", new { UserID = RecipientUserID });


        }
        
        public ActionResult RejectRequest(int RecipientUserID, int SenderUserID)
        {

            ContactList contact = dbmeals.ContactLists.Where(x => (x.UserID == RecipientUserID && x.RecipientUserID == SenderUserID) || (x.UserID == SenderUserID && x.RecipientUserID == RecipientUserID)).First();

            contact.RequestAccepted = 0;
            dbmeals.Entry(contact).State = EntityState.Modified;
            dbmeals.SaveChanges();
            return RedirectToAction("Index", new { UserID = RecipientUserID });


        }
        
        public ActionResult Remove(int UserID, int ContactUserID)
        {
            ContactList contact = dbmeals.ContactLists.Where(x => (x.UserID == UserID && x.RecipientUserID == ContactUserID) || (x.UserID == ContactUserID && x.RecipientUserID == UserID)).First();
            contact.RequestAccepted = 0;
            dbmeals.Entry(contact).State = EntityState.Modified;
            Connection cn = dbmeals.Connections.Where(x => (x.UserId == UserID && x.ContactID == ContactUserID) || (x.UserId == ContactUserID && x.ContactID == UserID)).First();
            dbmeals.Connections.Remove(cn);
            dbmeals.SaveChanges();
            return RedirectToAction("Index", new { UserID = UserID });

        }

        public ActionResult Index(int UserID)
        {
            NetworkViewModel nvm = new NetworkViewModel();


            List<ContactsWaiting> contacts = (from p in dbmeals.ContactLists
                           where p.UserID == UserID && p.RequestAccepted == null
                                              group p by new { p.RecipientEmailAddress, p.RequestAccepted, p.UserID ,p.RecipientUserID} into g
                                              select new ContactsWaiting { EmailAddress = g.Key.RecipientEmailAddress, Accepted = g.Key.RequestAccepted, SenderUserID = g.Key.UserID, RecipientUserID = g.Key.RecipientUserID, Sender = 1 }).ToList(); ;

            
           List<ContactsWaiting> contacts1 = (from p in dbmeals.ContactLists
                                              where p.RecipientUserID == UserID && p.RequestAccepted == null
                                              group p by new { p.SenderEmailAddress, p.RequestAccepted, p.UserID, p.RecipientUserID } into g
                                              select new ContactsWaiting { EmailAddress = g.Key.SenderEmailAddress, Accepted = g.Key.RequestAccepted, SenderUserID = g.Key.UserID, RecipientUserID = g.Key.RecipientUserID, Sender = 0 }).ToList(); ;

            var cninner =  (from sa in dbmeals.Connections
            where sa.UserId == UserID && sa.DegreeOfSeparation==1
                        select new {ContactID = sa.ContactID,Email = sa.ContactEmail, SharesFood = sa.SharesFood, BoughtFoodFromUser = sa.BoughtFoodFromUser, SoldFoodToUser = sa.SoldFoodToUser }).ToArray();


            var prf = (from s in db.UserProfiles
                       select new { Name = s.FirstName, UserId = s.UserId }).ToArray();


            var InnerCircle = (from s in prf
                                             join sa in cninner on s.UserId equals sa.ContactID
                               select new InnerCircle { Name = s.Name, Email = sa.Email, SharesFood = sa.SharesFood, BoughtFoodFromUser = sa.BoughtFoodFromUser, SoldFoodToUser = sa.SoldFoodToUser }).ToArray();

            var cnouter = (from sa in dbmeals.Connections
                           where sa.UserId == UserID && sa.DegreeOfSeparation ==2
                           select new { ContactID = sa.ContactID, Email = sa.ContactEmail, SharesFood = sa.SharesFood, BoughtFoodFromUser = sa.BoughtFoodFromUser, SoldFoodToUser = sa.SoldFoodToUser }).ToArray();


            List<OuterCircle> OuterCircle = (from s in prf
                                             join sa in cnouter on s.UserId equals sa.ContactID
                                             select new OuterCircle { Name = s.Name, Email = sa.Email, SharesFood = sa.SharesFood, BoughtFoodFromUser = sa.BoughtFoodFromUser, SoldFoodToUser = sa.SoldFoodToUser }).ToList();

            var cnfood = (from sa in dbmeals.Connections
                           where sa.UserId == UserID && sa.DegreeOfSeparation > 2
                           select new { ContactID = sa.ContactID, Email = sa.ContactEmail, SharesFood = sa.SharesFood, BoughtFoodFromUser = sa.BoughtFoodFromUser, SoldFoodToUser = sa.SoldFoodToUser }).ToArray();

            List<FoodCircle> FoodCircle = (from s in prf
                                           join sa in cnfood on s.UserId equals sa.ContactID
                                           select new FoodCircle { Name = s.Name, Email = sa.Email, SharesFood = 1, BoughtFoodFromUser = 0, SoldFoodToUser = 1 }).ToList();
                                       
               
            contacts.AddRange(contacts1);
           
            nvm.Contacts = contacts;
            nvm.InnerCircleContacts = InnerCircle.ToList();
            nvm.OuterCircleContacts = OuterCircle;
            nvm.FoodCircleContacts = FoodCircle;

            return View(nvm);
        }

        //
        // GET: /Contact/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Contact/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Contact/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Contact/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Contact/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Contact/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Contact/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
