using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MealsToGo.ViewModels
{
    public class LoginRegisterViewModel
    {

        [MaxLength(50)]

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        [Display(Name = "Email")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MaxLength(50)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }


    }
}