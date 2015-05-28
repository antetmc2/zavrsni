using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace zavrsni.Models
{
    public class IndexPageModel
    {
        [Key]
        public IList<Page> pages { get; set; }
        public string Username { get; set; }
    }

    public class NewPageModel
    {
        [Required]
        [Display(Name = "Page Title")]
        [DataType(DataType.Text)]
        public string PageTitle { get; set; }
        [Required]
        [Display(Name = "Date created")]
        public DateTime CreatedAt { get; set; }
        [Required]
        [Display(Name = "Number of Views")]
        public int PageViews { get; set; }
        public SelectList Privacy { get; set; }
        public string Username { get; set; }
    }

    public class EditPageModel
    {
        [Display(Name = "Enter a page title")]
        [DataType(DataType.Text)]
        public string PageTitle { get; set; }
        [Display(Name = "Choose a privacy settings")]
        public SelectList Privacy { get; set; }
        [Display(Name = "Enter a tag")]
        public string Tag { get; set; }
        [Display(Name = "Enter a contributor")]
        public string Contributor { get; set; }
        public int IDpage { get; set; }
        [Display(Name = "List of added tags")]
        public IList<PageTag> TagList { get; set; }
        [Display(Name = "List of added contributors")]
        public IList<Contributor> ContributorList { get; set; }
        public string Username { get; set; }
    }

    public class PageDetailModel
    {
        [Key]
        public List<LocationContent> PageContents { get; set; }
        public string PageName { get; set; }
        public int IDpage { get; set; }
        public string Username { get; set; }

    }

    public class PageRemoveModel
    {
        public int IDpage { get; set; }
    }
}