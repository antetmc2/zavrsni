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
    
    public partial class Tag
    {
        public Tag()
        {
            this.Page = new HashSet<Page>();
        }
    
        public int ID { get; set; }
        public string name { get; set; }
    
        public virtual ICollection<Page> Page { get; set; }
    }
}