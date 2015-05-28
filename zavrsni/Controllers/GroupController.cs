using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using zavrsni.Models;

namespace zavrsni.Controllers
{
    public class GroupController : Controller
    {
        public async Task<ActionResult> Index(GroupListModel model)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var currentUser = User.Identity.GetUserName();
                var user = db.User.FirstOrDefault(u => u.Username.Equals(currentUser));

                var groups = (from u in db.Group
                              where u.IDgroupOwner == user.IDuser
                              || u.IDgroupOwner == null
                              select u).Include(u => u.GroupType).Include(u => u.User).ToList();

                model.GroupList = groups;

                return View(model);

            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                AddNewGroupModel model = new AddNewGroupModel();
                var groupTypes = (from g in db.GroupType
                    orderby g.Name
                    select g).ToList();
                model.GroupType = new SelectList(groupTypes, "ID", "Name");
                return View(model);
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Add(AddNewGroupModel model)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var username = User.Identity.GetUserName();
                var user = db.User.FirstOrDefault(u => u.Username.Equals(username));

                var newGroup = db.Group.Create();
                if (Request["GroupTypeDropDown"].Any())
                {
                    var groupTypeSel = Request["GroupTypeDropDown"];
                    var gt = Convert.ToInt32(groupTypeSel);
                    newGroup.IDgroupType = gt;
                }
                newGroup.Name = model.Name;
                newGroup.IDgroupOwner = user.IDuser;
                db.Group.Add(newGroup);
                db.SaveChanges();
                return RedirectToAction("Index", "Group");
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Delete(int IDgroup)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                if (IDgroup == 1) return RedirectToAction("Index", "Group");
                var groupDelete = db.Group.Find(IDgroup);
                db.Group.Remove(groupDelete);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Group");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Details(int IDgroup)
        {
            GroupDetailsModel model = new GroupDetailsModel();
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var groupQuery = (from g in db.Group
                                  where g.IDgroup == IDgroup
                    select g).Include(g => g.GroupType);
                model.GroupType = groupQuery.FirstOrDefault().GroupType.Name;
                model.Name = groupQuery.FirstOrDefault().Name;
                model.IDgroup = IDgroup;

                var membersQuery = (from b in db.BelongsToGroup
                    where b.IDgroup == IDgroup
                    select b).Include(b => b.User).ToList();
                model.Members = membersQuery;
            }
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult DeleteMember(int IDgroup, int IDuser)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                if (IDgroup == 1) return RedirectToAction("Index", "Group");

                var groupMemberDelete = db.BelongsToGroup.Find(IDgroup, IDuser);
                db.BelongsToGroup.Remove(groupMemberDelete);
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { IDgroup = IDgroup });
        }
    }
}