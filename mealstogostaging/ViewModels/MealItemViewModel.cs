﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MealsToGo.ViewModels
{
    public class MealItemViewModel
    {
        public int MealItemId { get; set; }
        [Required]
        public string MealItemName { get; set; }
        public int UserId { get; set; }
        [Required]
        public string Ingredients { get; set; }
        [Required]
        public string ServingUnit { get; set; }
        public ItemViewModel ServingUnitDD { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
        [Required]
        public int Status { get; set; }
        public int ApprovalStatus { get; set; }
        public Nullable<System.DateTime> ApprovalStatusDate { get; set; }
        public ItemViewModel CusineTypeDD { get; set; }
        public ItemViewModel MealTypeDD { get; set; }
        public ItemViewModel DietTypeDD { get; set; }
        public List<Allergen> AllergenDD { get; set; }
        public List<MealItemsPhoto> Images { get; set; }
        public IEnumerable<HttpPostedFileBase> Imagelist { get; set; }
        


        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        //public string[] LKUPAllergenicFoods { get; set; }
        //public IEnumerable<SelectListItem> AllergenicFoodsDDList { get; set; }

        public MealItemViewModel()
        {
            CusineTypeDD = new ItemViewModel();
            MealTypeDD = new ItemViewModel();
            DietTypeDD = new ItemViewModel();
            ServingUnitDD = new ItemViewModel();
        }
    }
    public partial class MealItemsPhoto
    {
        public int MealItemPhotoID { get; set; }
        public int MealItemID { get; set; }
        public string Photo { get; set; }
        public int IsCover { get; set; }

       
    }
    //public class AllergenDDListViewModel
    //{
    //    public List<Allergens> AllergenDDList { get; set; }
    //}
    public class ItemViewModel
    {
        public string SelectedId { get; set; }
        public IEnumerable<SelectListItem> Items { get; set; }
    }
    public class Photos
    {
        public string Photo { get; set; }
        public bool Selected { get; set; }
    }
    public class Allergen
    {
        public string AllergenName { get; set; }
        public int AllergenID { get; set; }
        public bool Selected { get; set; }
    }
   
        //public CuisineTypeDDListViewModel()
        //{
        //    CuisineDDList = new[]
        //        {
        //            new SelectListItem { Value = "1", Text = "within 1 mile" },
        //            new SelectListItem { Value = "2", Text = "within 2 miles" },
        //            new SelectListItem { Value = "5", Text = "within 5 miles" },
        //            new SelectListItem { Value = "10", Text = "within 10 miles" },
        //            new SelectListItem { Value = "20", Text = "within 20 miles" },
        //            new SelectListItem { Value = "30", Text = "within 30 miles" },
        //            new SelectListItem { Value = "40", Text = "within 40 miles" },
        //            new SelectListItem { Value = "50", Text = "within 50 miles" }
        //        };
        //}
    

    //public class CuisineTypeDDListViewModel
    //{
    //    public string SelectedCuisine { get; set; }
    //    public IEnumerable<SelectListItem> DistanceDDList { get; set; }

    //    public CuisineTypeDDListViewModel()
    //    {
    //        DistanceDDList = new[]
    //            {
    //                new SelectListItem { Value = "1", Text = "within 1 mile" },
    //                new SelectListItem { Value = "2", Text = "within 2 miles" },
    //                new SelectListItem { Value = "5", Text = "within 5 miles" },
    //                new SelectListItem { Value = "10", Text = "within 10 miles" },
    //                new SelectListItem { Value = "20", Text = "within 20 miles" },
    //                new SelectListItem { Value = "30", Text = "within 30 miles" },
    //                new SelectListItem { Value = "40", Text = "within 40 miles" },
    //                new SelectListItem { Value = "50", Text = "within 50 miles" }
    //            };
    //    }
    //}

    //public class CuisineTypeDDListViewModel
    //{
    //    public string SelectedCuisine { get; set; }
    //    public IEnumerable<SelectListItem> DistanceDDList { get; set; }

    //    public CuisineTypeDDListViewModel()
    //    {
    //        DistanceDDList = new[]
    //            {
    //                new SelectListItem { Value = "1", Text = "within 1 mile" },
    //                new SelectListItem { Value = "2", Text = "within 2 miles" },
    //                new SelectListItem { Value = "5", Text = "within 5 miles" },
    //                new SelectListItem { Value = "10", Text = "within 10 miles" },
    //                new SelectListItem { Value = "20", Text = "within 20 miles" },
    //                new SelectListItem { Value = "30", Text = "within 30 miles" },
    //                new SelectListItem { Value = "40", Text = "within 40 miles" },
    //                new SelectListItem { Value = "50", Text = "within 50 miles" }
    //            };
    //    }
    //}

  
     
}

    