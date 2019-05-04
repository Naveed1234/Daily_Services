using Daily_Services.Models;
using System.Linq;
using System.Web.Mvc;

namespace Daily_Services.Controllers
{
    public class AcademicController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Academic
        public ActionResult AcademicSubCategories()
        {
            var list = db.GetSubCategories.Where(x => x.Category_Id == 2).ToList();
            return View(list);
        }
    }
}
