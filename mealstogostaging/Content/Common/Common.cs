using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using GeoCoding;
using MealsToGo.Models;
using MealsToGo.Helpers;
using System.Net.Mail;
using System.Transactions;
using System.IO;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;

namespace MealsToGo
{
    public static class Common
    {
        private static readonly string smtpserverusername = ConfigurationManager.AppSettings["smtpserverusername"];
        private static readonly string smtpserverpassword = ConfigurationManager.AppSettings["smtpserverpassword"];


        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        public static GLatLong GetLatLng(string address)
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
        // Convert dotted IP address into IP number in long
        public static long IP2Decimal(String DottedIP)
        {
            string[] arrConvert;
            int i;
            long intResult = 0;
            try
            {
                arrConvert = DottedIP.Split('.');
                for (i = arrConvert.Length - 1; i >= 0; i--)
                {
                    intResult = intResult + ((long.Parse(arrConvert[i]) % 256) * long.Parse(Math.Pow(256, 3 - i).ToString()));
                }
                return intResult;
            }
            catch (Exception ex)
            {
                //  Response.Write(ex.Message);
                return 0;
            }
        }

        public static bool sendeMail(EmailModel emailmodel, bool EmailExist)
        {
            try
            {
                MailMessage mail = new MailMessage();
                string receiver = emailmodel.To;
                string sender = smtpserverusername;
                //string verifyUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Home/ValidateRequest/?id=" + 4;
                //string Body = verifyUrl;
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(sender);
                mail.To.Add(receiver);
                mail.Bcc.Add(sender);
                mail.Subject = emailmodel.Subject;
                mail.Body = emailmodel.EmailBody;
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(smtpserverusername, smtpserverpassword);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool sendMail(EmailModel emailmodel, bool EmailExist)
        {
            try
            {
                MailMessage mail = new MailMessage();
                string receiver = emailmodel.To;
                string sender = emailmodel.From;
                string verifyUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Home/ValidateRequest/?id=" + 4;
                string Body = verifyUrl;
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(sender);
                mail.To.Add(receiver);
                mail.Bcc.Add(sender);
                mail.Subject = "Request";
                mail.Body = Body;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("kanjasaha@gmail.com", "Debabrata71");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

       
        public static DateTime AbsoluteStart(this DateTime dateTime)
        {
            
            return dateTime.Date;
        }

        /// <summary>
        /// Gets the 11:59:59 instance of a DateTime
        /// </summary>
        public static DateTime AbsoluteEnd(this DateTime dateTime)
        {
            return AbsoluteStart(dateTime).AddDays(1).AddTicks(-1);
        }

        public static string GetFullAddress(MealsToGo.Models.AddressList address)
        {
            List<string> fulladdress = new List<string>();

            if ((address.Address1 != null) && (address.Address1 != ""))
                fulladdress.Add(address.Address1);

            if ((address.Address2 != null) && (address.Address2 != ""))
                fulladdress.Add(address.Address1);

            if ((address.City != null) && (address.City != ""))
                fulladdress.Add(address.City);

            if ((address.Province != null) && (address.Province != ""))
                fulladdress.Add(address.Province);

            if ((address.Zip != null) && (address.Zip != ""))
                fulladdress.Add(address.Zip);

            if ((address.LKUPCountry.Country != null) && (address.LKUPCountry.Country != ""))
                fulladdress.Add(address.LKUPCountry.Country);

            return string.Join(",", fulladdress.ToArray());



        }
        
        public static IEnumerable<IEnumerable<T>> Partition<T>
   (this IEnumerable<T> source, int size)
        {
            T[] array = null;
            int count = 0;
            foreach (T item in source)
            {
                if (array == null)
                {
                    array = new T[size];
                }
                array[count] = item;
                count++;
                if (count == size)
                {
                    yield return new ReadOnlyCollection<T>(array);
                    array = null;
                    count = 0;
                }
            }
            if (array != null)
            {
                Array.Resize(ref array, count);
                yield return new ReadOnlyCollection<T>(array);
            }
        }


      

    }


}