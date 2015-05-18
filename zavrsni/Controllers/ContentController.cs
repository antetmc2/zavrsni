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

                var query1 = (from p in db.Page
                              orderby p.name
                              select p).ToList();
                model.Page = new SelectList(query1, "IDpage", "name");

                var query2 = (from p in db.ContentType
                              orderby p.Description
                              select p).ToList();
                model.ContentType = new SelectList(query2, "ID", "Description", query.IDcontentType);


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
                if (ModelState.IsValid)
                {
                    string username = User.Identity.GetUserName();
                    var user = db.User.FirstOrDefault(u => u.Username.Equals(username));
                    // Get the userprofile

                    query.Title = model.Title;
                    query.Text = model.Text;
                    query.IDeditor = user.IDuser;
                    query.TimeChanged = DateTime.Now;

                    db.Entry(query).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("ViewContent", "Content"); // or whatever
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
                newContent.Title = model.Title;

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