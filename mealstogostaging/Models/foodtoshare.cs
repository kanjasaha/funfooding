using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SolrNet.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MealsToGo.Models
{
    public class foodtoshare
    {


        public string Country { get; set; }

        public string PhoneNumber { get; set; }

        public string FullAddress { get; set; }

        public string latlng { get; set; }

        public string Distance { get; set; }

        public string ProviderType { get; set; }

        public int MealAdID { get; set; }

        public string MealItemName { get; set; }

        [SolrField("Cuisine")]
        public string Cuisine { get; set; }

        [SolrField("Meal")]
        public string MealType { get; set; }

        [SolrField("Price")]
        public decimal Price { get; set; }

        public DateTime Timestamp { get; set; }

        public string Description { get; set; }

        public string Ingredients { get; set; }

        [SolrField("Allergen")]
        public string AllergenicIngredients { get; set; }

        [SolrField("PriceRange")]
        public string PriceRange { get; set; }

        [SolrField("Diet")]
        public string FoodType { get; set; }

        [Display(Name = "Kitchen Name")]
        public string ProviderName { get; set; }

               
    }
}