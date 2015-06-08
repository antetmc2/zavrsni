using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using PagedList;

namespace zavrsni.Models
{
    public class HomeContentModel
    {
        [Key]
        public IPagedList<Content> contents { get; set; }
        public IPagedList<Content> contentsGuest { get; set; }
        [Display(Name = "Enter search keyword(s)")]
        public string keyword { get; set; }
    }

    public interface Top { }

    public class TopPag : Top
    {
        public int Page { get; set; }
        public double Avg { get; set; }
        public string TopPageName{ get; set; }
    }

    public class TopListModel
    {
        public IList<Page> topPages { get; set; }
        public IList<TopPag> topRatedPages { get; set; } 

    }

    public class SearchModel
    {
        public IPagedList<Content> results { get; set; }
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
        [Display(Name = "Choose 1 location type")]
        public SelectList LocationType { get; set; }
    }
}