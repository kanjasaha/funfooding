using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MealsToGo.ViewModels
{
    public class UserSettingsViewModel
    {

        public int UserID { get; set; }
        public int UserSettingsID { get; set; }
       
       
        public int PrivacySettingID { get; set; }
        public string PrivacySetting { get; set; }
       
        public int ReceiveEmailNotificationID { get; set; }
        
        public int ReceiveMobileTextNotificationID { get; set; }
                
        public string ReceiveEmailNotification { get; set; }
        public string ReceiveMobileTextNotification { get; set; }

        public NotificationFrequencyViewModel EmailNotificationFrequencyDD { get; set; }
        public NotificationFrequencyViewModel TextNotificationFrequencyDD { get; set; }
        public PrivacySettingViewModel PrivacySettingDD { get; set; }

        public UserSettingsViewModel()
        {
            EmailNotificationFrequencyDD = new NotificationFrequencyViewModel(); 
            TextNotificationFrequencyDD = new NotificationFrequencyViewModel();
            PrivacySettingDD = new PrivacySettingViewModel();
           
        }

        public class NotificationFrequencyViewModel
        {
            public string SelectedFrequency { get; set; }
            public IEnumerable<SelectListItem> FrequencyDDList { get; set; }
        }

        public class PrivacySettingViewModel
        {
            public string SelectedPrivacySetting { get; set; }
            public IEnumerable<SelectListItem> PrivacySettingDDList { get; set; }
        }
    }
}


