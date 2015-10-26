using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MealsToGo.ViewModels
{
    public class NetworkViewModel
    {
        public List<ContactsWaiting> Contacts { get; set; }
        public List<InnerCircle> InnerCircleContacts { get; set; }
        public List<OuterCircle> OuterCircleContacts { get; set; }
        public List<Business> BusinessContacts { get; set; }
        public  NetworkViewModel()
        {}
    }
  
}

  public class ContactsWaiting
{
    public string EmailAddress { get; set; }
    public int? Accepted { get; set; }
    public int Sender { get; set; }
      

     public ContactsWaiting() {
}
}

  public class InnerCircle
  {
      public string Name { get; set; }
      public int SharesFood { get; set; }
      public int BoughtFoodFromUser { get; set; }
      public int SoldFoodToUser { get; set; }
     
      public InnerCircle()
      {
      }
  }

  public class OuterCircle
  {
      public string Name { get; set; }
      public int SharesFood { get; set; }
      public int BoughtFoodFromUser { get; set; }
      public int SoldFoodToUser { get; set; }

      public OuterCircle()
      {
      }
  }

  public class Business
  {
      public string Name { get; set; }
      public int SharesFood { get; set; }
      public int BoughtFoodFromUser { get; set; }
      public int SoldFoodToUser { get; set; }

      public Business()
      {
      }
  }