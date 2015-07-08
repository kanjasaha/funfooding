using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolrNet;
using MealsToGo.Models;
using System.Net;
using Microsoft.Practices.ServiceLocation;
using SolrNet.Commands.Parameters;
using SolrNet.Exceptions;
using SolrNet.Impl;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;
using System.Net.Http;
using System.Data.SqlClient;
using MealsToGo.Repository;
using SolrNet.DSL;

using System.Net.Http.Headers;
using MealsToGo;

using MealsToGo.Service;
using System.Net.Mail;




namespace ConsoleApp
{


    class Program
    {
        private ThreeSixtyTwoEntities context = new ThreeSixtyTwoEntities();



        static void Main()
        {
            Program p = new Program();
            p.getrecord();


            #region OPTION 1


            //   ISolrOperations<ActiveMealAd> solrwrite =




            //  IEnumerable<ActiveMealAd> docs = unitOfWork.ActiveMealAdRepository.GetAll();



            //   Parallel.ForEach(Common.Partition(docs, 1000), group => solrwrite.AddRange(group));



            #endregion





            //#region OPTION 2

            using (var client = new WebClient())
            {

                string ret = client.DownloadString("http://localhost:8983/solr/collection1/dataimport?command=full-import&clean=true&commit=true?command=status&indent=true&wt=json&_=1384132704933");

            }

            //#endregion
        }

        public void getrecord()
        {


            List<SendEmail> lstemailRecords = new List<SendEmail>();
           
            lstemailRecords = getEmailRecords();
            List<SendText> lstTextRecords = new List<SendText>();

            lstTextRecords = getTextRecords();


            try
            {

                SendEmail(lstemailRecords);
                SendText(lstTextRecords);
            }
            catch (Exception ex)
            {

            }

        }
        public List<SendText> getTextRecords()
        {

            List<SendText> lstUserRecords = new List<SendText>();
            //  EmailEntities dbcontext = new EmailEntities();


            try
            {
                lstUserRecords = (from a in context.SendTexts
                                  where a.Status != "Success"
                                  && a.DeliveryTime <= DateTime.Now
                                  select new SendText
                                  {
                                      SendTextID = a.SendTextID,
                                      SenderPhone = a.SenderPhone,
                                      RecipientPhone = a.RecipientPhone,
                                      Message = a.Message,
                                     
                                  }).ToList();

                return lstUserRecords;
            }
            catch (Exception ex)
            {

                return lstUserRecords;
            }
        }


        public List<SendEmail> getEmailRecords()
        {

            List<SendEmail> lstUserRecords = new List<SendEmail>();
            //  EmailEntities dbcontext = new EmailEntities();


            try
            {
                lstUserRecords = (from a in context.SendEmails
                                  where a.Status != "Success"
                                  && a.DeliveryTime <= DateTime.Now
                                  select new SendEmail
                                  {
                                      SendEmailID = a.SendEmailID,
                                      SenderEmailAddress = a.SenderEmailAddress,
                                      RecipientEmailAddress = a.RecipientEmailAddress,
                                      Body = a.Body,
                                      Subject = a.Subject,


                                  }).ToList();

                return lstUserRecords;
            }
            catch (Exception ex)
            {

                return lstUserRecords;
            }
        }

        public void UpdateTablePass(int SendEmailID)
        {
            Console.WriteLine("Writing Database Log for Success...!");
            var objcls = context.SendEmails.Where(id => id.SendEmailID == SendEmailID).First();
            objcls.Status = "Success";
            context.SaveChanges();

        }

        public void UpdateTableFail(int SendEmailID)
        {
            Console.WriteLine("Writing Database Log for Failure...!");
            var objcls = context.SendEmails.Where(id => id.SendEmailID == SendEmailID).First();
            objcls.Status = "Failed";
            context.SaveChanges();
        }

        public void SendText(List<SendText> objects)
        {
            Console.WriteLine("Sending Text...!");
            //try
            //{
            //    Parallel.ForEach(
            //       objects, blog =>
            //       {
            //           var client = new MailMessage();
            //           SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            //           client.From = new MailAddress(blog.SenderEmailAddress);
            //           client.Bcc.Add(blog.RecipientEmailAddress);
            //           client.Subject = blog.Subject;
            //           client.Body = blog.Body;
            //           SmtpServer.Port = 587;
            //           // Request both failure and success report
            //           client.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.OnSuccess;
            //           SmtpServer.Credentials = new System.Net.NetworkCredential("kanjasaha@gmail.com", "Debabrata71");
            //           SmtpServer.EnableSsl = true;
            //           SmtpServer.Send(client);
            //           UpdateTablePass(Convert.ToInt32(blog.SendEmailID));




            //       });
            //}
            //catch (Exception ex)
            //{
            //    //UpdateTableFail(Convert.ToInt32(blog.SendEmailID));
            //}
        }

        public void SendEmail(List<SendEmail> objects)
        {
            Console.WriteLine("Sending Emails...!");
            try
            {
                Parallel.ForEach(
                   objects, blog =>
                   {
                       var client = new MailMessage();
                       SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                       client.From = new MailAddress(blog.SenderEmailAddress);
                       client.Bcc.Add(blog.RecipientEmailAddress);
                       client.Subject = blog.Subject;
                       client.Body = blog.Body;
                       SmtpServer.Port = 587;
                       // Request both failure and success report
                       client.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.OnSuccess;
                       SmtpServer.Credentials = new System.Net.NetworkCredential("kanjasaha@gmail.com", "Debabrata71");
                       SmtpServer.EnableSsl = true;
                       SmtpServer.Send(client);
                       UpdateTablePass(Convert.ToInt32(blog.SendEmailID));




                   });
            }
            catch (Exception ex)
            {
                //UpdateTableFail(Convert.ToInt32(blog.SendEmailID));
            }
        }


    }


}


