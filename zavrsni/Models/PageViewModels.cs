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
    }

    public class NewPageModel
    {
        [Required]
        [Display(Name = "Page Title")]
        [DataType(DataType.Text)]
        public string PageTitle { get; set; }
        [Required]
        [Display(Name = "Privacy")]
        public int Privacy { get; set; }
        [Required]
        [Display(Name = "Date created")]
        public DateTime CreatedAt { get; set; }
        [Required]
        [Display(Name = "Number of Views")]
        public int PageViews { get; set; }
    }

    public class PageDetailModel
    {
        [Key]
        public List<LocationContent> PageContents { get; set; }
    }
}