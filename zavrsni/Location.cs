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
    
    public partial class Location
    {
        public Location()
        {
            this.LocationContent = new HashSet<LocationContent>();
        }
    
        public int IDlocation { get; set; }
        public int IDcity { get; set; }
        public int IDlocationType { get; set; }
        public Nullable<System.DateTime> TimeChanged { get; set; }
    
        public virtual City City { get; set; }
        public virtual LocationType LocationType { get; set; }
        public virtual ICollection<LocationContent> LocationContent { get; set; }
    }
}
