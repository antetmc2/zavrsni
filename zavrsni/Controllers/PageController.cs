using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using zavrsni.Models;
using System.Data.Entity.Infrastructure;
using System.Web.Helpers;

namespace zavrsni.Controllers
{
    public class PageController : Controller
    {
        // GET: Page
        public ActionResult Index(string username)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var currentUser = User.Identity.GetUserName();
                var user = db.User.FirstOrDefault(u => u.Username.Equals(username));
                var current = db.User.FirstOrDefault(u => u.Username.Equals(currentUser));
                var allPages = (from p in db.Page
                    join c in db.Contributor
                        on p.IDpage equals c.IDpage
                        where c.IDuser == user.IDuser
                        //&& c.IsAuthor
                    select p).ToList();

                var isContributor = (from a in allPages
                    join c in db.Contributor on a.IDpage equals c.IDpage
                    where c.IDuser == current.IDuser
                    select a).ToList();

                var pagesPublic = (from a in allPages
                                   join c in db.Contributor on a.IDpage equals c.IDpage
                                   where a.IDprivacy >=2
                                   select a).Except(from a in allPages
                                                    join c in db.Contributor on a.IDpage equals c.IDpage
                                                    where c.IDuser == current.IDuser
                                                    select a).ToList();

                var isGroupMember = (from g in db.Group
                    join b in db.BelongsToGroup on g.IDgroup equals b.IDgroup
                    where g.IDgroupOwner == user.IDuser
                          && b.IDuser == current.IDuser
                          && g.IDgroup > 1
                    select b).ToList();

                var model = new IndexPageModel()
                {
                    pages = isContributor,
                    Username = username,
                    pagesPublic = pagesPublic,
                    IsMember = isGroupMember.Any()
                };
                return View(model);
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int IDpage, string username)
        {
            EditPageModel model = new EditPageModel();
            model.Username = username;
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                model.IDpage = IDpage;
                var selPage = db.Page.FirstOrDefault(u => u.IDpage.Equals(IDpage));
                var query = (from p in db.Privacy
                    orderby p.Description
                    select p).ToList();
                model.PageTitle = selPage.name;
                model.Privacy = new SelectList(query, "IDprivacy", "description", selPage.IDprivacy);

                var tagList = (from t in db.PageTag
                    join a in db.Tag on t.IDtag equals a.ID
                    where t.IDpage == IDpage
                    orderby a.name
                    select t).Include(t => t.Tag).ToList();
                model.TagList = tagList;

                var contribList = (from c in db.Contributor
                    join u in db.User on c.IDuser equals u.IDuser
                    orderby u.Username
                    where c.IDpage == IDpage
                    select c).Include(c => c.User).ToList();
                model.ContributorList = contribList;
            }
            return View(model);
        }

        [Authorize]
        [HttpPost, ActionName("Edit")]
        public async Task<ActionResult> EditPage(int IDpage, string username, EditPageModel model)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var selPage = db.Page.FirstOrDefault(u => u.IDpage.Equals(IDpage));
                if (ModelState.IsValid)
                {
                    var user = db.User.FirstOrDefault(u => u.Username.Equals(username));
                    selPage.name = model.PageTitle;

                    if (Request["PrivacyDropDown"].Any())
                    {
                        var privSel = Request["PrivacyDropDown"];
                        var privacy = Convert.ToInt32(privSel);

                        selPage.IDprivacy = privacy;
                    }

                    selPage.IDeditor = user.IDuser;
                    selPage.TimeChanged = DateTime.Now;

                    if (model.Tag != null)
                    {
                        var tagModel = model.Tag.ToLower();
                        var existsInPage = from p in db.PageTag
                            join t in db.Tag on p.IDtag equals t.ID
                            where t.name == tagModel
                            select t;
                        var existsTag = from t in db.Tag
                            where t.name == tagModel
                            select t;

                        if (!existsTag.Any())
                        {
                            var newTag = db.Tag.Create();
                            newTag.name = tagModel;
                            db.Tag.Add(newTag);
                            db.SaveChanges();
                        }
                        if (!existsInPage.Any())
                        {
                            var newPageTag = db.PageTag.Create();
                            newPageTag.IDtag = existsTag.First().ID;
                            newPageTag.IDpage = IDpage;
                            db.PageTag.Add(newPageTag);
                            db.SaveChanges();
                        }
                    }

                    if (model.Contributor != null)
                    {
                        var userExists = from u in db.User
                            where u.Username == model.Contributor
                            select u;

                        if (!userExists.Any()) return RedirectToAction("Edit", new { IDpage = IDpage, Username = username });

                        var exists = from t in db.Contributor
                            join u in db.User on t.IDuser equals u.IDuser
                            where u.Username == model.Contributor
                            select t;

                        if (!exists.Any())
                        {
                            var contribUser = db.User.FirstOrDefault(u => u.Username.Equals(model.Contributor));
                            var newContributor = db.Contributor.Create();
                            newContributor.IDpage = IDpage;
                            newContributor.IDuser = contribUser.IDuser;
                            newContributor.IsAuthor = false;
                            db.Contributor.Add(newContributor);
                            db.SaveChanges();
                        }


                    }

                    db.Entry(selPage).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Edit", new { IDpage = IDpage, Username = username });
                }
            }
            return RedirectToAction("Edit", new { IDpage = IDpage, Username = username });
        }

        [Authorize]
        [HttpGet]
        public ActionResult DeleteTag(int IDpage, int IDtag, string username)
        {
            UsernameModel model = new UsernameModel();
            model.Username = username;
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var tagDelete = db.PageTag.Find(IDtag, IDpage);
                db.PageTag.Remove(tagDelete);
                db.SaveChanges();
            }

            return RedirectToAction("Edit", new { IDpage = IDpage, Username = username });
        }

        [Authorize]
        [HttpGet]
        public ActionResult DeleteContributor(int IDpage, int IDuser, string username)
        {
            UsernameModel model = new UsernameModel();
            model.Username = username;
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var contributorDelete = db.Contributor.Find(IDpage, IDuser);
                db.Contributor.Remove(contributorDelete);
                db.SaveChanges();
            }

            return RedirectToAction("Edit", new { IDpage = IDpage, Username = username });
        }

        [HttpGet]
        public ActionResult Details(int IDpage, string username)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                List<LocationContent> query = (from c in db.LocationContent
                    join p in db.ContentPage on c.IDcontent equals p.IDcontent
                    where p.IDpage == IDpage
                    select c).Include(c => c.Content).Include(c => c.Location).Include(c => c.City).ToList();

                var PageInfo = (from p in db.Page
                    where p.IDpage == IDpage
                    select p);

                var selPage = db.Page.FirstOrDefault(u => u.IDpage.Equals(IDpage));
                var views = selPage.PageView;
                views++;
                selPage.PageView = views;
                db.Entry(selPage).State = EntityState.Modified;
                db.SaveChanges();
                var average = (from p in db.PageReview
                    where p.IDpage == IDpage
                    select p).Average(p => p.Mark);

                if (!Request.IsAuthenticated)
                {
                    var model = new PageDetailModel
                    {
                        PageContents = query,
                        PageName = PageInfo.First().name,
                        IDpage = PageInfo.First().IDpage,
                        AverageGrade = average
                    };
                    return View(model);
                }
                else
                {
                    var model = new PageDetailModel
                    {
                        PageContents = query,
                        PageName = PageInfo.First().name,
                        IDpage = PageInfo.First().IDpage,
                        Username = username,
                        AverageGrade = average
                    };
                    return View(model);
                }
            }
        }

        [HttpPost, ActionName("Details")]
        public async Task<ActionResult> ViewDetails(int IDpage, string username, PageDetailModel model)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var currentUser = User.Identity.GetUserName();
                var user = db.User.FirstOrDefault(u => u.Username.Equals(username));
                var current = db.User.FirstOrDefault(u => u.Username.Equals(currentUser));

                var reviewExists = (from p in db.PageReview
                    where p.IDpage == IDpage
                    && p.IDreviewer == current.IDuser
                    select p).ToList();

                if (reviewExists.Any())
                {
                    var selReview = db.PageReview.Find(IDpage, current.IDuser);
                    selReview.Mark = model.Grade;
                    db.Entry(selReview).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (!reviewExists.Any())
                {
                    var selReview = db.PageReview.Create();
                    selReview.IDpage = IDpage;
                    selReview.IDreviewer = current.IDuser;
                    selReview.Mark = model.Grade;
                    db.PageReview.Add(selReview);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Details", new { IDpage = IDpage, Username = username });
        }

        [Authorize]
        [HttpGet]
        public ActionResult Delete(int IDpage, string username)
        {
            UsernameModel model = new UsernameModel();
            model.Username = username;
            return View(model);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeletePage(int IDpage, string username)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var page = db.Page.FirstOrDefault(u => u.IDpage.Equals(IDpage));

                var pageDelete = db.Page.Find(IDpage);
                db.Page.Remove(pageDelete);
                db.SaveChanges();
            }

            return RedirectToAction("Index", new { Username = username });
        }

        public ActionResult DeleteContent(int IDpage, int IDcontent, string username)
        {
            UsernameModel model = new UsernameModel();
            model.Username = username;
            return View(model);
        }

        [HttpPost, ValidateInput(false), ActionName("DeleteContent")]
        public async Task<ActionResult> DeleteContentConfirm(int IDpage, int IDcontent, string username)
        {

            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var contentDelete = db.ContentPage.Find(IDcontent, IDpage);
                db.ContentPage.Remove(contentDelete);
                db.SaveChanges();
            }

            return RedirectToAction("Details", new { IDpage = IDpage });
        }

        [Authorize]
        [HttpGet]
        public ActionResult NewPage(string username)
        {
            NewPageModel model = new NewPageModel();
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var query = (from p in db.Privacy
                    select p).ToList();
                model.Privacy = new SelectList(query, "IDprivacy", "description");
                model.Username = username;
                return View(model);
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> NewPage(string username, NewPageModel model)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var user = db.User.FirstOrDefault(u => u.Username.Equals(username));
                var newPage = db.Page.Create();
                newPage.name = model.PageTitle;
                if (Request["PrivacyDropDown"].Any())
                {
                    var privSel = Request["PrivacyDropDown"];
                    var privacy = Convert.ToInt32(privSel);

                    newPage.IDprivacy = privacy;
                }
                newPage.CreatedAt = DateTime.Now;
                newPage.PageView = 0;

                db.Page.Add(newPage);
                db.SaveChanges();

                var newContributor = db.Contributor.Create();
                newContributor.IDpage = newPage.IDpage;
                newContributor.IDuser = user.IDuser;
                newContributor.IsAuthor = true;

                db.Contributor.Add(newContributor);
                db.SaveChanges();

                return RedirectToAction("Index", new { Username = username });


            }
        }
    }
}