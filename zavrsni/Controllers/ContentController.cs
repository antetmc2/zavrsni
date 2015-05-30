using System;
using System.Collections.Generic;
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
using System.Text.RegularExpressions;
using PagedList;

namespace zavrsni.Controllers
{
    public class ContentController : Controller
    {
        public ActionResult Edit(int IDcontent, string username)
        {
            Contents model = new Contents();
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var query = db.Content.FirstOrDefault(u => u.IDcontent.Equals(IDcontent));
                var queryLocation = db.LocationContent.FirstOrDefault(l => l.IDcontent.Equals(query.IDcontent));

                var user = db.User.FirstOrDefault(u => u.Username.Equals(username));

                if (query.IDauthor != user.IDuser) return RedirectToAction("Index", "Home");

                var query1 = (from p in db.Page
                    join c in db.Contributor on p.IDpage equals c.IDpage
                    where c.IDuser == user.IDuser
                              orderby p.name
                              select p).ToList();
                model.Page = new SelectList(query1, "IDpage", "name");

                var query2 = (from p in db.ContentType
                              orderby p.Description
                              select p).ToList();
                model.ContentType = new SelectList(query2, "ID", "Description", query.IDcontentType);

                var query3 = (from c in db.City
                              orderby c.CityName
                              select c).ToList();
                if (queryLocation != null)
                    model.Location = new SelectList(query3, "IDcity", "CityName", queryLocation.IDlocation);
                else model.Location = new SelectList(query3, "IDcity", "CityName");

                var query4 = (from c in db.Content
                    join d in db.ContentPage on c.IDcontent equals d.IDcontent
                    join p in db.Page on d.IDpage equals p.IDpage
                    where c.IDcontent == IDcontent
                    orderby p.name
                    select d).Include(d => d.Page).ToList();

                model.Pages = query4;

                var query5 = (from c in db.Content
                              join d in db.LocationContent on c.IDcontent equals d.IDcontent
                              join l in db.City on d.IDlocation equals l.IDcity
                              where c.IDcontent == IDcontent
                              orderby l.CityName
                              select d).Include(d => d.City).ToList();

                model.Locations = query5;

                model.Text = query.Text;
                model.Title = query.Title;
                model.Username = username;
            }
            return View(model);

        }

        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> Edit(int IDcontent, string username, Contents model)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var query = db.Content.FirstOrDefault(u => u.IDcontent.Equals(IDcontent));
                //var queryLocation = db.LocationContent.FirstOrDefault(l => l.IDcontent.Equals(IDcontent));
                //var queryPage = db.ContentPage.FirstOrDefault(l => l.IDcontent.Equals(IDcontent));
                if (ModelState.IsValid)
                {
                    var user = db.User.FirstOrDefault(u => u.Username.Equals(username));
                    // Get the userprofile

                    model.Username = username;

                    if (model.Title != null) query.Title = model.Title;
                    else query.Title = "(no title)"; query.Text = model.Text;
                    query.IDeditor = user.IDuser;
                    query.TimeChanged = DateTime.Now;

                    if (Request["PageDropDown"].Any())
                    {
                        var pageSel = Request["PageDropDown"];
                        var page = Convert.ToInt32(pageSel);

                        var exists = from cp in db.ContentPage
                                     where cp.IDpage == page
                                     && cp.IDcontent == IDcontent
                                     select cp;
                        if (!exists.Any())
                        {
                            var newPage = db.ContentPage.Create();
                            newPage.IDcontent = IDcontent;
                            newPage.IDpage = Convert.ToInt32(pageSel);
                            newPage.IDuser = user.IDuser;
                            db.ContentPage.Add(newPage);
                            db.SaveChanges();
                        }
                        else
                        {
                            ModelState.AddModelError("", "The selected page already contains this content.");
                            return RedirectToAction("Edit", new { IDcontent = IDcontent, Username = username });
                        }
                    }

                    if (Request["ContentTypeDropDown"].Any())
                    {
                        var contSel = Request["ContentTypeDropDown"];
                        query.IDcontentType = Convert.ToInt32(contSel);
                    }

                    /*if (Request["LocationEdit"].Any())// && queryLocation != null)
                    {
                        var locationSel = Request["LocationEdit"];
                        queryLocation.IDlocation = Convert.ToInt32(locationSel);
                        //db.Entry(queryLocation).State = EntityState.Modified;
                    }*/

                    if (Request["LocationEdit"].Any())
                    {
                        var locationSel = Request["LocationEdit"];
                        var loc = Convert.ToInt32(locationSel);

                        var exists = from lc in db.LocationContent
                                     where lc.IDlocation == loc
                                     && lc.IDcontent == IDcontent
                                     select lc;

                        if (!exists.Any())
                        {
                            var location = db.LocationContent.Create();
                            location.IDlocation = Convert.ToInt32(locationSel);
                            location.IDcontent = IDcontent;
                            location.TimeChanged = DateTime.Now;
                            db.LocationContent.Add(location);
                            db.SaveChanges();
                        }
                        else
                        {
                            ModelState.AddModelError("", "The selected location already contains this content.");
                            //return RedirectToAction("Edit", new { IDcontent = IDcontent });
                        }

                    }

                    db.Entry(query).State = EntityState.Modified;
                    //db.Entry(queryPage).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Edit", new { IDcontent = IDcontent, Username = username });
                }
            }
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult DeleteLocation(int IDlocation, int IDcontent, string username)
        {
            UsernameModel model = new UsernameModel();
            model.Username = username;
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var deleteLocationContent = db.LocationContent.Find(IDlocation, IDcontent);
                db.LocationContent.Remove(deleteLocationContent);
                db.SaveChanges();
            }

            return RedirectToAction("Edit", new { IDcontent = IDcontent, Username = username });
        }

        public ActionResult Details(int IDcontent, string username)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var cont = db.Content.FirstOrDefault(u => u.IDcontent.Equals(IDcontent));
                var usernameAuthor = db.User.FirstOrDefault(u => u.IDuser.Equals(cont.IDauthor));
                var usernameEditor = (from u in db.User
                    where u.IDuser == cont.IDeditor
                    select u).ToList();
                var contType = db.ContentType.FirstOrDefault(c => c.ID.Equals(cont.IDcontentType));
                var locations = (from l in db.LocationContent
                    join c in db.City on l.IDlocation equals c.IDcity
                    where l.IDcontent == IDcontent
                    orderby c.CityName
                    select l).Include(l => l.City).ToList();
                var pages = (from c in db.Contributor
                    join p in db.Page on c.IDpage equals p.IDpage
                    where c.IDuser == usernameAuthor.IDuser
                    orderby p.name
                    select p).ToList();
                try
                {
                    if (Request.IsAuthenticated)
                    {
                        ContentDetails model = new ContentDetails()
                        {
                            IDcontent = cont.IDcontent,
                            Author = usernameAuthor.Username,
                            ContentType = contType.Description,
                            Title = cont.Title,
                            Editor = usernameEditor.FirstOrDefault().Username,
                            TimeChanged = cont.TimeChanged,
                            Text = cont.Text,
                            Locations = locations,
                            Page = new SelectList(pages, "IDpage", "name"),
                            Username = username
                        };
                        return View(model);
                    }
                    else
                    {
                        ContentDetails model = new ContentDetails()
                        {
                            IDcontent = cont.IDcontent,
                            Author = usernameAuthor.Username,
                            ContentType = contType.Description,
                            Title = cont.Title,
                            Editor = usernameEditor.FirstOrDefault().Username,
                            TimeChanged = cont.TimeChanged,
                            Text = cont.Text,
                            Locations = locations,
                            Page = new SelectList(pages, "IDpage", "name"),
                        };
                        return View(model);
                    }
                }
                catch (NullReferenceException)
                {
                    if (Request.IsAuthenticated)
                    {
                        ContentDetails model = new ContentDetails()
                        {
                            IDcontent = cont.IDcontent,
                            Author = usernameAuthor.Username,
                            Title = cont.Title,
                            ContentType = contType.Description,
                            Text = cont.Text,
                            Locations = locations,
                            Username = username
                        };
                        return View(model);
                    }
                    else
                    {
                        ContentDetails model = new ContentDetails()
                        {
                            IDcontent = cont.IDcontent,
                            Author = usernameAuthor.Username,
                            Title = cont.Title,
                            ContentType = contType.Description,
                            Text = cont.Text,
                            Locations = locations,
                        };
                        return View(model);
                    }
                }

            }
        }

        [HttpPost, ValidateInput(false), ActionName("Details")]
        public async Task<ActionResult> ShowDetails(int IDcontent, string username, ContentDetails model)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var cont = db.Content.FirstOrDefault(u => u.IDcontent.Equals(IDcontent));
                var usernameAuthor = db.User.FirstOrDefault(u => u.IDuser.Equals(cont.IDauthor));
                var contPage = db.ContentPage.Create();
                contPage.IDcontent = IDcontent;
                contPage.IDuser = usernameAuthor.IDuser;
                if (Request["PageDropDown"].Any())
                {
                    var pageSel = Request["PageDropDown"];
                    contPage.IDpage = Convert.ToInt32(pageSel);
                    db.ContentPage.Add(contPage);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Details", new { IDcontent = IDcontent, Username = username });
        }

        public ActionResult Delete(int IDcontent, string username)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {

                var query = db.Content.FirstOrDefault(u => u.IDcontent.Equals(IDcontent));

                var user = db.User.FirstOrDefault(u => u.Username.Equals(username));

                if (query.IDauthor != user.IDuser) return RedirectToAction("Index", "Home");

            }
            UsernameModel model = new UsernameModel();
            model.Username = username;

            return View(model);
        }

        [HttpPost, ValidateInput(false), ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirm(int IDcontent, string username)
        {

            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var cont = db.Content.FirstOrDefault(u => u.IDcontent.Equals(IDcontent));

                var contentDelete = db.Content.Find(IDcontent);
                db.Content.Remove(contentDelete);
                db.SaveChanges();
            }

            return RedirectToAction("ViewContent", new { Username = username });
        }

        [Authorize]
        [HttpGet]
        public ActionResult ViewContent(string username, int page = 1, int pageSize = 10)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var currentUser = User.Identity.GetUserName();
                var user = db.User.FirstOrDefault(u => u.Username.Equals(username));
                var allContents = (from c in db.Content
                    where c.IDauthor == user.IDuser
                    orderby c.TimeChanged descending 
                    select c).Include(c => c.ContentType);

                var model = new IndexContentModel()
                {
                    contents = new PagedList<Content>(allContents, page, pageSize),
                    Username = username
                };

                return View(model);
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> ViewContent(string username, int? page, IndexContentModel model)
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult Insert(string username)
        {
            AddNewContentModel model = new AddNewContentModel();
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var user = db.User.FirstOrDefault(u => u.Username.Equals(username));

                var query1 = (from p in db.Page
                              join c in db.Contributor on p.IDpage equals c.IDpage
                              where c.IDuser == user.IDuser
                    orderby p.name
                    select p).ToList();
                model.Page = new SelectList(query1, "IDpage", "name");

                var query2 = (from p in db.ContentType
                             orderby p.Description
                             select p).ToList();
                model.ContentType = new SelectList(query2, "ID", "Description");

                var query3 = (from c in db.City
                              orderby c.CityName
                              select c).ToList();
                model.Location = new SelectList(query3, "IDcity", "CityName");
                model.Username = username;
            }
            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Insert(string username, AddNewContentModel model)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var user = db.User.FirstOrDefault(u => u.Username.Equals(username));
                var newContent = db.Content.Create();
                if (!Request["ContentTypeDropDown"].Any())
                {
                    ModelState.AddModelError("", "Incorrect content.");
                    return RedirectToAction("Insert", new { Username = username });
                }
                var contSel = Request["ContentTypeDropDown"];
                newContent.IDcontentType = Convert.ToInt32(contSel);
                newContent.Text = model.Text;
                newContent.IDauthor = user.IDuser;
                if (model.Title != null) newContent.Title = model.Title;
                else newContent.Title = "(no title)";
                newContent.IDeditor = user.IDuser;
                newContent.TimeChanged = DateTime.Now;

                db.Content.Add(newContent);


                if (Request["PageDropDown"].Any())
                {
                    var content = db.ContentPage.Create();
                    var pageSel = Request["PageDropDown"];
                    content.IDuser = user.IDuser;
                    content.IDpage = Convert.ToInt32(pageSel);
                    content.IDcontent = newContent.IDcontent;
                    db.ContentPage.Add(content);
                    db.SaveChanges();
                }

                if (Request["LocationInsert"].Any())
                {
                    var location = db.LocationContent.Create();
                    var locationSelect = Request["LocationInsert"];
                    location.IDlocation = Convert.ToInt32(locationSelect);
                    location.IDcontent = newContent.IDcontent;
                    location.TimeChanged = DateTime.Now;
                    db.LocationContent.Add(location);
                    db.SaveChanges();
                }

                db.SaveChanges();
                return RedirectToAction("ViewContent", new { Username = username });
            }
            return View(model);
        }

        public class RequiresParameterAttribute : ActionMethodSelectorAttribute
        {

            readonly string parameterName;

            public RequiresParameterAttribute(string parameterName)
            {
                this.parameterName = parameterName;
            }

            public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
            {
                return controllerContext.RouteData.Values[parameterName] != null;
            }
        }

    }
}