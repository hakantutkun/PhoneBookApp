using Microsoft.AspNetCore.Mvc;
using PhoneBook.UI.APIServices.Abstract;
using PhoneBook.UI.Models;

namespace PhoneBook.UI.Controllers
{
    /// <summary>
    /// Home Controller
    /// </summary>
    public class HomeController : Controller
    {
        #region Members

        /// <summary>
        /// API Service Object
        /// </summary>
        private readonly IPersonAPIService _personAPIService;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="personAPIService"></param>
        public HomeController(IPersonAPIService personAPIService)
        {
            _personAPIService = personAPIService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Index Get Method
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                // Get all from api service.
                var result = await _personAPIService.GetAllAsync();

                // Return the result
                return View(result);
            }
            catch (Exception)
            {
                // If any error occured, return empty view.
                return View();
            }
        }

        /// <summary>
        /// Deletes requested person
        /// </summary>
        /// <param name="id">requested person id</param>
        public async Task<IActionResult> Delete(string id)
        {
            // Send delete request to api service. 
            await _personAPIService.DeleteAsync(id);

            // Redirect back to the index page.
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Create Get Method
        /// </summary>
        public async Task<IActionResult> Create()
        {
            return View();
        }

        /// <summary>
        /// Create Post Method
        /// Creates a new person.
        /// </summary>
        /// <param name="person">Received person model</param>
        [HttpPost]
        public async Task<IActionResult> Create(Person person)
        {
            // Check if received model is null
            if (person == null)
                return View();

            // Check if received model is valid
            if (!ModelState.IsValid)
            {
                return View(person);
            }

            // Set a new guid to person
            person.Id = Guid.NewGuid().ToString();

            // Set contact infos the same guid 
            foreach (var contactInfo in person.ContactInfo)
            {
                contactInfo.PersonId = person.Id;
            }

            // Send create request to api service.
            await _personAPIService.CreateAsync(person);

            // Redirect back to the index page.
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Detail page get method
        /// </summary>
        public async Task<IActionResult> Detail()
        {
            return View();
        }

        /// <summary>
        /// Gets detail of requested person
        /// </summary>
        /// <param name="id">Id of the requested person</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(string id)
        {
            // Get requested person from api service.
            var person = await _personAPIService.GetOneByIdAsync(id);

            // Return person model to view.
            return View(person);
        }

        /// <summary>
        /// Detail page get method
        /// </summary>
        public async Task<IActionResult> CreateInfo(string id)
        {
            ViewData["personId"] = id;
            return View();
        }

        /// <summary>
        /// Creates contact info
        /// </summary>
        /// <param name="contactInfo">received contact info</param>
        [HttpPost]
        public async Task<IActionResult> CreateInfo(ContactInfo contactInfo)
        {
            // send create request to api service.
            await _personAPIService.CreateInfoAsync(contactInfo);

            // Redirect back to the Person Detail page.
            return RedirectToAction("Detail", new { id = contactInfo.PersonId });
        }

        /// <summary>
        /// Deletes requested contact info 
        /// </summary>
        /// <param name="id">The id of the requested contact info</param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteInfo(int id)
        {
            // Get info from the api service
            var info = await _personAPIService.GetOneInfoByIdAsync(id);

            // Send delete request to api service.
            await _personAPIService.DeleteInfoAsync(id);

            // Redirect back to the Person Detail page.
            return RedirectToAction("Detail", new { id = info.PersonId });
        }

        #endregion
    }
}
