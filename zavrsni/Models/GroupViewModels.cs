using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace zavrsni.Models
{
    public class GroupListModel
    {
        public IList<Group> GroupList { get; set; }
    }

    public class AddNewGroupModel
    {
        [Required]
        [Display(Name = "Choose a group type")]
        public SelectList GroupType { get; set; }

        [Required]
        [Display(Name = "Group name")]
        public string Name { get; set; }
    }

    public class GroupDetailsModel
    {
        public int IDgroup { get; set; }
        public string GroupType { get; set; }
        public string Name { get; set; }
        public IList<BelongsToGroup> Members { get; set; } 
    }
}