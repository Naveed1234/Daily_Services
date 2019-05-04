using Daily_Services.Models;
using System.Linq;
using System.Web.Mvc;

namespace Daily_Services.Controllers
{
    public class LaborController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Labor
        public ActionResult LaborSubCategories()
        {
            var list = db.GetSubCategories.Where(x => x.Category_Id == 1).ToList();
            return View(list);
        }
    }
}
