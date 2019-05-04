using Daily_Services.Models;
using System.Linq;
using System.Web.Mvc;

namespace Daily_Services.Controllers
{
    public class MedicalController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Medical
        public ActionResult MedicalSubCategories()
        {
            var list = db.GetSubCategories.Where(x => x.Category_Id == 3).ToList();
            return View(list);
        }
    }
}
