using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using zavrsni.Models;
using PagedList;

namespace zavrsni.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int page = 1, int pageSize = 20)
        {
            HomeContentModel model = new HomeContentModel();
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                if (Request.IsAuthenticated)
                {
                    var currentUser = User.Identity.GetUserName();
                    var user = db.User.FirstOrDefault(u => u.Username.Equals(currentUser));

                    var groupContents = (from g in db.Group
                                         join b in db.BelongsToGroup on g.IDgroup equals b.IDgroup
                                         join u in db.User on b.IDuser equals u.IDuser
                                         join c in db.Content on u.IDuser equals c.IDauthor
                                         where g.IDgroupOwner == user.IDuser
                                         && g.IDgroup != 1
                                         && g.IDgroup == b.IDgroup
                                         && c.IsCopied == false
                                         orderby c.TimeChanged descending
                                         select c).Include(c => c.User);
                    model.contents = new PagedList<Content>(groupContents, page, pageSize);
                }

                var allContents = (from c in db.Content
                                   orderby c.TimeChanged descending 
                                   where c.IsCopied == false
                                   select c).Include(c => c.ContentType).Include(c => c.User);

                var mostViewedPages = (from p in db.Page
                                       where p.IDprivacy == 3
                                       orderby p.PageView ascending 
                                       select p).Take(3).ToList();

                model.contentsGuest = new PagedList<Content>(allContents, page, pageSize);
                model.topPages = mostViewedPages;
                return View(model);
            }
        }

        [HttpPost, ActionName("Index")]
        public async Task<ActionResult> Index(HomeContentModel model, int page = 1, int pageSize = 20)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                return RedirectToAction("Search", new { keyword = model.keyword, page = page, pageSize = pageSize });
            }
        }

        public ActionResult Search(string keyword, int page = 1, int pageSize = 20)
        {
            SearchModel model = new SearchModel();

            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var searchResults = (from c in db.Content
                                     where c.Text.ToLower().Contains(keyword.ToLower())
                                           || c.Title.ToLower().Contains(keyword.ToLower())
                                     orderby c.TimeChanged descending
                                     select c);
                model.results = new PagedList<Content>(searchResults, page, pageSize);
            }
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Cities()
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                var allCities = (from c in db.City
                                   orderby c.CityName
                                   select c).Include(c => c.Country).ToList();

                var availableCountries = (from c in db.Country
                    orderby c.CountryName
                    select c).ToList();

                var locTypes = (from l in db.LocationType
                    orderby l.Name
                    select l).ToList();

                var model = new CityCountryListModel()
                {
                    Cities = allCities,
                    LocationType = new SelectList(locTypes, "ID", "Name")
                };
                model.Country = new SelectList(availableCountries, "ID", "CountryName");
                return View(model);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Cities(CityCountryListModel model)
        {
            using (ZavrsniEFentities db = new ZavrsniEFentities())
            {
                if (model.CountryName != null && model.CityName == null && Request["CountryDropDown"] == null && Request["LocationTypeDropDown"] == null)
                {
                    var newCountry = db.Country.Create();
                    newCountry.CountryName = model.CountryName;

                    db.Country.Add(newCountry);
                    db.SaveChanges();
                    return RedirectToAction("Cities", "Home");
                }

                if (model.CountryName == null && model.CityName != null && Request["LocationTypeDropDown"] != null && Request["CountryDropDown"] != null)
                {
                    var newCity = db.City.Create();
                    newCity.CityName = model.CityName;
                    var countrySel = Request["CountryDropDown"];
                    newCity.IDcountry = Convert.ToInt32(countrySel);

                    db.City.Add(newCity);

                    db.SaveChanges();
                    var newLocation = db.Location.Create();
                    newLocation.IDcity = newCity.IDcity;
                    var locTypeSel = Request["LocationTypeDropDown"];
                    newLocation.IDlocationType = Convert.ToInt32(locTypeSel);
                    db.Location.Add(newLocation);
                    db.SaveChanges();

                    return RedirectToAction("Cities", "Home");
                }
            }
            return RedirectToAction("Cities", "Home");
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