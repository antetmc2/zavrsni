//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace zavrsni
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.BelongsToGroup = new HashSet<BelongsToGroup>();
            this.Content = new HashSet<Content>();
            this.Content2 = new HashSet<Content>();
            this.ContentComment = new HashSet<ContentComment>();
            this.ContentPage = new HashSet<ContentPage>();
            this.Contributor = new HashSet<Contributor>();
            this.Group = new HashSet<Group>();
            this.Page = new HashSet<Page>();
            this.Page1 = new HashSet<Page>();
            this.PageReview = new HashSet<PageReview>();
        }
    
        public int IDuser { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> IDprofilePic { get; set; }
        public int IDcityFrom { get; set; }
    
        public virtual ICollection<BelongsToGroup> BelongsToGroup { get; set; }
        public virtual City City { get; set; }
        public virtual ICollection<Content> Content { get; set; }
        public virtual Content Content1 { get; set; }
        public virtual ICollection<Content> Content2 { get; set; }
        public virtual ICollection<ContentComment> ContentComment { get; set; }
        public virtual ICollection<ContentPage> ContentPage { get; set; }
        public virtual ICollection<Contributor> Contributor { get; set; }
        public virtual ICollection<Group> Group { get; set; }
        public virtual ICollection<Page> Page { get; set; }
        public virtual ICollection<Page> Page1 { get; set; }
        public virtual ICollection<PageReview> PageReview { get; set; }
    }
}
