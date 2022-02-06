using Microsoft.AspNetCore.Mvc;

namespace PhoneBook.UI.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
