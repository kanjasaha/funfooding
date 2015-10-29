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
       [HttpPost]
        public ActionResult AcceptRequest(int RecipientUserID, int SenderUserID)
        {
            ContactList contact = dbmeals.ContactLists.Where(x => (x.UserID == RecipientUserID && x.RecipientUserID == SenderUserID) || (x.UserID == SenderUserID && x.RecipientUserID == RecipientUserID)).First();

            
            contact.RequestAccepted = 1;
            dbmeals.Entry(contact).State = EntityState.Modified;

            Connection cn1 = new Connection();
            cn1.UserId = SenderUserID;
            cn1.ContactID = RecipientUserID;
            cn1.DegreeOfSeparation = 1;
            dbmeals.Connections.Add(cn1);

            Connection cn2 = new Connection();
            cn2.UserId = RecipientUserID;
            cn2.ContactID = SenderUserID;
            cn2.DegreeOfSeparation = 1;
            dbmeals.Connections.Add(cn2);

            dbmeals.SaveChanges();
            return RedirectToAction("Index");


        }
         [HttpPost]
        public ActionResult RejectRequest(int RecipientUserID, int SenderUserID)
        {

            ContactList contact = dbmeals.ContactLists.Where(x => (x.UserID == RecipientUserID && x.RecipientUserID == SenderUserID) || (x.UserID == SenderUserID && x.RecipientUserID == RecipientUserID)).First();

            contact.RequestAccepted = 0;
            dbmeals.Entry(contact).State = EntityState.Modified;
            dbmeals.SaveChanges();
            return RedirectToAction("Index");


        }
         [HttpPost]
        public ActionResult Remove(int UserID, int ContactUserID)
        {
            ContactList contact = dbmeals.ContactLists.Where(x => (x.UserID == UserID && x.RecipientUserID == ContactUserID) || (x.UserID == ContactUserID && x.RecipientUserID == UserID)).First();
            contact.RequestAccepted = 0;
            dbmeals.Entry(contact).State = EntityState.Modified;
            Connection cn = dbmeals.Connections.Where(x => (x.UserId == UserID && x.ContactID == ContactUserID) || (x.UserId == ContactUserID && x.ContactID == UserID)).First();
            dbmeals.Connections.Remove(cn);
            dbmeals.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult Index(int UserID)
        {
            NetworkViewModel nvm = new NetworkViewModel();


            List<ContactsWaiting> contacts = (from p in dbmeals.ContactLists
                                            where p.UserID == UserID && p.RequestAccepted ==null
                                      select new ContactsWaiting { EmailAddress = p.RecipientEmailAddress, Accepted = p.RequestAccepted, Sender = 1 }).ToList();


           List<ContactsWaiting> contacts1 = (from p in dbmeals.ContactLists
                                              where p.RecipientUserID == UserID && p.RequestAccepted == null
                                      select new ContactsWaiting { EmailAddress = p.RecipientEmailAddress, Accepted = p.RequestAccepted, Sender = 0 }).ToList();

            List<InnerCircle> InnerCircle = (from s in dbmeals.UserDetails
                                                    join sa in dbmeals.Connections on s.UserId equals sa.UserId
                                                where sa.UserId == UserID && sa.DegreeOfSeparation==1
                                             select new InnerCircle { Name = s.FirstName + " " + s.LastName, SharesFood = 1, BoughtFoodFromUser = 0, SoldFoodToUser = 1 }).ToList();
            List<InnerCircle> InnerCircle1 = (from s in dbmeals.UserDetails
                                              join sa in dbmeals.Connections on s.UserId equals sa.ContactID
                                             where sa.ContactID == UserID && sa.DegreeOfSeparation==1
                                             select new InnerCircle { Name = s.FirstName + " " + s.LastName, SharesFood = 1, BoughtFoodFromUser = 0, SoldFoodToUser = 1 }).ToList();
            List<OuterCircle> OuterCircle = (from s in dbmeals.UserDetails
                                             join sa in dbmeals.Connections on s.UserId equals sa.UserId
                                             where sa.UserId == UserID && sa.DegreeOfSeparation>1
                                             select new OuterCircle { Name = s.FirstName + " " + s.LastName, SharesFood = 1, BoughtFoodFromUser = 0, SoldFoodToUser = 1 }).ToList();
            List<OuterCircle> OuterCircle1 = (from s in dbmeals.UserDetails
                                              join sa in dbmeals.Connections on s.UserId equals sa.ContactID
                                              where sa.ContactID == UserID && sa.DegreeOfSeparation > 1
                                              select new OuterCircle { Name = s.FirstName + " "+ s.LastName, SharesFood = 1, BoughtFoodFromUser = 0, SoldFoodToUser = 1 }).ToList();
            List<Business> Business = (from s in dbmeals.UserDetails
                                             join sa in dbmeals.Connections on s.UserId equals sa.UserId
                                             where sa.UserId == UserID
                                       select new Business { Name = s.FirstName + " " + s.LastName, SharesFood = 1, BoughtFoodFromUser = 0, SoldFoodToUser = 1 }).ToList();
            List<Business> Business1 = (from s in dbmeals.UserDetails
                                              join sa in dbmeals.Connections on s.UserId equals sa.ContactID
                                              where sa.ContactID == UserID
                                        select new Business { Name = s.FirstName + " " + s.LastName, SharesFood = 1, BoughtFoodFromUser = 0, SoldFoodToUser = 1 }).ToList();
                                         
               
            contacts.AddRange(contacts1);
            InnerCircle.AddRange(InnerCircle1);
            OuterCircle.AddRange(OuterCircle1);
            Business.Concat(Business1);

            nvm.Contacts = contacts;
            nvm.InnerCircleContacts = InnerCircle;
            nvm.OuterCircleContacts = OuterCircle;
            nvm.BusinessContacts = Business;

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
