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
            dbmeals.SaveChanges();
            return RedirectToAction("Index");


        }
         [HttpPost]
        public ActionResult RejectRequest(int RecipientUserID, int SenderUserID)
        {

            ContactList contact = dbmeals.ContactLists.Where(x => (x.UserID == RecipientUserID && x.RecipientUserID == SenderUserID) || (x.UserID == SenderUserID && x.RecipientUserID == RecipientUserID)).First();

            contact.RequestAccepted = 2;
            dbmeals.Entry(contact).State = EntityState.Modified;
            dbmeals.SaveChanges();
            return RedirectToAction("Index");


        }
         [HttpPost]
        public ActionResult Remove(int UserID, int ContactUserID)
        {
            ContactList contact = dbmeals.ContactLists.Where(x => (x.UserID == UserID && x.RecipientUserID == ContactUserID) || (x.UserID == ContactUserID && x.RecipientUserID == UserID)).First();
            dbmeals.ContactLists.Remove(contact);
            dbmeals.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult Index(int UserID)
        {

            List<Contact> contacts = (from p in dbmeals.ContactLists
                                            where p.UserID == UserID
                                            select new Contact{ Name=p.RecipientEmailAddress,Accepted=p.RequestAccepted,Sender=1 }).ToList();

            List<Contact> contacts1 = (from p in dbmeals.ContactLists
                                      where p.RecipientUserID == UserID
                                      select new Contact { Name = p.SenderEmailAddress, Accepted = p.RequestAccepted,Sender=0 }).ToList();
                
            
            contacts.Concat(contacts1);
            return View(contacts);
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
