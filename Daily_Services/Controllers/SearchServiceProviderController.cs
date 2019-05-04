using Daily_Services.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Daily_Services.Controllers
{
    public class SearchServiceProviderController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: SearchServiceProvider
        public ActionResult Search()
        {
            ViewBag.Category_Id = new SelectList(db.Categories, "Category_Id", "Category_Type");

            return View();
        }

        public ActionResult GetId(int? id)
        {
            //Daily_ServicesEntities3 db = new Daily_ServicesEntities3();
            ApplicationDbContext db = new ApplicationDbContext();
            ViewBag.Data = db.Users.Where(x => x.SubCategory.SubCategory_Id == id).Include(x => x.SubCategory).ToList();
            return PartialView("_ShowServiceProviderList");
        }
    }
}
