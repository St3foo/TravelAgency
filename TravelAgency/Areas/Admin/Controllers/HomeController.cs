using Microsoft.AspNetCore.Mvc;

namespace TravelAgency.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
