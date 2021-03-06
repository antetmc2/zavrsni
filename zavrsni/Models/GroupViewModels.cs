﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace zavrsni.Models
{
    public class GroupListModel
    {
        public IPagedList<Group> GroupList { get; set; }
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
        [Display(Name = "Add the new member to the list: ")]
        public SelectList MembersNotInList { get; set; } 
    }

    public class GroupEditDetailsModel
    {
        public int IDgroup { get; set; }
        [Display(Name = "Choose a group type")]
        public SelectList GroupType { get; set; }
        [Display(Name = "Group name")]
        public string Name { get; set; }
    }
}