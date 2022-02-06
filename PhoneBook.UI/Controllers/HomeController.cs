using Microsoft.AspNetCore.Mvc;
using PhoneBook.UI.APIServices.Abstract;

namespace PhoneBook.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPersonAPIService _personAPIService;

        public HomeController(IPersonAPIService personAPIService)
        {
            _personAPIService = personAPIService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var result = await _personAPIService.GetAllAsync();
                return View(result);
            }
            catch (Exception)
            {
                return View();
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _personAPIService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

    }
}
