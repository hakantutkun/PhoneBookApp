using Microsoft.AspNetCore.Mvc;

namespace ReportService.API.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
