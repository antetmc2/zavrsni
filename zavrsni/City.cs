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
    
    public partial class City
    {
        public City()
        {
            this.User = new HashSet<User>();
            this.Location = new HashSet<Location>();
            this.LocationContent = new HashSet<LocationContent>();
        }
    
        public int IDcity { get; set; }
        public int IDcountry { get; set; }
        public string CityName { get; set; }
    
        public virtual Country Country { get; set; }
        public virtual ICollection<User> User { get; set; }
        public virtual ICollection<Location> Location { get; set; }
        public virtual ICollection<LocationContent> LocationContent { get; set; }
    }
}
