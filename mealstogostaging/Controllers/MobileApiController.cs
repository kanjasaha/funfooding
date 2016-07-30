using AutoMapper;
using MealsToGo.Helpers;
using MealsToGo.Models;
using MealsToGo.Service;
using MealsToGo.ViewModels;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.DSL;
using SolrNet.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using System.Data;
using System.Data.SqlClient;
using System.Web.Routing;
using Microsoft.Web.WebPages.OAuth;
using System.IO;
using System.Text.RegularExpressions;
using DataAnnotationsExtensions;

namespace MealsToGo.Controllers
{
    public class MobileApiController : Controller
    {
        // private readonly IMealItemService _service;


        // public MobileApiController()
        //{
        //    _service = new MealItemService();
        //}
        /// <summary>
        /// All selectable facet fields
        /// </summary>
        /// 
        private static readonly string[] AllFacetFields = new[] { "Cuisine", "Provider", "Diet", "PriceRange", "Meal" };
        private readonly ISolrReadOnlyOperations<SolrResultSet> solr;
        private ThreeSixtyTwoEntities dbmeals = new ThreeSixtyTwoEntities();
        private UsersContext db = new UsersContext();
        static string InvaliImage = "";
         public static string strPhoto = "";
         static UserDetail userinfo;
        public MobileApiController(ISolrReadOnlyOperations<SolrResultSet> solr)
        {
            this.solr = solr;

        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        [HttpGet]
        // [AllowAnonymous]
        public JsonResult test()
        {

            LoginRegisterViewModel testa = new LoginRegisterViewModel();
            testa.FirstName = "Kanja";
            testa.UserName = "kanjasaha@gmail.com";
            testa.Password = "test1234";
            testa.RememberMe = true;
            return Json(testa, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult EmailConfirmation(string id)
        {
            if (WebSecurity.ConfirmAccount(id))
            {
                //get the userid from confirmationtoken.
                //then get the username or emailaddress for this account
                //update contactlist with this userid for this emailaddress/username
                UserSetting us = new UserSetting();
                us.ReceiveEmailNotificationID = 3;
                us.ReceiveMobileTextNotificationID = 1;

                us.PrivacySettingsID = 3;

                us.UserID = dbmeals.webpages_Membership.Where(x => x.ConfirmationToken == id).FirstOrDefault().UserId;
                string username = db.UserProfiles.Where(n => n.UserId == us.UserID).FirstOrDefault().UserName;
                ContactList contact = dbmeals.ContactLists.Where(x => x.RecipientEmailAddress == username).FirstOrDefault();

                if (contact != null)
                {
                    contact.RecipientUserID = us.UserID;
                    dbmeals.Entry(contact).State = EntityState.Modified;
                }

                dbmeals.UserSettings.Add(us);

                dbmeals.SaveChanges();



                string userName = db.UserProfiles.Where(x => x.UserId == us.UserID).FirstOrDefault().UserName;

                //LoginRegisterViewModel viewmodel=new LoginRegisterViewModel();
                //viewmodel.FirstName=lm.FirstName;
                // viewmodel.UserName=lm.UserName;
                //viewmodel.Password=dbmeals.webpages_Membership.Where(x => x.ConfirmationToken == id).FirstOrDefault().Password;
                // return Login(viewmodel,null);
                FormsAuthentication.SetAuthCookie(userName, true);
                string returnUrl = "";
                returnUrl = RedirectPage(us.UserID);
                return Json(new { success = true, Url = returnUrl }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { success = false, msg = "Confirmation Failure" }, JsonRequestBehavior.AllowGet);
        }

        private string RedirectPage(int userid)
        {
            var redirectionUrl=""; 
            bool userinvited = dbmeals.ContactLists.Any(x => x.RecipientUserID == userid);
            if (userinvited)
            {

                redirectionUrl = Url.Action("Index", "Contact", new { userid = userid });
                return redirectionUrl;

            }
            string ipaddress = Request.ServerVariables["REMOTE_ADDR"];
            ipaddress = ipaddress == null ? String.Empty : ipaddress;
            ipaddress = ipaddress.Replace("\r\n", ",");
            ipaddress = ipaddress.Replace(" ", ",");
            ipaddress = ipaddress.Replace(":", "");
            string[] arripaddress = ipaddress.Split(',');
            string location = string.Empty;
            string address = String.Empty;
            if (arripaddress.Length != 0)
            {
                for (int i = 0; i <= arripaddress.Length - 1; i++)
                {
                    if (arripaddress[i] != "")
                    {
                        // check if the IP address is supported in demo version
                        long ipno = Common.IP2Decimal(arripaddress[i]);
                        if ((ipno > 0) || (ipno < 33554431))
                        {
                            SqlDataReader reader;
                            // select MS-SQL database using DSNless connection
                            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                            // query string to lookup the country by matching the range of IP address number
                            SqlCommand sqlCmd = new SqlCommand("SELECT TOP 1 * FROM ip2location_db3_ipv6 WHERE " + ipno.ToString() + " <= ip_to ORDER BY ip_to", sqlConn);
                            sqlCmd.Connection.Open();
                            // execute the query
                            reader = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                            // display results
                            if (reader.Read())
                            {
                                location = Convert.ToString(reader["city_name"]);
                                address = Convert.ToString(reader["city_name"]) + "," + Convert.ToString(reader["region_name"]) + "," + Convert.ToString(reader["country_name"]);

                              
                            }
                            sqlCmd.Connection.Close();
                        }
                    }
                }
            }
            var user = db.UserProfiles.Where(x => x.UserId.Equals(userid)).First();
            Session["FirstName"] = user.FirstName;
            LocationsSearched ls = new LocationsSearched();
            ls = dbmeals.LocationsSearcheds.Where(x => x.UserID.Equals(userid)).OrderByDescending(y => y.DateCreated).FirstOrDefault();
            if (ls != null)
            {
                Session["UserLoc"] = ls.Location;
                Session["UserLocLat"] = Convert.ToDouble(ls.Latitude);
                Session["UserLocLong"] = Convert.ToDouble(ls.Longitude);
            }
            else
            {
                if (!string.IsNullOrEmpty(location))
                {
                    Session["UserLoc"] = location;

                    GLatLong latlng = Common.GetLatLng(address);
                    Session["UserLocLat"] = (decimal)latlng.Latitude;
                    Session["UserLocLong"] = (decimal)latlng.Longitude;
                }
                else
                {
                    

                    redirectionUrl = Url.Action("LocationToSearch", "Home", null);
                    return redirectionUrl;
                }
            }

            if (Session["UserLoc"] == null)
            {


                UserDetail userdetail = new UserDetail();
                userdetail = dbmeals.UserDetails.Where(x => x.UserId.Equals(userid)).FirstOrDefault();
                if (userdetail != null)
                {
                    Session["UserLoc"] = Common.GetFullAddress(userdetail.AddressList);
                    Session["UserLocLat"] = Convert.ToDouble(userdetail.AddressList.Latitude);
                    Session["UserLocLong"] = Convert.ToDouble(userdetail.AddressList.Longitude);
                }
                else
                    Session["UserLoc"] = null;

                if (Session["UserLocLat"] == null)
                {
                    redirectionUrl = Url.Action("LocationToSearch", "Home", null);
                    return redirectionUrl;
                }

            }
            SearchParam searchparam = new SearchParam();
            redirectionUrl = Url.Action("Index", "Home", new RouteValueDictionary(searchparam));
            return redirectionUrl;

        }


        [HttpPost()]
        // [AllowAnonymous]
       // [ValidateAntiForgeryToken]
        public JsonResult Register(LoginRegisterViewModel viewmodel)
        {
            bool res = true;
            //return Json(viewmodel, JsonRequestBehavior.AllowGet);
            try
            {



                LoginModel model = new LoginModel();
                model = Mapper.Map<LoginRegisterViewModel, LoginModel>(viewmodel);
                
                if ((model.FirstName == "") || (model.FirstName == null))
                {
                    return Json(new { success = false, msg = "First Name is required" }, JsonRequestBehavior.AllowGet);
                }


                bool userexists = db.UserProfiles.Any(x => x.UserName.Equals(model.UserName));

                if (userexists)
                {

                    return Json(new { success = false, msg = "There is already an account with that email. Please use a different email" }, JsonRequestBehavior.AllowGet);
                }

               



                // Attempt to register the user

                string confirmationToken =
                    WebSecurity.CreateUserAndAccount(viewmodel.UserName, viewmodel.Password, propertyValues: new
                    {
                        FirstName = viewmodel.FirstName
                    }, requireConfirmationToken: true); //new {Email=model.Email}


                EmailModel emailmodel = new EmailModel();
                emailmodel.To = viewmodel.UserName;
                emailmodel.Subject = "Welcome to Fun Fooding";


                StringBuilder sb = new StringBuilder();
                sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
                sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
                sb.Append("<head>");
                sb.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                sb.Append("</head>");
                sb.Append("<body>");
                sb.Append("<div style=\"padding:20px; font:normal 14px Arial, Helvetica, sans-serif; color:#333333;\">");
                sb.Append("Hi " + viewmodel.FirstName + ",<br />");
                sb.Append("Thank you for signing up!<br />");
                sb.Append("<br />");
                sb.Append("To complete the sign up process click ");
                sb.Append("<a href=" + ConfigurationManager.AppSettings["funfoodingUrl"] + "/MobileApi/EmailConfirmation/" + confirmationToken + " style=\"color:#0066CC\"> here</a>.<br />");
                sb.Append("<br />");
                sb.Append("If you have any problem completing the process, please contact <a href=\"#\" style=\"color:#0066CC\">support@gmail.com</a>.<br />");
                sb.Append("<br /> ");
                sb.Append("Best regards,<br />");
                sb.Append("Support Team<br />");
                sb.Append("<a href=\"http://funfooding.com/\" style=\"color:#0066CC\">www.funfooding.com</a></div>");
                sb.Append("</body>");
                sb.Append("</html>");
                emailmodel.EmailBody = sb.ToString();
                Common.sendeMail(emailmodel, true);
                return Json(new { success = res }, JsonRequestBehavior.AllowGet);
            }
                catch (Exception e)
                {
                    res = false;
                    //ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                    return Json(new { success=res, msg = e.Message.ToString()}, JsonRequestBehavior.AllowGet);
                }
            

            // If we got this far, something failed, redisplay form
            //return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost()]

        public JsonResult Login(LoginRegisterViewModel viewmodel, string returnUrl)
        {

            bool res = false;

            LoginModel model = new LoginModel();
            model = Mapper.Map<LoginRegisterViewModel, LoginModel>(viewmodel);
            if (model.UserName == "" && model.Password == "")
            {
                 return Json(new { success = res, msg = "Please enter username and password" }, JsonRequestBehavior.AllowGet);

            }
            try
            {
                if (WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
                {

                    var user = db.UserProfiles.Where(x => x.UserName.Equals(model.UserName)).First();
                    int UserID = user.UserId;
                    Session["FirstName"] = user.FirstName;
                    bool UserExists1 = db.UserProfiles.Any(x => x.UserName.Equals(model.UserName));
                    if (UserExists1)
                    {
                        var Username = new HttpCookie("Username");
                        var Password = new HttpCookie("Password");
                        if (model.RememberMe)
                        {

                            Username.Value = model.UserName;
                            Password.Value = model.Password;
                            Username.Expires = DateTime.Now.AddDays(7);
                            Password.Expires = DateTime.Now.AddDays(7);

                            Response.Cookies.Add(Username);
                            Response.Cookies.Add(Password);
                            //  return View(viewmodel);
                        }
                        else
                        {
                            Username.Value = "";
                            Password.Value = "";

                            Username.Expires = DateTime.Now.AddDays(7);
                            Password.Expires = DateTime.Now.AddDays(7);

                            Response.Cookies.Add(Username);
                            Response.Cookies.Add(Password);
                        }

                    }
 
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        bool UserExists12 = db.UserProfiles.Any(x => x.UserName.Equals(model.UserName));
                        if (UserExists12)
                        {
           
                        }
                        else
                        {
                           
                            return Json(new { success = res, msg = "This username does not have an account with us at Funfooding. Please try again with correct username or signup." }, JsonRequestBehavior.AllowGet);

                        }


                        return Json(new { success = true, UserId = UserID, UserName = model.UserName, FirstName = user.FirstName, Url = returnUrl }, JsonRequestBehavior.AllowGet);
                        
                    }
                    else
                    {
                        bool UserExists2 = db.UserProfiles.Any(x => x.UserName.Equals(model.UserName));
                        if (UserExists2)
                        {

                            }
                        else
                        {
                              return Json(new { success = res, msg = "This username does not have an account with us at Funfooding. Please try again with correct username or signup." }, JsonRequestBehavior.AllowGet);

                        }
                        returnUrl = RedirectPage(UserID);
                        return Json(new { success = true, UserId = UserID, UserName = model.UserName, FirstName = user.FirstName, Url = returnUrl }, JsonRequestBehavior.AllowGet);
                     
                    }

                }
            }
            catch (Exception e1)
            {
               
                return Json(new { success = res, msg = "Please enter username and password" }, JsonRequestBehavior.AllowGet);
    }
            bool UserExists = db.UserProfiles.Any(x => x.UserName.Equals(model.UserName));
            if (UserExists)
            {

                return Json(new { success = res, msg = "The user name or password provided is incorrect." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = res, msg = "This username does not have an account with us at Funfooding. Please try again with correct username or signup." }, JsonRequestBehavior.AllowGet);
            }
            
        }

        private bool EmailExist(string email)
        {


            return db.UserProfiles.Any(x => x.UserName == email);


        }


         [HttpPost()]
        public JsonResult ResetPassword(ResetPasswordModel objResetPasswordModel)
        {

            if (ModelState.IsValid)
            {

                if (EmailExist(objResetPasswordModel.UserNameOrEmailId))
                {
                    EmailModel emailmodel = new EmailModel();

                    emailmodel.To = objResetPasswordModel.UserNameOrEmailId;
                    emailmodel.Subject = "Reset Password";
                    string EncryptMail = Common.Encrypt(emailmodel.To);
                    EncryptMail = EncryptMail.Replace("+", "%20");
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<div style=\"padding:20px; font:normal 14px Arial, Helvetica, sans-serif; color:#333333;\">");
                    sb.Append("Click the link below for resetting your password.<br />");
                    // sb.Append("<a href=" + ConfigurationManager.AppSettings["funfoodingUrl"] + "/Account/Reset?emailId=" + emailmodel.To + " style=\"color:#0066CC\"> here</a>.<br />");
                    sb.Append("<a href=" + ConfigurationManager.AppSettings["funfoodingUrl"] + "/MobileApi/Reset/" + EncryptMail + " style=\"color:#0066CC\"> here</a>.<br />");
                    sb.Append("regards,<br /> Funfooding Team");
                    emailmodel.EmailBody = sb.ToString();
                    Common.sendeMail(emailmodel, true);

                     return Json(new { success = true, msg = "Email sent to your emailaddress. Please check your mail and reset your password." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //ModelState.AddModelError("UserNameOrEmailId", new Exception("This emailadress does not exist in our system. please try with valid emailaddress."));
                     return Json(new { success = false, msg = "This emailadress does not exist in our system. please try with valid emailaddress." }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, msg = "Please enter an email address." }, JsonRequestBehavior.AllowGet);
               
        }

           [HttpPost()]
         public JsonResult Reset(ResetModel resetModel)
         {
             if (resetModel.Password == null)
             {
                 return Json(new { success = false, msg = "password needed." }, JsonRequestBehavior.AllowGet);

             }
             if (resetModel.ConfirmPassword == null)
             {
                 return Json(new { success = false, msg = "confirm password." }, JsonRequestBehavior.AllowGet);
             }

                         
             if (resetModel.Password.Length < 6 || resetModel.Password.Length < 6)
                 return Json(new { success = false, msg = "password must be more than 6 characters." }, JsonRequestBehavior.AllowGet);
             if (resetModel.Password != resetModel.ConfirmPassword)
             {
                   return Json(new { success = false, msg = "Password and confirm password does not match." }, JsonRequestBehavior.AllowGet);
             }
             WebSecurity.ResetPassword(WebSecurity.GeneratePasswordResetToken(resetModel.UserName), resetModel.Password);
             EmailModel emailmodel = new EmailModel();
             emailmodel.To = resetModel.UserName;
             emailmodel.Subject = "Reset Password";
             var user = db.UserProfiles.Where(x => x.UserName.Equals(resetModel.UserName)).First();
             int UserID = user.UserId;
             Session["FirstName"] = user.FirstName;

             StringBuilder sb = new StringBuilder();
             sb.Append("<div style=\"padding:20px; font:normal 14px Arial, Helvetica, sans-serif; color:#333333;\">");
             sb.Append("Hi " + user.FirstName + ",<br />");
             sb.Append("Your password has been changed.<br />");
             sb.Append("regards,<br /> Funfooding Team");
             emailmodel.EmailBody = sb.ToString();
             Common.sendeMail(emailmodel, true);
             if (WebSecurity.Login(resetModel.UserName, resetModel.Password, persistCookie: true))
             {
               
                string  returnUrl = RedirectPage(UserID);
                 return Json(new { success = true, Url = returnUrl }, JsonRequestBehavior.AllowGet);
             }

             return Json(new { success = false,msg="Login Expired" }, JsonRequestBehavior.AllowGet);
         }
           [HttpGet]
            public JsonResult yournetwork(int UserID)
           {
               NetworkViewModel nvm = new NetworkViewModel();

               List<ContactsWaiting> contacts = (from p in dbmeals.ContactLists
                                                 where p.UserID == UserID && p.RequestAccepted == null
                                                 group p by new { p.RecipientEmailAddress, p.RequestAccepted, p.UserID, p.RecipientUserID } into g
                                                 select new ContactsWaiting { EmailAddress = g.Key.RecipientEmailAddress, Accepted = g.Key.RequestAccepted, SenderUserID = g.Key.UserID, RecipientUserID = g.Key.RecipientUserID, Sender = 1 }).ToList(); ;


               List<ContactsWaiting> contacts1 = (from p in dbmeals.ContactLists
                                                  where p.RecipientUserID == UserID && p.RequestAccepted == null
                                                  group p by new { p.SenderEmailAddress, p.RequestAccepted, p.UserID, p.RecipientUserID } into g
                                                  select new ContactsWaiting { EmailAddress = g.Key.SenderEmailAddress, Accepted = g.Key.RequestAccepted, SenderUserID = g.Key.UserID, RecipientUserID = g.Key.RecipientUserID, Sender = 0 }).ToList(); ;

               var cninner = (from sa in dbmeals.Connections
                              where sa.UserId == UserID && sa.DegreeOfSeparation == 1
                              select new { ContactID = sa.ContactID, Email = sa.ContactEmail, SharesFood = sa.SharesFood, BoughtFoodFromUser = sa.BoughtFoodFromUser, SoldFoodToUser = sa.SoldFoodToUser }).ToArray();


               var prf = (from s in db.UserProfiles
                          select new { Name = s.FirstName, UserId = s.UserId }).ToArray();


               var InnerCircle = (from s in prf
                                  join sa in cninner on s.UserId equals sa.ContactID
                                  select new InnerCircle { Name = s.Name, Email = sa.Email, SharesFood = sa.SharesFood, BoughtFoodFromUser = sa.BoughtFoodFromUser, SoldFoodToUser = sa.SoldFoodToUser }).ToArray();

               var cnouter = (from sa in dbmeals.Connections
                              where sa.UserId == UserID && sa.DegreeOfSeparation == 2
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

               return Json(nvm , JsonRequestBehavior.AllowGet);
           }


        [HttpGet]
         public JsonResult Reset(string id)
         {
             try
             {
                // ResetModel resetModel = new ResetModel();
                 id = id.Replace(" ", "+");
                 string DecryptMail = Common.Decrypt(id);
                 string UserName = DecryptMail;
                 return Json(new { success = true, UserName = UserName }, JsonRequestBehavior.AllowGet);
             }
             catch (Exception e)
             {
                 return Json(new { success = false, msg = e.Message.ToString() + id }, JsonRequestBehavior.AllowGet);
             
             }
            
         }
          [HttpGet]
        public JsonResult SearchMealItem(string mealItemName)
        {
            if (!string.IsNullOrEmpty(mealItemName))
            {
                IMealItemService _service = new MealItemService();
                IEnumerable<MealItem> mt = _service.GetAll().Where(x => x.MealItemName.Contains(mealItemName.Trim()));
                IEnumerable<MealItemViewModel> mtvm = Mapper.Map<IEnumerable<MealItem>, IEnumerable<MealItemViewModel>>(mt);
                return Json(mtvm, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, msg = "Meal item name cannnot be empty" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult MealItem(int Id)
        {
            IMealItemService _service = new MealItemService();
            MealItem mt = _service.GetById(Id);
            MealItemViewModel mtvm = Mapper.Map<MealItem, MealItemViewModel>(mt);
            //mtvm = PopulateDropDown(mtvm);

            return Json(mtvm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult accountsetting(int userid)
        {
            UserSetting usersetting = dbmeals.UserSettings.Find(userid);
            UserSettingsViewModel mtvm = Mapper.Map<UserSetting, UserSettingsViewModel>(usersetting);
            if (usersetting == null)
            {
                return Json(new { success=false}, JsonRequestBehavior.AllowGet);
            }
            return Json(mtvm, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult DeleteMealItem(int Id)
        {
            IMealItemService _service = new MealItemService();
            MealItem mt = _service.GetById(Id);
            try
            {
                bool msg = _service.Delete(mt);
                return Json(new { success = msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
            return Json(new { success = false, msg=e.Message.ToString()}, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost()]
        public JsonResult UpdateAccountSettings(UserSettingsViewModel usvm)
        {
            
            try
            {
                UserSetting usersetting = dbmeals.UserSettings.Find(usvm.UserSettingsID);
                usersetting.ReceiveEmailNotificationID = Convert.ToInt32(usvm.EmailNotificationFrequencyDD.SelectedFrequency);
                usersetting.ReceiveMobileTextNotificationID = Convert.ToInt32(usvm.TextNotificationFrequencyDD.SelectedFrequency);
                usersetting.PrivacySettingsID = Convert.ToInt32(usvm.PrivacySettingDD.SelectedPrivacySetting);

                UserSetting us = Mapper.Map<UserSettingsViewModel, UserSetting>(usvm);
                db.Entry(usersetting).State = EntityState.Modified;
                
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, msg = e.Message.ToString() }, JsonRequestBehavior.AllowGet);

            }
        }

          [HttpPost()]
        public JsonResult UpdateMealItem(MealItemViewModel mtvms)
        {
            IMealItemService _service = new MealItemService();
            MealItem mealitem = Mapper.Map<MealItemViewModel, MealItem>(mtvms);
            foreach (var mealaller in mtvms.AllergenDD)
            {
                if (mealaller.Selected)
                {
                    MealItems_AllergenicFoods aller = new MealItems_AllergenicFoods();
                    aller.AllergenicFoodID = mealaller.AllergenID;
                    mealitem.MealItems_AllergenicFoods.Add(aller);
                }
            }
            try
            {
                bool msg = _service.Update(mealitem);
                return Json(new { success = msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, msg = e.Message.ToString() }, JsonRequestBehavior.AllowGet);

            }
        }
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

          [HttpPost()]
          public JsonResult UpdateUser(UserDetail currentinfo,HttpPostedFileBase Photo,string phone)
          {
               
              try
              {
                  
           
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
                    if (!(IsImage(Photo)))
                   {
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
                    
                          return Json(new { success = false, msg = "Please select image file only." }, JsonRequestBehavior.AllowGet);
           
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
                
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
          
               }
              catch (Exception e)
              {
                  return Json(new { success = false, msg = e.Message.ToString() }, JsonRequestBehavior.AllowGet);

              }
          }
         

          [HttpGet]
        public JsonResult MyMealItem(int userID)
        {
            IMealItemService _service = new MealItemService();
            IEnumerable<MealItem> mt = _service.FindByUser(userID);
            IEnumerable<MealItemViewModel> mtvm = Mapper.Map<IEnumerable<MealItem>, IEnumerable<MealItemViewModel>>(mt);
            //mtvm = PopulateDropDown(mtvm);

            return Json(mtvm, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CreateMealItemDD(int userID)
        {

            MealItem mt = new MealItem();
            mt.UserId = userID;

            MealItemViewModel mtvm = Mapper.Map<MealItem, MealItemViewModel>(mt);

            mtvm = PopulateDropDown(mtvm, mt);

            return Json(mtvm, JsonRequestBehavior.AllowGet);
        }

        private MealItemViewModel PopulateDropDown(MealItemViewModel mtvm, MealItem mealitem)
        {
            IMealItemService _service = new MealItemService();
            if (mtvm == null)
                mtvm = new MealItemViewModel();
            mtvm.ServingUnitDD.SelectedId = mealitem.ServingUnit.ToString();
            mtvm.ServingUnitDD.Items = _service.ServingUnitDDList().ToList().Select(x => new SelectListItem
            {
                Value = x.ServingUnitID.ToString(),
                Text = x.ServingUnit,
                Selected = (mealitem != null && mealitem.ServingUnit == x.ServingUnitID)
            });
            mtvm.MealTypeDD.SelectedId = mealitem.MealTypeID.ToString();
            mtvm.MealTypeDD.Items = _service.MealTypeDDList().ToList().Select(x => new SelectListItem
            {
                Value = x.MealTypeID.ToString(),
                Text = x.Name,
                Selected = (mealitem != null && mealitem.MealTypeID == x.MealTypeID)
            });
            mtvm.CusineTypeDD.SelectedId = mealitem.CusineTypeID.ToString();
            
            mtvm.CusineTypeDD.Items = _service.CuisineTypeDDList().ToList().Select(x => new SelectListItem
            {
                Value = x.CuisineTypeID.ToString(),
                Text = x.Name,
                Selected = (mealitem != null && mealitem.CusineTypeID == x.CuisineTypeID)
            });
            mtvm.DietTypeDD.SelectedId = mealitem.DietTypeID.ToString();
            mtvm.DietTypeDD.Items = _service.DietTypeDDList().ToList().Select(x => new SelectListItem
            {
                Value = x.DietTypeID.ToString(),
                Text = x.Name,
                Selected = (mealitem != null && mealitem.DietTypeID == x.DietTypeID)
            });
            mtvm.AllergenDD = _service.AllergenicFoodsDDList().Select(x => new Allergen
            {
                AllergenName = x.AllergenicFood,
                AllergenID = x.AllergenicFoodID,
                Selected = (mealitem != null && mealitem.MealItems_AllergenicFoods.Where(y => y.AllergenicFoodID == x.AllergenicFoodID).Count() > 0)
            }).ToList();

            return mtvm;
        }
        public JsonResult CreateMealItem1()
        {
            MealItemViewModel MealAdvm = new MealItemViewModel();
            MealAdvm.AllergenDD = new List<Allergen>();
            MealAdvm.AllergenDD.Add(new Allergen());
            //MealAdvm.AvailabilityTypeDD = new AvailabilityTypeViewModel();
            return Json(MealAdvm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost()]
        public JsonResult CreateMealItem(MealItemViewModel mtvms)
        {
             
            IMealItemService _service = new MealItemService();
            MealItem mealitem = Mapper.Map<MealItemViewModel, MealItem>(mtvms);
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

            return Json(new { success = true, id = mealitem.MealItemId }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult MyMealAd(int userid)
        {
            ThreeSixtyTwoEntities db = new ThreeSixtyTwoEntities();
            IMealAdService _service = new MealAdService();
            IEnumerable<MealAd> mealad = _service.FindByUser(userid);
          //  ViewBag.AddressPresent = false;
          //  ViewBag.PrivacySettingConfirmed = false;
          //  ViewBag.SellerTermsandSettingAccepted = false;

            //if (mealad.Count() == 0)
            //{
            //    ViewBag.MealItemCount = db.MealItems.Where(x => x.UserId == userid).Count();
            //    ViewBag.AddressPresent = db.UserDetails.Any(x => x.UserId == userid && x.AddressID != null);
            //    ViewBag.PrivacySettingConfirmed = db.UserSettings.Where(x => x.UserID == userid).All(x => x.Verified == 1);
            //    var useragreed = db.UserAgreementsAcceptanceDetails.Where(x => x.UserID == userid).FirstOrDefault();
            //    if (useragreed != null)

            //        ViewBag.SellerTermsandSettingAccepted = true;
            //    if (!((ViewBag.MealItemCount != 0) && (ViewBag.AddressPresent) && (ViewBag.PrivacySettingConfirmed) && (ViewBag.SellerTermsandSettingAccepted)))
            //    {

            //        Session["ReadyToAdvertise"] = "0";
            //    }


            //}

            IEnumerable<MealAdViewModel> mealadvm = Mapper.Map<IEnumerable<MealAd>, IEnumerable<MealAdViewModel>>(mealad);

            foreach (var item in mealad)
            {
                if (item.MealItem != null)
                    mealadvm.Where(x => x.MealAdID == item.MealAdID).FirstOrDefault().MealItemName = item.MealItem.MealItemName;
            }

            return Json(mealadvm, JsonRequestBehavior.AllowGet);
        }

         [HttpGet]
        public JsonResult MealAd(int Id)
        {
            IMealAdService _service = new MealAdService();
            MealAd mt = _service.GetById(Id);
            MealAdViewModel mtvm = Mapper.Map<MealAd, MealAdViewModel>(mt);
            //mtvm = PopulateDropDown(mtvm);

            return Json(mtvm, JsonRequestBehavior.AllowGet);
        }

         [HttpGet]
         public JsonResult UserDetails(int userid)
         {
             UserDetail userdetail = dbmeals.UserDetails.Find(userid);
             if (userdetail == null)
             {

                 return Json(new { success = false, msg = "Invalid Userid" }, JsonRequestBehavior.AllowGet);
             }
             AddressList address = userdetail.AddressList;// dbmeals.AddressLists.Where(x => x.UserId == userid && x.IsCurrent == 1).FirstOrDefault();
             ViewBag.UserID = userid;

             UserDetailViewModel userinfovm = Mapper.Map<UserDetail, UserDetailViewModel>(userdetail);
             userinfovm.Address = Mapper.Map<AddressList, AddressViewModel>(address);
             if (userinfovm.Address != null && dbmeals.LKUPCountries.FirstOrDefault(x => x.CountryID == userinfovm.Address.CountryID) != null)
                 userinfovm.Address.CountryName = dbmeals.LKUPCountries.FirstOrDefault(x => x.CountryID == userinfovm.Address.CountryID).Country;



             return Json(userinfovm, JsonRequestBehavior.AllowGet);
         }

         

          [HttpGet]
        public JsonResult CreateMealAdDD(int userid)
        {
            MealAd mealad = new MealAd();
            MealAdViewModel mealadvm = Mapper.Map<MealAd, MealAdViewModel>(mealad);
            mealadvm = PopulateDropDown(mealadvm, mealad, userid);
            return Json(mealadvm, JsonRequestBehavior.AllowGet);

        }
        public JsonResult CreateMealAd1()
        {
            MealAdViewModel MealAdvm = new MealAdViewModel();
            MealAdvm.AvailabilityTypeDD = new AvailabilityTypeViewModel();
            return Json(MealAdvm, JsonRequestBehavior.AllowGet);
        }
        [HttpPost()]
        public JsonResult CreateMealAd(MealAdViewModel MealAdvm)
        {
            IMealAdService _service = new MealAdService();
            ThreeSixtyTwoEntities db = new ThreeSixtyTwoEntities();
            MealAd mealad = Mapper.Map<MealAdViewModel, MealAd>(MealAdvm);

            foreach (var payment in MealAdvm.PaymentMethods)
            {
                if (payment.Selected)
                {

                    MealAds_PaymentOptions paymentoptions = new MealAds_PaymentOptions();
                    paymentoptions.PaymentOptionID = payment.PaymentMethodID;
                    mealad.MealAds_PaymentOptions.Add(paymentoptions);
                }

            }

            foreach (var delivery in MealAdvm.DeliveryMethods)
            {
                if (delivery.Selected)
                {

                    MealAds_DeliveryMethods deliverymthd = new MealAds_DeliveryMethods();
                    deliverymthd.DeliveryMethodID = delivery.DeliveryMethodID;
                    mealad.MealAds_DeliveryMethods.Add(deliverymthd);
                }

            }
            int orderingoptionnum = 0;
            var availabilityType = db.AvailabilityTypes.Where(x => x.AvaiilabilityTypeID == mealad.AvailabilityTypeID).FirstOrDefault();
            if (availabilityType != null)
            {

                string orderingoption = availabilityType.AvailabilityType1;
                orderingoptionnum = Convert.ToInt32(orderingoption);
            }
            //foreach (var schedules in MealAdvm.MealAdSchedules)
            //{

            //    MealAd_Schedules meadadschedule = new MealAd_Schedules();
            //    meadadschedule.PickUpStartDateTime = schedules.PickUpStartDateTime;
            //    meadadschedule.PickUpEndDateTime = schedules.PickUpEndDateTime;
            //    meadadschedule.LastOrderDateTime = schedules.PickUpEndDateTime.AddHours(-orderingoptionnum);
            //    mealad.MealAd_Schedules.Add(meadadschedule);


            //}
            try
            {
                mealad.MealAdID = _service.AddAndReturnID(mealad);

                return Json(new { success = true, id = mealad.MealAdID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json(new { success = false, msg = e.Message.ToString() }, JsonRequestBehavior.AllowGet); 
            
            }
        }

        [HttpPost()]
        public JsonResult UpdateMealAd(MealAdViewModel MealAdvm)
        {
            IMealAdService _service = new MealAdService();
            ThreeSixtyTwoEntities db = new ThreeSixtyTwoEntities();
            MealAd mealad = Mapper.Map<MealAdViewModel, MealAd>(MealAdvm);

            foreach (var payment in MealAdvm.PaymentMethods)
            {
                if (payment.Selected)
                {

                    MealAds_PaymentOptions paymentoptions = new MealAds_PaymentOptions();
                    paymentoptions.PaymentOptionID = payment.PaymentMethodID;
                    mealad.MealAds_PaymentOptions.Add(paymentoptions);
                }

            }

            foreach (var delivery in MealAdvm.DeliveryMethods)
            {
                if (delivery.Selected)
                {

                    MealAds_DeliveryMethods deliverymthd = new MealAds_DeliveryMethods();
                    deliverymthd.DeliveryMethodID = delivery.DeliveryMethodID;
                    mealad.MealAds_DeliveryMethods.Add(deliverymthd);
                }

            }
            int orderingoptionnum = 0;
            var availabilityType = db.AvailabilityTypes.Where(x => x.AvaiilabilityTypeID == mealad.AvailabilityTypeID).FirstOrDefault();
            if (availabilityType != null)
            {

                string orderingoption = availabilityType.AvailabilityType1;
                orderingoptionnum = Convert.ToInt32(orderingoption);
            }
            //foreach (var schedules in MealAdvm.MealAdSchedules)
            //{

            //    MealAd_Schedules meadadschedule = new MealAd_Schedules();
            //    meadadschedule.PickUpStartDateTime = schedules.PickUpStartDateTime;
            //    meadadschedule.PickUpEndDateTime = schedules.PickUpEndDateTime;
            //    meadadschedule.LastOrderDateTime = schedules.PickUpEndDateTime.AddHours(-orderingoptionnum);
            //    mealad.MealAd_Schedules.Add(meadadschedule);


            //}
            try
            {
                bool msg = _service.Update(mealad);

                return Json(new { success = msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, msg = e.Message.ToString() }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpGet]
        public JsonResult DeleteMealAd(int Id)
        {
            IMealAdService _service = new MealAdService();
            MealAd mt = _service.GetById(Id);
            try
            {
                bool msg = _service.Delete(mt);
                return Json(new { success = msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, msg = e.Message.ToString() }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpGet]
        public JsonResult DeleteUser(int userid)
        {
           
            try
            {

                UserDetail userdetail = dbmeals.UserDetails.Find(userid);
                dbmeals.UserDetails.Remove(userdetail);
                dbmeals.SaveChanges();
                  return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, msg = e.Message.ToString() }, JsonRequestBehavior.AllowGet);

            }
        }


        private MealAdViewModel PopulateDropDown(MealAdViewModel mealadvm, MealAd mealad, int userid)
        {
            ThreeSixtyTwoEntities db = new ThreeSixtyTwoEntities();
            IMealAdService _service = new MealAdService();
            if (mealadvm == null)
                mealadvm = new MealAdViewModel();

            var MealItems = from b in db.MealItems
                            where b.UserId == userid
                            select b;

            IEnumerable<MealItem> MealItemsList = MealItems.ToList();
            mealadvm.MealItemsDD.MealItemsDDList =
                MealItemsList.Select(x => new SelectListItem
                {
                    Value = x.MealItemId.ToString(),
                    Text = x.MealItemName,
                    Selected = (mealad != null && mealad.MealItemID == x.MealItemId)
                });

            var AvailTypeMethod = from b in db.AvailabilityTypes
                                  select b;

            IEnumerable<AvailabilityType> AvailTypeList = AvailTypeMethod.ToList();
            mealadvm.AvailabilityTypeDD.AvailabilityTypeDDList =
                AvailTypeList.Select(x => new SelectListItem
                {
                    Value = x.AvaiilabilityTypeID.ToString(),
                    Text = x.Descriptions,
                    Selected = (mealad != null && mealad.AvailabilityTypeID == x.AvaiilabilityTypeID)

                });

            mealadvm.DeliveryMethods = _service.GetDeliveryMethodDDList().Select(x => new DeliveryMethodViewModel
            {
                DeliveryMethod = x.DeliveryMethod,
                DeliveryMethodID = x.DeliveryMethodID,
                Selected = (mealad != null && mealad.MealAds_DeliveryMethods.Where(y => y.DeliveryMethodID == x.DeliveryMethodID).Count() > 0)
            }).ToList();


            mealadvm.PaymentMethods = _service.PaymentMethodDDList().Select(x => new PaymentMethodViewModel
            {
                PaymentMethodID = x.PaymentOptionID,
                PaymentMethod = x.PaymentOption1,
                Selected = (mealad != null && mealad.MealAds_PaymentOptions.Where(y => y.PaymentOptionID == x.PaymentOptionID).Count() > 0)
            }).ToList();

            MealAdSchedule mealads = new MealAdSchedule();
            mealadvm.MealAdSchedules = new List<MealAdSchedule>();
            if (mealad != null && mealad.MealAd_Schedules != null && mealad.MealAd_Schedules.Count > 0)
            {
                foreach (var data in mealad.MealAd_Schedules)
                {
                    mealadvm.MealAdSchedules.Add(new MealAdSchedule() { LastOrderDateTime = data.LastOrderDateTime, PickUpEndDateTime = data.PickUpEndDateTime, PickUpStartDateTime = data.PickUpStartDateTime });
                }
            }
            else
            {
                mealadvm.MealAdSchedules.Add(mealads);
            }

            return mealadvm;
        }
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        [HttpPost()]
        public JsonResult AddtoCart(string mealItemId, int qty, string itemName, decimal price,string sessionId,int userId)
        {

            TempOrderList tempOrder = new TempOrderList();
            tempOrder.MealItemId = mealItemId;
            tempOrder.qty = qty;
            tempOrder.userid = userId;
            tempOrder.sessionId = sessionId;
            tempOrder.itemName = itemName;
            tempOrder.lineitemcost = price;
            tempOrder.TotalCost = qty * price;
            //dbmeals.Entry(dbmeals.TempOrderLists).State = EntityState.Added;
            dbmeals.TempOrderLists.Add(tempOrder);
            int id = Convert.ToInt32(mealItemId);
            var mealItem = dbmeals.MealItems.Where(x => x.MealItemId == id).FirstOrDefault();
            if (mealItem != null)
            {
                mealItem.Quantity = mealItem.Quantity - qty;
            }
            dbmeals.SaveChanges();
            int count = dbmeals.TempOrderLists.Where(x => x.sessionId == sessionId && x.userid == userId).Count();
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CartList(string sessionId,int userId)
        {
            dbmeals = new ThreeSixtyTwoEntities();
            List<TempOrderList> lstTempOrderList = dbmeals.TempOrderLists.Where(x => x.sessionId == sessionId && x.userid == userId).ToList();
            return Json(lstTempOrderList, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public JsonResult Invite(EmailaddressesViewModel invitedemails)
        {
            int userid = invitedemails.UserID;

        
                string invalidemails = "";
                string repeatrequests = "";


                char[] delimiters = (",: ").ToCharArray();

                List<string> emails = invitedemails.Emailaddresses.Split(delimiters).ToList();
              //  IUserMailer mailer = new UserMailer();


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
                             return Json(new { success = false, msg = "Please enter valid email address" }, JsonRequestBehavior.AllowGet);

                        }


                        if (IsValidEmail(emailaddress))
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
                            InsertRequestInfo(userid, emailmodel.From, emailmodel.To, null);
                        }
                        else
                        {
                            invalidemails = invalidemails + "," + emailaddress;


                        }

                    }

                }

                if (invalidemails == "" && repeatrequests == "")
                {

                    return Json(new { success = true, msg = "Invitation Sent." }, JsonRequestBehavior.AllowGet);

                }
                if (repeatrequests != "")

                    invitedemails.ErrorMessage = "Sorry, you cannot send another request.you have already requested these email address twice. " + repeatrequests;
                return Json(new { success = true, msg = invitedemails.ErrorMessage }, JsonRequestBehavior.AllowGet);


                if (invalidemails != "")
                {
                    return Json(new { success = false, msg = "<br><br>Please correct these emailaddresses" + invalidemails }, JsonRequestBehavior.AllowGet);

                              
                }
                           

               
                      

        }

        private bool IsValidEmail(string email)
        {
            var attribute = new EmailAttribute();


            return attribute.IsValid(email);


        }
         [HttpPost]
        public JsonResult AcceptRequest(int RecipientUserID, int SenderUserID)
        {
            EmailModel em = new EmailModel();
            string err = "";
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
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
           
            }
            catch (Exception e)
            {
                err = e.Message.ToString();
                em.To = "kanjasaha@gmail.com";
                em.From = "kanjasaha@gmail.com";
                em.EmailBody = err;
                Common.sendeMail(em, true);
                return Json(new { success = false, msg = "There is already an account with that email. Please use a different email" }, JsonRequestBehavior.AllowGet);
            

            }

         

        }
         private int HasBoughtFoodFromUser(int UserId, int ContactID)
         {
             bool BoughtFood = dbmeals.OrderDetails.Any(x => x.OrderID == (dbmeals.Orders.Where(n => n.UserId == ContactID)).FirstOrDefault().OrderID && x.MealAdID == (dbmeals.MealAds.Where(n => n.UserId == UserId)).FirstOrDefault().MealAdID);//add a column userid in mealadid
             if (BoughtFood)
                 return 1;
             else return 0;
         }

         private int HasSoldFoodToUser(int UserId, int ContactID)
         {
             int SoldFood = (from a in dbmeals.OrderDetails
                             join b in dbmeals.Orders
                                   on a.OrderID equals b.OrderID
                             join c in dbmeals.MealAds
                             on a.MealAdID equals c.MealAdID
                             where b.UserId == ContactID
                             && b.UserId == UserId
                             select a.MealAdID).Count();


             if (SoldFood == 0)
                 return 0;
             else return 1;
         }

         private int SharesFood(int ContactID)
         {
             int SharedFood = (from a in dbmeals.MealAds
                               join b in dbmeals.MealItems
                                   on a.MealItemID equals b.MealItemId
                               where b.UserId == ContactID
                               select a.MealAdID).Count();


             if (SharedFood == 0)
                 return 0;
             else return 1;
         }
       

        [HttpPost]
        public JsonResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                        return Json(new { success = true, msg = "Password Changed." }, JsonRequestBehavior.AllowGet);

                    }
                    catch (Exception)
                    {
                        return Json(new { success = false, msg = "please try again." }, JsonRequestBehavior.AllowGet);

                    }
                }

                else
                { return Json(new { success = false, msg = "modelstate invalid." }, JsonRequestBehavior.AllowGet); }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return Json(new { success = true, msg = "Password Set." }, JsonRequestBehavior.AllowGet);
             
                    }
                    catch (Exception e)
                    {
                        return Json(new { success = false, msg = "please try again." }, JsonRequestBehavior.AllowGet);
             
                    }
                }

                else
                { return Json(new { success = false, msg = "modelstate invalid." }, JsonRequestBehavior.AllowGet); }
            }

           
        }


    }
}