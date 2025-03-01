﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MealsToGo.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ThreeSixtyTwoEntities : DbContext
    {
        public ThreeSixtyTwoEntities()
            : base("name=ThreeSixtyTwoEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AddressList> AddressLists { get; set; }
        public DbSet<AllEmail> AllEmails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<ContactList> ContactLists { get; set; }
        public DbSet<country> countries { get; set; }
        public DbSet<EmailSubscription> EmailSubscriptions { get; set; }
        public DbSet<LKUPActivityType> LKUPActivityTypes { get; set; }
        public DbSet<LKUPAssociationType> LKUPAssociationTypes { get; set; }
        public DbSet<LKUPCountry> LKUPCountries { get; set; }
        public DbSet<LKUPCuisineType> LKUPCuisineTypes { get; set; }
        public DbSet<LKUPDietType> LKUPDietTypes { get; set; }
        public DbSet<LKUPMealType> LKUPMealTypes { get; set; }
        public DbSet<LKUPPrivacySetting> LKUPPrivacySettings { get; set; }
        public DbSet<LKUPServingUnit> LKUPServingUnits { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<SendText> SendTexts { get; set; }
        public DbSet<state> states { get; set; }
        public DbSet<UserPaymentOption> UserPaymentOptions { get; set; }
        public DbSet<NotificationFrequency> NotificationFrequencies { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<LocationsSearched> LocationsSearcheds { get; set; }
        public DbSet<MealItems_Photos> MealItems_Photos { get; set; }
        public DbSet<LKUPAllergenicFood> LKUPAllergenicFoods { get; set; }
        public DbSet<MealItem> MealItems { get; set; }
        public DbSet<MealItems_AllergenicFoods> MealItems_AllergenicFoods { get; set; }
        public DbSet<webpages_Membership> webpages_Membership { get; set; }
        public DbSet<TempOrderList> TempOrderLists { get; set; }
        public DbSet<TempUserOrder> TempUserOrders { get; set; }
        public DbSet<FunOrderDetail> FunOrderDetails { get; set; }
        public DbSet<FunOrder> FunOrders { get; set; }
        public DbSet<SendEmail> SendEmails { get; set; }
        public DbSet<ActiveMealAd> ActiveMealAds { get; set; }
        public DbSet<LKUPKitchenType> LKUPKitchenTypes { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<SellerDefaultSetting> SellerDefaultSettings { get; set; }
        public DbSet<UserAgreement> UserAgreements { get; set; }
        public DbSet<UserAgreementsAcceptanceDetail> UserAgreementsAcceptanceDetails { get; set; }
        public DbSet<AvailabilityType> AvailabilityTypes { get; set; }
        public DbSet<MealAds_PaymentOptions> MealAds_PaymentOptions { get; set; }
        public DbSet<PaymentOption> PaymentOptions { get; set; }
        public DbSet<LKUPDeliveryMethod> LKUPDeliveryMethods { get; set; }
        public DbSet<MealAds_DeliveryMethods> MealAds_DeliveryMethods { get; set; }
        public DbSet<MealAd> MealAds { get; set; }
        public DbSet<MealAd_Schedules> MealAd_Schedules { get; set; }
    }
}
