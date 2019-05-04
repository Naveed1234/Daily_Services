using Daily_Services.Models;
using Mvc.Mailer;
using System.Configuration;


namespace DailyServices.Mailer
{
    public class UserMailer : MailerBase, IUserMailer
    {
        public UserMailer()
        {
            MasterName = "_Layout";
        }

        public virtual MvcMailMessage Welcome(ContactUs contact)
        {
            //ViewBag.callbackUrl = callbackUrl;
            return Populate(x =>
            {
                ViewBag.Email = contact.Email;
                ViewBag.Name = contact.Name;
                ViewBag.Number = contact.PhoneNumber;
                ViewBag.Query = contact.Message;
                x.Subject = "Welcome to Daily Services";
                x.ViewName = "ContactUs";
                x.To.Add(contact.Email);
            });
        }
        public MvcMailMessage ResetPassword(string URL, string emailTo, string account)
        {
            ViewBag.callbackurl = URL;
            ViewBag.account = account;
            ViewBag.BASEURL = ConfigurationManager.AppSettings["MvcMailer.BaseURL"];
            return Populate(x =>
            {
                x.Subject = "Password Recovery - Saffron";
                x.ViewName = "ResetPassword";
                x.To.Add(emailTo);
            });
        }
    }
}
