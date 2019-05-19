using Daily_Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Daily_Services.Controllers
{
    public class ServiceProviderController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: ServiceProvider
        public ActionResult Academic()
        {
            ViewBag.Category_Id = new SelectList(db.Categories, "Category_Id", "Category_Type");
            ViewBag.Gender_Id = new SelectList(db.Genders, "Gender_Id", "Gender_Type");
            ViewBag.Qualification_Id = new SelectList(db.Qualifications, "Qualification_Id", "Qualification_Type");
            List<ApplicationUser> registrations = db.Users.Where(r => r.Category_Id == 2).ToList();
            ViewBag.AcadmicList = registrations;

            return View();
        }


        public ActionResult AcademicDetail(string id)
        {
          
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          ApplicationUser registration = db.Users.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }
    }
}