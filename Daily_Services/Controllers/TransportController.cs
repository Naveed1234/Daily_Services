using Daily_Services.Models;
using System.Linq;
using System.Web.Mvc;

namespace Daily_Services.Controllers
{
    public class TransportController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Transport
        public ActionResult LaborSubCategories()
        {
            var list = db.GetSubCategories.Where(x => x.Category_Id == 4).ToList();
            return View(list);
        }
    }
}
