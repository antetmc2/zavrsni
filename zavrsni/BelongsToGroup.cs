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
    
    public partial class BelongsToGroup
    {
        public int IDgroup { get; set; }
        public int IDuser { get; set; }
        public Nullable<System.DateTime> TimeChanged { get; set; }
    
        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}
