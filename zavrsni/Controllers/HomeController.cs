using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
                                   orderby c.TimeChanged descending 
                                   select c).Include(c => c.ContentType).ToList();

                var model = new HomeContentModel()
                {
                    contents = allContents
                };
                return View(model);
            }
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

                var model = new CityCountryListModel()
                {
                    Cities = allCities,
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
                if (model.CountryName != null && model.CityName == null && Request["CountryDropDown"] == null)
                {
                    var newCountry = db.Country.Create();
                    newCountry.CountryName = model.CountryName;

                    db.Country.Add(newCountry);
                    db.SaveChanges();
                    return RedirectToAction("Cities", "Home");
                }

                if (model.CountryName == null && model.CityName != null && Request["CountryDropDown"] != null)
                {
                    var newCity = db.City.Create();
                    newCity.CityName = model.CityName;
                    var countrySel = Request["CountryDropDown"];
                    newCity.IDcountry = Convert.ToInt32(countrySel);

                    db.City.Add(newCity);
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