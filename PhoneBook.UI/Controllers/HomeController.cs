using Microsoft.AspNetCore.Mvc;
using PhoneBook.UI.APIServices.Abstract;
using PhoneBook.UI.Models;

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

        [HttpPost]
        public async Task<IActionResult> Create(Person person)
        {
            if (person == null)
                return View();

            if(!ModelState.IsValid)
            {
                return View(person);
            }

            person.Id = Guid.NewGuid().ToString();

            foreach (var contactInfo in person.ContactInfo)
            {
                contactInfo.PersonId = person.Id;
            }

            await _personAPIService.CreateAsync(person);

            return RedirectToAction("Index");
        }

    }
}
