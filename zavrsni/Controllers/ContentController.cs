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

namespace zavrsni.Controllers
{
    public class ContentController : Controller
    {
        public ActionResult Edit(int IDcontent)
        {
            Contents model = new Contents();
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var query = db.Content.FirstOrDefault(u => u.IDcontent.Equals(IDcontent));
                var queryLocation = db.LocationContent.FirstOrDefault(l => l.IDcontent.Equals(query.IDcontent));

                var query1 = (from p in db.Page
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
                    select p).ToList();

                model.Pages = query4;

                var query5 = (from c in db.Content
                              join d in db.LocationContent on c.IDcontent equals d.IDcontent
                              join l in db.City on d.IDlocation equals l.IDcity
                              where c.IDcontent == IDcontent
                              select l).ToList();

                model.Locations = query5;

                model.Text = query.Text;
                model.Title = query.Title;
            }
            return View(model);

        }

        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> Edit(int IDcontent, Contents model)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var query = db.Content.FirstOrDefault(u => u.IDcontent.Equals(IDcontent));
                //var queryLocation = db.LocationContent.FirstOrDefault(l => l.IDcontent.Equals(IDcontent));
                //var queryPage = db.ContentPage.FirstOrDefault(l => l.IDcontent.Equals(IDcontent));
                if (ModelState.IsValid)
                {
                    string username = User.Identity.GetUserName();
                    var user = db.User.FirstOrDefault(u => u.Username.Equals(username));
                    // Get the userprofile

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
                            return RedirectToAction("Edit", new { IDcontent = IDcontent });
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
                            return RedirectToAction("Edit", new { IDcontent = IDcontent });
                        }

                    }

                    db.Entry(query).State = EntityState.Modified;
                    //db.Entry(queryPage).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Edit", new { IDcontent = IDcontent });
                }
            }
            return View(model);
        }

        public ActionResult Details(int IDcontent)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var cont = db.Content.FirstOrDefault(u => u.IDcontent.Equals(IDcontent));
                var usernameAuthor = db.User.FirstOrDefault(u => u.IDuser.Equals(cont.IDauthor));
                var usernameEditor = (from u in db.User
                    where u.IDuser == cont.IDeditor
                    select u).ToList();
                var contType = db.ContentType.FirstOrDefault(c => c.ID.Equals(cont.IDcontentType));
                try
                {
                    ContentDetails model = new ContentDetails()
                    {
                        IDcontent = cont.IDcontent,
                        Author = usernameAuthor.Username,
                        ContentType = contType.Description,
                        Title = cont.Title,
                        Editor = usernameEditor.FirstOrDefault().Username,
                        TimeChanged = cont.TimeChanged,
                        Text = cont.Text
                    };
                    return View(model);
                }
                catch (NullReferenceException)
                {
                    ContentDetails model = new ContentDetails()
                    {
                        IDcontent = cont.IDcontent,
                        Author = usernameAuthor.Username,
                        Title = cont.Title,
                        ContentType = contType.Description,
                        Text = cont.Text
                    };
                    return View(model);
                }

            }
        }

        [HttpPost, ValidateInput(false), ActionName("Details")]
        public async Task<ActionResult> ShowDetails(int IDcontent)
        {
            return View();
        }

        public ActionResult Delete(int IDcontent)
        {
            return View();
        }

        [HttpPost, ValidateInput(false), ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirm(int IDcontent)
        {

            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var cont = db.Content.FirstOrDefault(u => u.IDcontent.Equals(IDcontent));

                var contentDelete = db.Content.Find(IDcontent);
                db.Content.Remove(contentDelete);
                db.SaveChanges();
            }

            return RedirectToAction("ViewContent", "Content");
        }

        [Authorize]
        [HttpGet]
        public ActionResult ViewContent()
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var currentUser = User.Identity.GetUserName();
                var user = db.User.FirstOrDefault(u => u.Username.Equals(currentUser));
                var allContents = (from c in db.Content
                    where c.IDauthor == user.IDuser
                    orderby c.TimeChanged descending 
                    select c).Include(c => c.ContentType).ToList();

                var model = new IndexContentModel()
                {
                    contents = allContents
                };
                return View(model);
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> ViewContent(IndexContentModel model)
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult Insert()
        {
            AddNewContentModel model = new AddNewContentModel();
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var query1 = (from p in db.Page
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
            }
            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Insert(AddNewContentModel model)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var author = User.Identity.GetUserName();
                var user = db.User.FirstOrDefault(u => u.Username.Equals(author));
                var newContent = db.Content.Create();
                if (!Request["ContentTypeDropDown"].Any())
                {
                    ModelState.AddModelError("", "Incorrect content.");
                    return RedirectToAction("Insert", "Content");
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
                return RedirectToAction("ViewContent", "Content");
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