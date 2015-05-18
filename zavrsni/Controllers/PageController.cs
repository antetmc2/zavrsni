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
using System.Web.Helpers;

namespace zavrsni.Controllers
{
    public class PageController : Controller
    {
        // GET: Page
        public ActionResult Index()
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var currentUser = User.Identity.GetUserName();
                var user = db.User.FirstOrDefault(u => u.Username.Equals(currentUser));
                var allPages = (from p in db.Page
                    join c in db.Contributor
                        on p.IDpage equals c.IDpage
                        where c.IDuser == user.IDuser
                        && c.IsAuthor
                    select p).ToList();

                var model = new IndexPageModel()
                {
                    pages = allPages
                };
                return View(model);
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult NewPage()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> NewPage(NewPageModel model)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var author = User.Identity.GetUserName();
                var user = db.User.FirstOrDefault(u => u.Username.Equals(author));
                var newPage = db.Page.Create();
                newPage.name = model.PageTitle;
                newPage.IDprivacy = 1;
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

                return RedirectToAction("Index", "Page");


            }
        }
    }
}