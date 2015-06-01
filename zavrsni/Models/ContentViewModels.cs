using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using PagedList;
using PagedList.Mvc;

namespace zavrsni.Models
{
    public class AddNewContentModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Choose content type")]
        public SelectList ContentType { get; set; }

        [Display(Name = "Title of your content")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Text")]
        [DataType(DataType.Text)]
        public string Text { get; set; }

        [Display(Name = "Choose a page")]
        public SelectList Page { get; set; }

        [Display(Name = "Choose a location")]
        public SelectList Location { get; set; }
    }

    public class ImagesViewModel
    {
        public string Url { get; set; }
    }

    public class IndexContentModel
    {
        [Key]
        public IPagedList<Content> contents { get; set; }
        public string Username { get; set; }
    }

    public class UsernameModel
    {
        public string Username { get; set; }
    }

    public class ContentDelete
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDcontent { get; set; }

        [Required]
        public int IDcontentType { get; set; }

        [Required]
        public int IDauthor { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Text { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string URL { get; set; }

        [Required]
        public int? IDeditor { get; set; }

        [Required]
        public DateTime? TimeChanged { get; set; }

        [Required]
        public string Title { get; set; }

    }

    public class Contents
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Number")]
        public int IDcontent { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Choose content type")]
        public SelectList ContentType { get; set; }

        [Display(Name = "Title of your content")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Text")]
        [DataType(DataType.Text)]
        public string Text { get; set; }

        [Display(Name = "Editor")]
        public int Editor { get; set; }

        [Display(Name = "Changed")]
        public DateTime TimeChanged { get; set; }

        [Display(Name = "Choose a page")]
        public SelectList Page { get; set; }

        [Display(Name = "Choose a location")]
        public SelectList Location { get; set; }

        public IList<ContentPage> Pages { get; set; }
        public IList<LocationContent> Locations { get; set; } 

    }
    public class ContentDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Number")]
        public int IDcontent { get; set; }

        [Display(Name = "Username")]
        public string Author { get; set; }

        [Display(Name = "Choose content type")]
        public string ContentType { get; set; }

        [Display(Name = "Title of your content")]
        public string Title { get; set; }

        [Display(Name = "Text")]
        [DataType(DataType.Text)]
        public string Text { get; set; }

        [Display(Name = "Editor")]
        public string Editor { get; set; }

        [Display(Name = "Changed")]
        public DateTime? TimeChanged { get; set; }

        [Display(Name = "Choose a page")]
        public SelectList Page { get; set; }
        public IList<LocationContent> Locations { get; set; }
        public string Username { get; set; }

    }
}