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
    
    public partial class PageTag
    {
        public int IDtag { get; set; }
        public int IDpage { get; set; }
        public Nullable<System.DateTime> TimeChanged { get; set; }
    
        public virtual Page Page { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
