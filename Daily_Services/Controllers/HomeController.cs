using Daily_Services.Models;
using DailyServices.Mailer;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
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
            return RedirectToAction("Login", "Account");
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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContactUs(Contact model)
        {

            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0}</p><p> Phone_Number: {1}</p><p>User_Email: {2}</p><p>Message: {3}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("naveedakram874@gmail.com"));  // replace with valid Gmail 
                message.From = new MailAddress("naveedakram874@outlook.com");  // replace with valid OutLook
                message.Subject = "FeedBack";
                message.Body = string.Format(body, model.Name, model.Phone_Number, model.Email, model.Message);
                message.IsBodyHtml = true;


                using (var smtp = new SmtpClient())
                {

                    var credential = new NetworkCredential
                    {
                        UserName = "naveedakram874@outlook.com",  // replace with valid value
                        Password = "Naveedakram123456"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp-mail.outlook.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Contact");

                }

            }
            ViewBag.Message = "Message Send Succesfully";

            return View(model);
        }


        //[HttpPost]
        //public ActionResult ContactUs(ContactUs contact)
        //{
        //    UserMailer.Welcome(contact).Send();
        //    ContactUs c = new ContactUs()
        //    {
        //        Email = contact.Email,
        //        Name = contact.Name,
        //        Message = contact.Message,
        //        PhoneNumber = contact.PhoneNumber
        //    };
        //    //feedback.IsActive = true;
        //    //feedback.IsDelete = false;
        //    //feedback.ShowOnWebsite = true;
        //    db.ContactUs.Add(c);
        //    db.SaveChanges();
        //    return View();
        //}


    }
}
