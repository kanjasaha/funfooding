using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MealsToGo.Models;
using System.ComponentModel.DataAnnotations;

namespace MealsToGo.ViewModels
{
    public class EmailaddressesViewModel
    {
      
        public int UserID { get; set; }

        [Display(Name = "Email")]
        [Required]
        [EmailAddress]

        public string Emailaddresses { get; set; }


        [Required]
        [Display(Name = "Message")]
        public string EmailMessage { get; set; }



        public string ErrorMessage { get; set; }

    }
}