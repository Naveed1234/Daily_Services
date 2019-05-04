using Daily_Services.Models;
using Mvc.Mailer;

namespace DailyServices.Mailer
{
    public interface IUserMailer
    {
        MvcMailMessage Welcome(ContactUs contact);
        MvcMailMessage ResetPassword(string token, string emailTo, string account);

    }
}
