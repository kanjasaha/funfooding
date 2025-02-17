﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MealsToGo.Models;
using PagedList;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing;
using MealsToGo.Helpers;
using GeoCoding;
using SolrNet;
using WebMatrix.WebData;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Web.Routing;
using System.Data.Objects;
using MealsToGo.ViewModels;
using AutoMapper;
using Postal;
using Mvc.Mailer;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using MealsToGo.Mailers;
using MealsToGo.Repository;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace MealsToGo.Controllers
{
    public class UserController : Controller
    {
        private ThreeSixtyTwoEntities dbmeals = new ThreeSixtyTwoEntities();
        private UsersContext db = new UsersContext();
        private IUserRepository userRepository;
        static UserDetail userinfo;
        public static string strPhoto = "";
        static string Kname = "";
        static int KID = 0;
        static string InvaliImage = "";
        public UserController()
        {
            this.userRepository = new UserRepository(new ThreeSixtyTwoEntities());
        }

        [HttpPost]
        public bool RemovePhoto(int id = 0)
        {
            strPhoto = "Yes";
            var mealItems_Photos = dbmeals.UserDetails.FirstOrDefault(x => x.UserId == id);
            if (mealItems_Photos != null)
            {
                
                mealItems_Photos.Photo = string.Empty;
                db.SaveChanges();
            }
            return true;
        }
        //
        // GET: /User/

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var users = from u in dbmeals.UserDetails select u;
            if (!String.IsNullOrEmpty(searchString))
            {

                users = users.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                                   || s.FirstName.ToUpper().Contains(searchString.ToUpper()));

                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "Date desc" : "Date";

                if (Request.HttpMethod == "GET")
                {
                    searchString = currentFilter;
                }
                else
                {
                    page = 1;
                }
                ViewBag.CurrentFilter = searchString;
            }




            switch (sortOrder)
            {
                case "Name desc":
                    users = users.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    users = users.OrderBy(s => s.DateCreated);
                    break;
                case "Date desc":
                    users = users.OrderByDescending(s => s.DateCreated);
                    break;
                default:
                    users = users.OrderBy(s => s.LastName);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));
        }



        // GET: /User/Details/5

        public ActionResult Details(int userid = 0)
        {
            UserDetail userdetail = dbmeals.UserDetails.Find(userid);

            if (userdetail == null)
            {
                return RedirectToAction("Create", "User", new { userID = userid });
            }
            AddressList address = userdetail.AddressList;// dbmeals.AddressLists.Where(x => x.UserId == userid && x.IsCurrent == 1).FirstOrDefault();
            ViewBag.UserID = userid;

            UserDetailViewModel userinfovm = Mapper.Map<UserDetail, UserDetailViewModel>(userdetail);
            userinfovm.Address = Mapper.Map<AddressList, AddressViewModel>(address);
            if (userinfovm.Address != null && dbmeals.LKUPCountries.FirstOrDefault(x => x.CountryID == userinfovm.Address.CountryID) != null)
                userinfovm.Address.CountryName = dbmeals.LKUPCountries.FirstOrDefault(x => x.CountryID == userinfovm.Address.CountryID).Country;

           
            return View(userinfovm);
        }

        //

        private bool IsValidEmail(string email)
        {
            var attribute = new EmailAttribute();


            return attribute.IsValid(email);


        }

        private bool EmailExist(string email)
        {


            return db.UserProfiles.Any(x => x.UserName == email);


        }




        [HttpPost]
        public ActionResult Invite(EmailaddressesViewModel invitedemails)
        {
            int userid = invitedemails.UserID;
            
            if (ModelState.IsValid)
            {
               string invalidemails = "";
                string repeatrequests = "";
        
                char[] delimiters = (",: ").ToCharArray();

                List<string> emails = invitedemails.Emailaddresses.Split(delimiters).ToList();
                IUserMailer mailer = new UserMailer();
   
                EmailModel emailmodel = new EmailModel();
                emailmodel.From = db.UserProfiles.Where(x => x.UserId == userid).First().UserName;
                string SenderFirstName = db.UserProfiles.Where(x => x.UserId == userid).First().FirstName;

                foreach (var emailaddress in emails)
                {
                    string email = emailaddress;

                    bool EmailExists = db.UserProfiles.Any(x => x.UserName == email);
                    bool self = db.UserProfiles.Any(x => x.UserName == email && x.UserId == userid);
                    int RequestSentCount = dbmeals.ContactLists.Where(x => x.UserID == userid && x.RecipientEmailAddress == emailaddress).Count();

                    if (self)
                    {
                        
                    }
                    else if (RequestSentCount >= 2)
                    {
                        repeatrequests = repeatrequests + "," + emailaddress;
                    }
                    else if (EmailExists)
                    {

                        var Recipient = db.UserProfiles.Where(x => x.UserName == email).First();
                        emailmodel.To = emailaddress;
                        emailmodel.BCC = "kanjasaha@gmail.com";
                        emailmodel.Subject = "Join me at Funfooding";

                        StringBuilder sb = new StringBuilder();
                        sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
                        sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
                        sb.Append("<head>");
                        sb.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                        sb.Append("</head>");
                        sb.Append("<body>");
                        sb.Append("<div style=\"padding:20px; font:normal 14px Arial, Helvetica, sans-serif; color:#333333;\">");
                        sb.Append("Hi " + Recipient.FirstName.ToString() + ",<br />");
                        sb.Append("Connect with " + SenderFirstName + " at Funfooding!<br />");
                        sb.Append("<br />");
                        sb.Append("To Sign In click ");
                        sb.Append("<a href=" + ConfigurationManager.AppSettings["funfoodingUrl"] + "/Account/Login/" + " style=\"color:#0066CC\"> here</a>.<br />");

                        // sb.Append("<a href=" + ConfigurationManager.AppSettings["funfoodingUrl"] + "/Account/SignUp/" + confirmationToken + " style=\"color:#0066CC\"> here</a>.<br />");
                        sb.Append("<br />");
                        sb.Append("If you have any problem completing the process, please contact <a href=\"#\" style=\"color:#0066CC\">support@funfooding.com</a>.<br />");
                        sb.Append("<br /> ");
                        sb.Append("Best regards,<br />");
                        sb.Append("Support Team<br />");
                        sb.Append("<a href=\"http://funfooding.com/\" style=\"color:#0066CC\">www.funfooding.com</a></div>");
                        sb.Append("</body>");
                        sb.Append("</html>");
                        emailmodel.EmailBody = sb.ToString();
                        Common.sendeMail(emailmodel, EmailExist(emailaddress));
                        InsertRequestInfo(userid, emailmodel.From, emailmodel.To, Recipient.UserId);
                    }
                    else
                    {
                        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                        Match match = regex.Match(email);
                        if (!match.Success)
                      
                        {
                            TempData["emailAlert"] = "Please enter valid email address";
                            return RedirectToAction("Invite", "User", new { userid = userid });

                        }

                      
                        if (IsValidEmail(emailaddress) )
                        {

                            //string confirmationToken =
                            //   WebSecurity.CreateUserAndAccount(emailaddress, "Test12345", propertyValues: new
                            //   {
                            //       FirstName = "FirstName"
                            //   }, requireConfirmationToken: true); //new {Email=model.Email}
                            emailmodel.To = emailaddress;
                            emailmodel.BCC = "kanjasaha@gmail.com";
                            emailmodel.Subject = "Join me at Funfooding";
                            

                            StringBuilder sb = new StringBuilder();
                            sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
                            sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
                            sb.Append("<head>");
                            sb.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                            sb.Append("</head>");
                            sb.Append("<body>");
                            sb.Append("<div style=\"padding:20px; font:normal 14px Arial, Helvetica, sans-serif; color:#333333;\">");
                            sb.Append("Hi there,<br />");
                            sb.Append("Join Funfooding to find and share food in and around your neighorhood!<br />");
                            sb.Append("<br />");
                            sb.Append("To sign up click ");
                            sb.Append("<a href=" + ConfigurationManager.AppSettings["funfoodingUrl"] + "/Account/SignUp/" + " style=\"color:#0066CC\"> here</a>.<br />");

                            // sb.Append("<a href=" + ConfigurationManager.AppSettings["funfoodingUrl"] + "/Account/SignUp/" + confirmationToken + " style=\"color:#0066CC\"> here</a>.<br />");
                            sb.Append("<br />");
                            sb.Append("If you have any problem completing the process, please contact <a href=\"#\" style=\"color:#0066CC\">support@funfooding.com</a>.<br />");
                            sb.Append("<br /> ");
                            sb.Append("Best regards,<br />");
                            sb.Append("Support Team<br />");
                            sb.Append("<a href=\"http://funfooding.com/\" style=\"color:#0066CC\">www.funfooding.com</a></div>");
                            sb.Append("</body>");
                            sb.Append("</html>");
                            emailmodel.EmailBody = sb.ToString();

                            Common.sendeMail(emailmodel, EmailExist(emailaddress));
                            InsertRequestInfo(userid, emailmodel.From, emailmodel.To,null);
                        }
                        else
                        {
                            invalidemails = invalidemails + "," + emailaddress;


                        }

                    }

                }

                if (invalidemails == "" && repeatrequests=="")
                {

                    //AccountController acc = new AccountController();
                    //return RedirectPage(userid);
                    return RedirectToAction("Index", "Contact", new { userID = userid });

                }
                if (repeatrequests != "")

                    invitedemails.ErrorMessage = "Sorry, you cannot send another request.you have already requested these email address twice. " + repeatrequests;



                if (invalidemails != "")
                {
                    invitedemails.Emailaddresses = invalidemails;


                    invitedemails.ErrorMessage = invitedemails.ErrorMessage + "<br><br>Please correct these emailaddresses" + invalidemails;
                }


                TempData["emailAlert"] = invitedemails.ErrorMessage;
                return View(invitedemails);
            }
            else { return View(); }
        }

        private void InsertRequestInfo(int userid, string SenderEmailAddress, string EmailTo, int? recipientuserid)
        {
            try
            {
                ContactList ct = new ContactList();
                ct.UserID = userid;
                ct.SenderEmailAddress = SenderEmailAddress;
                ct.RecipientEmailAddress = EmailTo;
                ct.RecipientUserID = recipientuserid;
                dbmeals.ContactLists.Add(ct);
                dbmeals.SaveChanges();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }

       
        public int CheckFriend(ContactList cls)
        {

            var objUsrTbl = (from a in dbmeals.ContactLists
                             where a.UserID == cls.UserID && a.RecipientUserID == cls.RecipientUserID
                             select a).Count();
            return objUsrTbl;

        }

        public int FriendIDUpdate(ContactList cls)
        {

            int intResult = 0;

            try
            {
                ContactList objTblUser = new ContactList();
                objTblUser.UserID = cls.UserID;
                objTblUser.RecipientUserID = cls.RecipientUserID;
                objTblUser.RequestAccepted = 1;

                dbmeals.ContactLists.Add(objTblUser);
                intResult = dbmeals.SaveChanges();
                return intResult;
            }
            catch (Exception ex)
            {
                return intResult;

            }


        }

        public RedirectToRouteResult RedirectPage(int userid)
        {
            UserDetail userdetail = new UserDetail();
            userdetail = dbmeals.UserDetails.Where(x => x.UserId.Equals(userid)).FirstOrDefault();
            if (userdetail != null)
            {


                {

                    //   searchparam.UserLoc.Latitude = Convert.ToDouble(userdetail.AddressList.Latitude);
                    //  searchparam.UserLoc.Longitude = Convert.ToDouble(userdetail.AddressList.Longitude);
                    //  searchparam.UserID = userdetail.UserId;
                    Session["UserLoc"] = Common.GetFullAddress(userdetail.AddressList);
                    Session["UserLocLat"] = Convert.ToDouble(userdetail.AddressList.Latitude);
                    Session["UserLocLong"] = Convert.ToDouble(userdetail.AddressList.Longitude);


                    if (Session["UserLocLat"] == null)

                        return RedirectToAction("LocationToSearch", "Home", null);

                    else
                    {

                        SearchParam searchparam = new SearchParam();
                        return RedirectToAction("Index", "Home", new RouteValueDictionary(searchparam));

                    }
                }
            }
            else
            {

                return RedirectToAction("Create", "User", new { userID = userid });
            }
        }


        public ActionResult Invite(int userID)
        {
            EmailaddressesViewModel invitedemails = new EmailaddressesViewModel();
            invitedemails.UserID = userID;
           

            return View(invitedemails);
        }

        public ActionResult AcceptAgreement(int userID)
        {
            UserDetail detail = new UserDetail();
            detail.UserId = userID;

            UserAcceptanceViewModel uacceptvm = new UserAcceptanceViewModel();
            uacceptvm.UserId = userID;
            UserAgreement ua = dbmeals.UserAgreements.Where(x => x.Name == "Seller Agreement").FirstOrDefault();
            uacceptvm.UserAgreement = ua.AgreementDetails;
            uacceptvm.AgreementID = ua.AgreementID;
            return View(uacceptvm);
        }

        [HttpPost]
        public ActionResult AcceptAgreement(UserAcceptanceViewModel uacceptvm)
        {
            UserAgreementsAcceptanceDetail uaad = new UserAgreementsAcceptanceDetail();
            uaad.UserID = uacceptvm.UserId;
            uaad.AgreementID = uacceptvm.AgreementID;
            uaad.AgreementAccepetedOn = DateTime.Now;
            dbmeals.UserAgreementsAcceptanceDetails.Add(uaad);
            dbmeals.SaveChanges();

            return RedirectToAction("Index", "MealAd", new { userID = uacceptvm.UserId });

        }





        //public ActionResult UserPreference(int userID)
        //{
        //    UserPreference userpref = new UserPreference();
        //    userpref.UserId = userID;


        //    return View(userpref);
        //}


        //[HttpPost]
        //public ActionResult UserPreference(UserPreference userpref)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        dbmeals.UserPreferences.Add(userpref);
        //    }


        //    return View(userpref);
        //}




        //
        // POST: /User/Create


        [HttpPost]
        public ActionResult Create(UserDetailViewModel userinfovm, HttpPostedFileBase Photo)
        {
            if (!userinfovm.CheckIfSeller)
            {
                
                ModelState.Remove("KitchenName");
            }
            if (ModelState.IsValid)
            {

                var fileName = "";

                // Verify that the user selected a file
                if (Photo != null && Photo.ContentLength > 0)
                {
                    // extract only the fielname
                    fileName = "Profilephoto" + userinfovm.UserId + Path.GetExtension(Photo.FileName);
                    //   .GetFileName(Photo.FileName).;
                    // store the file inside ~/App_Data/uploads folder
                    var path = Path.Combine(Server.MapPath("~/ProfilePhotos"), fileName);
                    Photo.SaveAs(path);
                    try
                    {
                        System.Drawing.Image.FromFile(fileName);
                    }
                    catch (Exception e1)
                    {
                        ModelState.AddModelError("", "Please upload Image only. " + e1.Message.ToString());


                    }
                    userinfovm.Photo = fileName;
                }
                try
                {

                    string Country = dbmeals.LKUPCountries.Where(x => x.CountryID == userinfovm.Address.CountryID).First().Country;
                    string address = userinfovm.Address.Address1 + "," + userinfovm.Address.Telephone + "," + userinfovm.Address.Address2 + "," + userinfovm.Address.City + "," + userinfovm.Address.Province + "," + userinfovm.Address.Zip + "," + Country;

                    GLatLong latlng = GetLatLng(address);
                    userinfovm.Address.Latitude = (decimal)latlng.Latitude;
                    userinfovm.Address.Longitude = (decimal)latlng.Longitude;
                    // userinfo.AddressList.DateCreated = DateTime.Now;


                    AddressList addr = Mapper.Map<AddressViewModel, AddressList>(userinfovm.Address);
                    addr.UserId = userinfovm.UserId;
                    addr.DateCreated = DateTime.Now;
                    addr.Telephone = userinfovm.Address.Telephone;
                    dbmeals.AddressLists.Add(addr);
                    dbmeals.SaveChanges();

                   
                    UserDetail userinfo = Mapper.Map<UserDetailViewModel, UserDetail>(userinfovm);
                   
                    userinfo.KitchenName = userinfovm.KitchenName;
                    
                    int last_insert_id = addr.AddressID;
                    userinfo.AddressID = last_insert_id;
                    userinfo.DateCreated = DateTime.Now;

                    dbmeals.UserDetails.Add(userinfo);
                    dbmeals.SaveChanges();


                }
                catch (DbEntityValidationException dbmealsEx)
                {
                    foreach (var validationErrors in dbmealsEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
                // redirect back to the index action to show the form once again

                //  return RedirectToAction("InviteYourConnections", "User", new { userid = userinfovm.UserId });
                return RedirectToAction("Details", "User", new { userid = userinfovm.UserId });

            }
            else
            {
                userinfovm.CountryDDList = dbmeals.LKUPCountries.ToList().Select(x => new SelectListItem
                {
                    Value = x.CountryID.ToString(),
                    Text = x.Country.ToString()
                });

               

                return View(userinfovm);
            }
            //return View(userinfovm);

        }



        // GET: /User/Create

        public ActionResult Create(int userID)
        {
            UserDetailViewModel user = new UserDetailViewModel();
            user.CreatedBy = userID;
            user.UpdatedBy = userID;
            user.UserId = userID;
            user.CheckIfSeller = true;
            user.FirstName = db.UserProfiles.Where(x => x.UserId.Equals(userID)).First().FirstName.ToString();// System.Web.HttpContext.Current.Session["FirstName"].ToString();


            user.CountryDDList = dbmeals.LKUPCountries.ToList().Select(x => new SelectListItem
            {
                Value = x.CountryID.ToString(),
                Text = x.Country.ToString()
            });

           
            return View(user);
        }

        public ActionResult ShowDemo(int userID)
        {
            ViewBag.UserId = userID;



            return View();
        }

        public GLatLong GetLatLng(string address)
        {


            //IGeoCoder geoCoder = new GoogleGeoCoder("my-api-key");
            //Address[] addresses = geoCoder.GeoCode("123 Main St");

            //IGeoCoder geoCoder = new YahooGeoCoder("my-app-ID");
            // addresses = geoCoder.GeoCode(38.8976777, -77.036517);

            GLatLong loc = new GLatLong();
            var c = GeoCodingHelper.GetLatLong(address);
            if (c != null)
            {
                loc.Latitude = c.Latitude;
                loc.Longitude = c.Longitude;
            }
            return loc;
        }



        public ActionResult LocateAddress(string address)
        {

            //address = "1 whitehall street new york ny 10004";

            //IGeoCoder geoCoder = new GoogleGeoCoder("my-api-key");
            //Address[] addresses = geoCoder.GeoCode("123 Main St");

            //IGeoCoder geoCoder = new YahooGeoCoder("my-app-ID");
            // addresses = geoCoder.GeoCode(38.8976777, -77.036517);

            GLatLong loc = new GLatLong();
            var c = GeoCodingHelper.GetLatLong(address);

            loc.Latitude = c.Latitude;
            loc.Longitude = c.Longitude;

            string revgeocode = "";

            var g = GeoCodingHelper.GetAddress(loc);
            revgeocode = g.ToString();

            return View(loc);
        }






        // GET: /UserInfo/Edit/5

        public ActionResult Edit(int userid)
        {

          
            
            userinfo = dbmeals.UserDetails.Find(userid);
            ViewBag.CountryDDList = dbmeals.LKUPCountries.ToList().Select(x => new SelectListItem
             {
                 Value = x.CountryID.ToString(),
                 Text = x.Country.ToString(),
                 Selected = (userinfo != null && x.CountryID == userinfo.AddressList.CountryID)
             });
          //  Kname = userinfo.KitchenName;
          //  KID = Convert.ToInt32(userinfo.KitchenTypeID);
            if (userinfo == null)
            {
                return RedirectToAction("Create", "User", new { userID = userid });
            }
            return View(userinfo);
        }


        //
        // POST: /UserInfo/Edit/5
        private bool IsImage(HttpPostedFileBase file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" }; // add more if u like...

            foreach (var item in formats)
            {
                if (file.FileName.Contains(item))
                {
                    return true;
                }
            }

            return false;
        } 
        [HttpPost]
        public ActionResult Edit(UserDetail currentinfo, HttpPostedFileBase Photo,string phone)
        {
            if (ModelState.IsValid)
            {
               
                //currentinfo.KitchenName = currentinfo.KitchenName;
                currentinfo.KitchenName = Kname;
                var fileName = "";
                // Verify that the user selected a file
                if (Photo != null && Photo.ContentLength > 0)
                {
                    // extract only the fielname
                    fileName = "Profilephoto" + currentinfo.UserId + Path.GetExtension(Photo.FileName);
                    
                    //   .GetFileName(Photo.FileName).;
                    // store the file inside ~/App_Data/uploads folder
                    var path = Path.Combine(Server.MapPath("~/ProfilePhotos"), fileName);
                    Photo.SaveAs(path);
                }
                if (currentinfo.AddressList != null)
                {
                    currentinfo.AddressID = currentinfo.AddressList.AddressID;
                }
            
              //  currentinfo.AddressList.Telephone = currentinfo.AddressList.Telephone;
                if (!String.IsNullOrEmpty(fileName))
                {
                    if (IsImage(Photo))
                    {
                    
                    }else{
                        TempData["userAlert"] = "Please select image file only";
                        InvaliImage = "Yes";
                        if (strPhoto == "Yes")
                        {
                           
                                currentinfo.Photo = "";
                                strPhoto = "";
                           
                        }
                        else
                            currentinfo.Photo = fileName;
                        dbmeals.Entry(currentinfo).State = System.Data.EntityState.Modified;
                        dbmeals.SaveChanges();
                    
                        return RedirectToAction("Edit", "User", new { userID = currentinfo.UserId });
                    }
                    if (strPhoto == "Yes")
                    {
                        if (InvaliImage == "Yes")
                        {
                            InvaliImage = "";
                        }
                        else
                        {
                            currentinfo.Photo = "";
                            strPhoto = "";
                        }
                    }
                    else
                    currentinfo.Photo = fileName;
                }
                else
                {
                    if (strPhoto == "Yes")
                    {
                        if (InvaliImage == "Yes")
                        {
                            InvaliImage = "";
                        }
                        else { 
                        currentinfo.Photo = "";
                        strPhoto = "";
                        }
                    }
                    else
                    {
                        currentinfo.Photo = fileName;
                        string str = userinfo.Photo;
                        currentinfo.Photo = str;
                    }
                }
                if (strPhoto == "Yes")
                {
                    if (InvaliImage == "Yes")
                    {
                        InvaliImage = "";
                    }
                    else
                    {
                        currentinfo.Photo = "";
                        strPhoto = "";
                    }
                }
                else
                    currentinfo.Photo = fileName;
                dbmeals.Entry(currentinfo).State = System.Data.EntityState.Modified;
                dbmeals.SaveChanges();

                string str1 = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(str1);
                con.Open();
                SqlCommand cmd = new SqlCommand("Update AddressList set Telephone='" + currentinfo.AddressList.Telephone + "' where AddressID='"+currentinfo.AddressID+"'",con);
                cmd.ExecuteNonQuery();
                con.Close();

            

                return RedirectToAction("Details", "User", new { userID = currentinfo.UserId });
            }
            return View();
        }
        // GET: /Default1/DeactivateUser/5

        public ActionResult DeactivateUser(int id = 0)
        {
            UserDetail userinfo = dbmeals.UserDetails.Find(id);
            if (userinfo == null)
            {
                return HttpNotFound();
            }
            return View(userinfo);
        }

        //
        // POST: /Default1/Edit/5

        [HttpPost]
        public ActionResult DeactivateUser(UserDetail currentinfo)
        {
            if (ModelState.IsValid)
            {

                var userinfo = dbmeals.UserDetails.Find(currentinfo.UserId);


                userinfo.DateUpdated = DateTime.Now;
                userinfo.UserStatus = 0;
                userinfo.Notes = currentinfo.Notes;

                //dbmeals.Entry(userinfo).State = System.Data.EntityState.Modified;

                dbmeals.SaveChanges();
                return RedirectToAction("Details", "User", new { userID = currentinfo.UserId });
            }
            return View(currentinfo);
        }

        // GET: /Default1/DeactivateUser/5

        public ActionResult ActivateUser(int id = 0)
        {
            UserDetail userinfo = dbmeals.UserDetails.Find(id);


            userinfo.DateUpdated = DateTime.Now;
            userinfo.UserStatus = 1;


            //dbmeals.Entry(userinfo).State = System.Data.EntityState.Modified;

            dbmeals.SaveChanges();
            return RedirectToAction("Details", "User", new { userID = id });

        }

        public ActionResult Thumbnail(int width, int height, string thumnailphoto)
        {
            // TODO: the filename could be passed as argument of course
            var imageFile = Path.Combine(Server.MapPath("~/ProfilePhotos"), thumnailphoto);
            using (var srcImage = System.Drawing.Image.FromFile(imageFile))
            using (var newImage = new Bitmap(width, height))
            using (var graphics = Graphics.FromImage(newImage))
            using (var stream = new MemoryStream())
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(srcImage, new Rectangle(0, 0, width, height));
                // newImage.Save("thumnailphoto" + thumnailphoto,System.Drawing.Imaging.ImageFormat.Jpeg);
                new Bitmap(newImage).Save("thumnailphoto" + thumnailphoto);

                return File(stream.ToArray(), "image/png");
            }
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int userid = 0)
        {
            UserDetail userdetail = dbmeals.UserDetails.Find(userid);
            dbmeals.UserDetails.Remove(userdetail);
            dbmeals.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserDetail userdetail = dbmeals.UserDetails.Find(id);
            dbmeals.UserDetails.Remove(userdetail);
            dbmeals.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Locate()
        {
            return View();
        }
        public ActionResult Suggestions()
        {
            return View();
        }



        protected override void Dispose(bool disposing)
        {
            dbmeals.Dispose();
            base.Dispose(disposing);
        }
    }
}