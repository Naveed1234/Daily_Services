using Daily_Services.Models;
using DailyServices.Mailer;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Daily_Services.Controllers
{
    public class HomeController : Controller
    {
        private IUserMailer _userMailer = new UserMailer();
        public IUserMailer UserMailer
        {
            get { return _userMailer; }
            set { _userMailer = value; }
        }

        ApplicationDbContext db = new ApplicationDbContext();
        // List Sub Categoryy
        public JsonResult GetStateList(int Category_Id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<SubCategory> StateList = db.GetSubCategories.Where(x => x.Category_Id == Category_Id).ToList();
            return Json(StateList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Register", "Account");
        }


        public ActionResult Index()
        {
            return View();
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
        [HttpGet]
        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ContactUs(ContactUs contact)
        {
            UserMailer.Welcome(contact).Send();
            ContactUs c = new ContactUs()
            {
                Email = contact.Email,
                Name = contact.Name,
                Message = contact.Message,
                PhoneNumber = contact.PhoneNumber
            };
            //feedback.IsActive = true;
            //feedback.IsDelete = false;
            //feedback.ShowOnWebsite = true;
            db.ContactUs.Add(c);
            db.SaveChanges();
            return View();
        }


    }
}
