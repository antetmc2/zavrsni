using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace zavrsni.Models
{
    public class HomeContentModel
    {
        [Key]
        public IList<Content> contents { get; set; }
    }

    public class CityCountryListModel
    {
        public IList<City> Cities { get; set; }

        [Display(Name = "Choose a country")]
        public SelectList Country { get; set; }

        [Display(Name = "City name")]
        public string CityName { get; set; }

        [Display(Name = "Country name")]
        public string CountryName { get; set; }
    }
}