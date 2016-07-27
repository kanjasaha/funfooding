using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Routing;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using MealsToGo.Filters;
using MealsToGo.Models;
using Postal;
using System.Configuration;
using MealsToGo.ViewModels;
using AutoMapper;
using System.Text;
using System.Data.SqlClient;
using MealsToGo.Helpers;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Data;


namespace MealsToGo.Controllers
{
    [Authorize]
    // [InitializeSimpleMembership]
    //    [SessionState(System.Web.SessionState.SessionStateBehavior.Default)]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login

        private UsersContext db = new UsersContext();
        private ThreeSixtyTwoEntities dbmeals = new ThreeSixtyTwoEntities();
        LoginRegisterViewModel log = new LoginRegisterViewModel();

        LoginModel model = new LoginModel();
       

        [AllowAnonymous]
        public ActionResult Reset(string emailId)
        {
            ResetModel resetModel = new ResetModel();
            emailId = emailId.Replace(" ", "+");
            string DecryptMail = Common.Decrypt(emailId);
            //resetModel.UserName = emailId;
            resetModel.UserName = DecryptMail;
            return View(resetModel);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Reset(ResetModel resetModel)
        {
           if (resetModel.Password == null)
            {
                return View(resetModel);

            }
            if (resetModel.ConfirmPassword == null)
            {
                //  TempData["BlankConfirmPassword"] = "Please enter Confirm password";
                return View(resetModel);
            }
            if (resetModel.Password.Length < 6 || resetModel.Password.Length < 6)
                return View(resetModel);
            if (resetModel.Password != resetModel.ConfirmPassword)
            {
                // TempData["BlankPassword"] = "Password and confirm password does not match";
                return View(resetModel);
            }
            WebSecurity.ResetPassword(WebSecurity.GeneratePasswordResetToken(resetModel.UserName), resetModel.Password);
            EmailModel emailmodel = new EmailModel();
            emailmodel.To = resetModel.UserName;
            emailmodel.Subject = "Reset Password";

            StringBuilder sb = new StringBuilder();
            sb.Append("<div style=\"padding:20px; font:normal 14px Arial, Helvetica, sans-serif; color:#333333;\">");
            sb.Append("Hi User,<br />");
            sb.Append("Your password has been changed.<br />");
            sb.Append("regards,<br /> Funfooding Team");
            emailmodel.EmailBody = sb.ToString();
            Common.sendeMail(emailmodel, true);
            //return RedirectToAction("Login", "Account");
            if (WebSecurity.Login(resetModel.UserName, resetModel.Password, persistCookie: true))
            {

                var user = db.UserProfiles.Where(x => x.UserName.Equals(resetModel.UserName)).First();
                int UserID = user.UserId;
                Session["FirstName"] = user.FirstName;

                return RedirectPage(UserID);
            }
            //}
            return View(resetModel);
        }
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel objResetPasswordModel)
        {

            if (ModelState.IsValid)
            {

                if (EmailExist(objResetPasswordModel.UserNameOrEmailId))
                {
                    EmailModel emailmodel = new EmailModel();

                    emailmodel.To = objResetPasswordModel.UserNameOrEmailId;
                    emailmodel.Subject = "Reset Password";
                    string EncryptMail = Common.Encrypt(emailmodel.To);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<div style=\"padding:20px; font:normal 14px Arial, Helvetica, sans-serif; color:#333333;\">");
                    sb.Append("Click the link below for resetting your password.<br />");
                    // sb.Append("<a href=" + ConfigurationManager.AppSettings["funfoodingUrl"] + "/Account/Reset?emailId=" + emailmodel.To + " style=\"color:#0066CC\"> here</a>.<br />");
                    sb.Append("<a href=" + ConfigurationManager.AppSettings["funfoodingUrl"] + "/Account/Reset?emailId=" + EncryptMail + " style=\"color:#0066CC\"> here</a>.<br />");
                    sb.Append("regards,<br /> Funfooding Team");
                    emailmodel.EmailBody = sb.ToString();
                    Common.sendeMail(emailmodel, true);

                    ViewBag.Message = "Email sent to your emailaddress. Please check your mail and reset your password.";
                }
                else
                {
                    //ModelState.AddModelError("UserNameOrEmailId", new Exception("This emailadress does not exist in our system. please try with valid emailaddress."));
                    ViewBag.Message = "This emailadress does not exist in our system. please try with valid emailaddress.";
                }
            }
            return View(objResetPasswordModel);
        }
       
     

        private bool EmailExist(string email)
        {


            return db.UserProfiles.Any(x => x.UserName == email);


        }


        [AllowAnonymous]
        public ActionResult ProcessRequest(int RequestID, int RequestAccepted)
        {

            //update COntactList table accordingly with the value of Requestid

            //check to see if RequestedTo is an existing member

            int result = 1;
            if (result == 1)
            {
                //return RedirectToAction("Login");
                return RedirectToAction("LocationToSearch", "Home");

            }
            else
            {
                return RedirectToAction("Register");
            }

        }
        [AllowAnonymous]
        public ActionResult Authenticate(string returnUrl)
        {
            // GET: /Account/Login
            if (Request.IsAuthenticated)
            {
                int UserID = WebSecurity.CurrentUserId;

                if (UserID == -1)
                {
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Authenticate", "Account", null);

                }
                Session["FirstName"] = db.UserProfiles.Where(x => x.UserId.Equals(UserID)).First().FirstName;
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else

                    return RedirectPage(UserID);

            }
            else

                return RedirectToAction("LocationToSearch", "Home", null);



        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            model = Mapper.Map<LoginRegisterViewModel, LoginModel>(log);

            if (Request.Cookies["Username"] != null && Request.Cookies["Password"] != null)
            {
                HttpCookie cookieUser = Request.Cookies["Username"];
                HttpCookie cookiePassword = Request.Cookies["Password"];
                model.UserName = cookieUser.Value;
                model.Password = cookiePassword.Value;
                model.RememberMe = true;
                log.UserName = cookieUser.Value;
                log.Password = cookiePassword.Value;
                if (log.UserName != "" && log.Password != "")
                {
                    log.RememberMe = true;
                }
            }
         
            return View(log);

        }

        [AllowAnonymous]
        public ActionResult SignUp(string id)
        {
            
            LoginRegisterViewModel lrvm = new LoginRegisterViewModel();
          

            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View();

        }

        //[Authorize]
        //public ActionResult Login(string returnUrl)
        //{



        //    LoginRegisterViewModel log = new LoginRegisterViewModel();

        //    LoginModel model = new LoginModel();
        //    model = Mapper.Map<LoginRegisterViewModel, LoginModel>(log);

        //    if (Request.Cookies["Username"] != null && Request.Cookies["Password"] != null)
        //    {
        //        HttpCookie cookieUser = Request.Cookies["Username"];
        //        HttpCookie cookiePassword = Request.Cookies["Password"];
        //        model.UserName = cookieUser.Value;
        //        model.Password = cookiePassword.Value;
        //    }

        //    return View(log);

        //}


        private GLatLong GetUserLocation(int UserId)
        {
            var userdetails = dbmeals.UserDetails.FirstOrDefault(u => u.UserId == UserId);
            GLatLong userloc = new GLatLong();
            if (userdetails != null)
            {
                userloc.Latitude = Convert.ToDouble(userdetails.AddressList.Latitude);
                userloc.Longitude = Convert.ToDouble(userdetails.AddressList.Longitude);

                return userloc;
            }

            else
                return null;



        }


        private void MigrateShoppingCart(string UserName, int userid)
        {
            // Associate shopping cart items with logged-in user
            var cart = ShoppingCart.GetCart(userid);

            cart.MigrateCart(UserName);
            Session[ShoppingCart.CartSessionKey] = UserName;
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginRegisterViewModel viewmodel, string returnUrl)
        {

            //LoginModel model = new LoginModel();
            model = Mapper.Map<LoginRegisterViewModel, LoginModel>(viewmodel);
            if (model.UserName == "" && model.Password == "")
            {
                TempData["alert1"] = "Please enter username and password";
                return View("Login");

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
                    //if (model.RememberMe)
                    //{

                    //    return View(viewmodel);
                    //}
                    //else
                    //{
                    //    viewmodel.UserName = null;
                    //    viewmodel.Password = null;
                    //    viewmodel.RememberMe = false;
                    //    return View(viewmodel);
                    //}



                    if (Url.IsLocalUrl(returnUrl))
                    {
                        bool UserExists12 = db.UserProfiles.Any(x => x.UserName.Equals(model.UserName));
                        if (UserExists12)
                        {
                            // If we got this far, something failed, redisplay form
                            // ModelState.AddModelError("", "The user name or password provided is incorrect.");
                            //TempData["alert1"] = "The user name or password provided is incorrect.";
                            //return View("Login");
                        }
                        else
                        {
                            TempData["alert1"] = "This username does not have an account with us at Funfooding. Please try again with correct username or signup.";
                            return View("Login");
                            //ModelState.AddModelError("", "This email Address is not registered with the site. Please complete your registration.");
                        }

                        return Redirect(returnUrl);
                    }
                    else
                    {
                        bool UserExists2 = db.UserProfiles.Any(x => x.UserName.Equals(model.UserName));
                        if (UserExists2)
                        {

                            // If we got this far, something failed, redisplay form
                            // ModelState.AddModelError("", "The user name or password provided is incorrect.");
                            //  TempData["alert1"] = "The user name or password provided is incorrect.";
                            //return View("Login");
                        }
                        else
                        {
                            //  ModelState.AddModelError("", "This email Address is not registered with the site. Please complete your registration.");
                            TempData["alert1"] = "This username does not have an account with us at Funfooding. Please try again with correct username or signup.";
                            return View("Login");
                        }
                        return RedirectPage(UserID);
                    }



                }
            }
            catch (Exception e1)
            {
                TempData["alert1"] = "Please enter username and password";
                return View("Login");

            }
            bool UserExists = db.UserProfiles.Any(x => x.UserName.Equals(model.UserName));
            if (UserExists)
            {

               //  If we got this far, something failed, redisplay form
                  ModelState.AddModelError("", "The user name or password provided is incorrect.");
                TempData["alert1"] = "The user name or password provided is not correct.";
                return View("Login");
            }
            else
            {
                // ModelState.AddModelError("", "This email Address is not registered with the site. Please complete your registration.");
                TempData["alert1"] = "This username does not have an account with us at Funfooding. Please try again with correct username or signup.";
                return View("Login");
            }
            if (model.RememberMe)
            {

                return View("SignUp", viewmodel);
            }
            else
            {
                viewmodel.UserName = null;
                viewmodel.Password = null;
                viewmodel.RememberMe = false;
                return View("SignUp", viewmodel);
            }

            //return View(viewmodel);

        }

        public RedirectToRouteResult RedirectPage(int userid)
        {
            
            bool userinvited = dbmeals.ContactLists.Any(x =>x.RecipientUserID==userid);
            if (userinvited)
            {
                return RedirectToAction("Index", "Contact", new { userid = userid });
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
                         long ipno = Common.IP2Decimal(arripaddress[i]);
                        if ((ipno > 0) || (ipno < 33554431))
                        {
                            SqlDataReader reader;
                               SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                             SqlCommand sqlCmd = new SqlCommand("SELECT TOP 1 * FROM ip2location_db3_ipv6 WHERE " + ipno.ToString() + " <= ip_to ORDER BY ip_to", sqlConn);
                            sqlCmd.Connection.Open();
                              reader = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
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
                    return RedirectToAction("LocationToSearch", "Home", null);
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
                    return RedirectToAction("LocationToSearch", "Home", null);

            }
            SearchParam searchparam = new SearchParam();
            return RedirectToAction("Index", "Home", new RouteValueDictionary(searchparam));

        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {

            //LoginRegisterViewModel model = new LoginRegisterViewModel();
            HttpCookie cookieUser = Request.Cookies["Username"];
            HttpCookie cookiePassword = Request.Cookies["Password"];
            if (cookieUser != null || cookiePassword != null)
            {

                model.UserName = cookieUser.Value;
                model.Password = cookiePassword.Value;
                model.RememberMe = true;
                log.UserName = cookieUser.Value;
                log.Password = cookiePassword.Value;
                if (log.UserName != "" && log.Password != "")
                {
                    log.RememberMe = true;
                }
            }

            Session["FirstName"] = null;
            WebSecurity.Logout();
            // return View(log);
            return RedirectToAction("Login", "Account");
            //   return RedirectToAction("LocationToSearch", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(LoginRegisterViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                LoginModel model = new LoginModel();
                model = Mapper.Map<LoginRegisterViewModel, LoginModel>(viewmodel);

                bool userexists = db.UserProfiles.Any(x => x.UserName.Equals(model.UserName));

                if (userexists)
                {
                    // ModelState.AddModelError("", "There is already an account with that UserName. Please use a different username");
                    TempData["alertemailexist"] = "There is already an account with that email. Please use a different email";
                    return View("SignUp");
                    // return View(viewmodel);
                }

                if ((model.FirstName == "") || (model.FirstName == null))
                {
                    TempData["alert1"] = "First Name is required";
                    return View("SignUp");

                }


                    try
                    {
                        string confirmationToken =
                            WebSecurity.CreateUserAndAccount(model.UserName, model.Password, propertyValues: new
                            {
                                FirstName = model.FirstName
                            }, requireConfirmationToken: true); //new {Email=model.Email}


                       bool userinvited = dbmeals.ContactLists.Any(x =>x.RecipientEmailAddress.Equals(model.UserName));
                         if (!userinvited)
                        {

                        EmailModel emailmodel = new EmailModel();
                        emailmodel.To = model.UserName;
                        emailmodel.Subject = "Welcome to Fun Fooding";


                        StringBuilder sb = new StringBuilder();
                        sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
                        sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
                        sb.Append("<head>");
                        sb.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                        sb.Append("</head>");
                        sb.Append("<body>");
                        sb.Append("<div style=\"padding:20px; font:normal 14px Arial, Helvetica, sans-serif; color:#333333;\">");
                        sb.Append("Hi " + model.FirstName + ",<br />");
                        sb.Append("Thank you for signing up!<br />");
                        sb.Append("<br />");
                        sb.Append("To complete the sign up process click ");
                        sb.Append("<a href=" + ConfigurationManager.AppSettings["funfoodingUrl"] + "/Account/EmailConfirmation/" + confirmationToken + " style=\"color:#0066CC\"> here</a>.<br />");
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
                        return RedirectToAction("ConfirmEmail", "Account", new { Name = model.FirstName });
                    }
                         else

                         {
                           return RedirectToAction("EmailConfirmation", "Account", new { id = confirmationToken });
                         }
                    }
                    catch (MembershipCreateUserException e)
                    {
                        // ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));

                    }
                
            }

            // If we got this far, something failed, redisplay form
            return View("Login", viewmodel);
        }

        [AllowAnonymous]
        public ActionResult ConfirmEmail(string name)
        {
            ViewBag.Name = name;
            return View();
        }


        [AllowAnonymous]
        public ActionResult EmailConfirmation(string id)
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

                return RedirectPage(us.UserID);
            }
            return RedirectToAction("ConfirmationFailure");
        }

        [AllowAnonymous]
        public ActionResult ConfirmationSuccess()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ConfirmationFailure()
        {
            return View();
        }


        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
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
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
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
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        // ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin



        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
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
        #endregion
    }
}
