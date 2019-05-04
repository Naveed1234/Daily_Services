using Daily_Services.Models;
using System.Linq;
using System.Web.Mvc;

namespace Daily_Services.Controllers
{
    public class AllUserListController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: AllUserList
        public ActionResult ServiceProvider()
        {
            var list = db.Users.Where(x => x.ServiceOffer == true && x.IsActive == true && x.IsDeleted == false).ToList();

            return View(list);
        }


        public ActionResult CutomerList()
        {
            var list = db.Users.Where(x => x.ServiceOffer == false && x.IsActive == true && x.IsDeleted == false).ToList();

            return View(list);
        }
    }
}
