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
        public IList<Page> pagesPublic { get; set; }
        public string Username { get; set; }
        public bool IsMember { get; set; }
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
        [Required]
        [Display(Name = "Privacy settings")]
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
        public string PageAuthor { get; set; }
        public IList<User> Contributors { get; set; } 
        public short Grade { get; set; }
        public double AverageGrade { get; set; }
        public IList<ContentComment> AllComments { get; set; }
        public string Comment { get; set; }
    }

    public class PageRemoveModel
    {
        public int IDpage { get; set; }
    }

    public class Serialized
    {
        public int col { get; set; }
        public int row { get; set; }
        public int size_x { get; set; }
        public int size_y { get; set; }
    }
}