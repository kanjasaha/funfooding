using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MealsToGo.Models;
using MealsToGo.ViewModels;
using System.Web.Mvc;

namespace MealsToGo
{
    public class AutoMapperConfiguration
    {

        public static void Configure()
        {
            Mapper.CreateMap<LoginRegisterViewModel, LoginModel>();
            Mapper.CreateMap<LoginRegisterViewModel, RegisterModel>();
            Mapper.CreateMap<UserDetail, UserDetailViewModel>();
            Mapper.CreateMap<UserDetailViewModel, UserDetail>();
            Mapper.CreateMap<AddressViewModel, AddressList>();
            Mapper.CreateMap<AddressList, AddressViewModel>();
            Mapper.CreateMap<MealItem, MealItemViewModel>();
            Mapper.CreateMap<MealAd_Schedules, MealAdSchedule>();
            Mapper.CreateMap<MealAdSchedule, MealAd_Schedules>();
            // .ForMember(d => d.Status, m => m.MapFrom(p => p.Status == 1 ? true : false));
            Mapper.CreateMap<MealAd, MealAdViewModel>();

            Mapper.CreateMap<UserSetting, UserSettingsViewModel>().ForMember(d => d.PrivacySetting, m => m.MapFrom(p => p.LKUPPrivacySetting.PrivacySettings))
                                                                  .ForMember(d => d.ReceiveEmailNotification, m => m.MapFrom(p => p.NotificationFrequency.Description))
                                                                   .ForMember(d => d.ReceiveMobileTextNotification, m => m.MapFrom(p => p.NotificationFrequency.Description));
                                                                  // .ForMember(d => d.ReceiveEmailNotification, m => m.AddFormatter<VipFormatter>())
                                                                 //    .ForMember(d => d.ReceiveMobileTextNotification, m => m.AddFormatter<VipFormatter>());
            Mapper.CreateMap<UserSettingsViewModel, UserSetting>();

            Mapper.CreateMap<MealItems_AllergenicFoods, Allergen>()
                  .ForMember(d => d.AllergenID, opt => opt.MapFrom(s => s.AllergenicFoodID));
            Mapper.CreateMap<MealItems_Photos, MealItemsPhoto>()
                 .ForMember(d => d.MealItemPhotoID, opt => opt.MapFrom(s => s.MealItemPhotoID))
             .ForMember(d => d.MealItemID, opt => opt.MapFrom(s => s.MealItemID))
              .ForMember(d => d.IsCover, opt => opt.MapFrom(s => s.IsCover))
             .ForMember(d => d.Photo, opt => opt.MapFrom(s => s.Photo));


            Mapper.CreateMap<MealItemViewModel, MealItem>().ForMember(d => d.MealTypeID, m => m.MapFrom(p => int.Parse(p.MealTypeDD.SelectedId)))
                                                             .ForMember(d => d.CusineTypeID, m => m.MapFrom(p => int.Parse(p.CusineTypeDD.SelectedId)))
                                                             .ForMember(d => d.DietTypeID, m => m.MapFrom(p => int.Parse(p.DietTypeDD.SelectedId)))
                                                             .ForMember(d => d.ServingUnit, m => m.MapFrom(p => int.Parse(p.ServingUnitDD.SelectedId)))
                                                             .AfterMap((s, d) =>
                                                                          {
                                                                              foreach (var mealaller in d.MealItems_AllergenicFoods)

                                                                                  mealaller.MealItemID = s.MealItemId;


                                                                          })

                                                                 .AfterMap((s, d) =>
                                                                 {
                                                                     foreach (var ph in d.MealItems_Photos)

                                                                         ph.MealItemID = s.MealItemId;


                                                                 });

            Mapper.CreateMap<MealAdViewModel, MealAd>().ForMember(d => d.MealItemID, m => m.MapFrom(p => int.Parse(p.MealItemsDD.SelectedMealItem)))
                                                            .ForMember(d => d.AvailabilityTypeID, m => m.MapFrom(p => int.Parse(p.AvailabilityTypeDD.SelectedAvailabilityType)))
                                                            //.ForMember(d => d.MealItem.MealItemName, m => m.MapFrom(p => p.MealItemName))
                                                            .AfterMap((s, d) =>
                                                            {
                                                                foreach (var mealamealadpayment in d.MealAds_PaymentOptions)

                                                                    mealamealadpayment.MealAdID = s.MealAdID;


                                                            })

                                                             .AfterMap((s, d) =>
                                                             {
                                                                 foreach (var mealaddel in d.MealAds_DeliveryMethods)

                                                                     mealaddel.MealAdID = s.MealAdID;


                                                             }).AfterMap((s, d) =>
                                                             {
                                                                 if (d.MealItem != null)
                                                                     s.MealItemName = d.MealItem.MealItemName;


                                                             });



            Mapper.CreateMap<Allergen, MealItems_AllergenicFoods>()
                  .ForMember(d => d.AllergenicFoodID, opt => opt.MapFrom(s => s.AllergenID));
            Mapper.CreateMap<MealItemsPhoto, MealItems_Photos>()
                 .ForMember(d => d.MealItemPhotoID, opt => opt.MapFrom(s => s.MealItemPhotoID))
             .ForMember(d => d.MealItemID, opt => opt.MapFrom(s => s.MealItemID))
              .ForMember(d => d.IsCover, opt => opt.MapFrom(s => s.IsCover))
             .ForMember(d => d.Photo, opt => opt.MapFrom(s => s.Photo) );
        }



        public class VipFormatter : IValueFormatter
        {
            public string FormatValue(ResolutionContext context)
            {


                if (context.SourceValue.ToString() == "1")
                {
                    return "Yes";
                }


                return "No";

            }
        }

    }
}