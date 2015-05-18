using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using zavrsni.Models;

namespace zavrsni.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var allContents = (from c in db.Content
                                   select c).Include(c => c.ContentType).ToList();

                var model = new HomeContentModel()
                {
                    contents = allContents
                };
                return View(model);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}