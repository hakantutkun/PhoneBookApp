using Microsoft.AspNetCore.Mvc;
using PhoneBook.UI.APIServices.Abstract;
using PhoneBook.UI.Models;

namespace PhoneBook.UI.Controllers
{
    /// <summary>
    /// Report Controller
    /// </summary>
    public class ReportController : Controller
    {
        #region Members

        /// <summary>
        /// API Service Object
        /// </summary>
        private readonly IReportAPIService _reportAPIService;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reportAPIService"></param>
        public ReportController(IReportAPIService reportAPIService)
        {
            _reportAPIService = reportAPIService;
        }

        #endregion

        public async Task<IActionResult> Index()
        {
            try
            {
                // Get all from api service.
                var result = await _reportAPIService.GetAllAsync();

                // Return the result
                return View(result);
            }
            catch (Exception)
            {
                // If any error occured, return empty view.
                return View();
            }
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Report report)
        {
            report.Id = Guid.NewGuid().ToString();

            report.CreationTime = DateTime.Now;

            report.ReportState = ReportState.Preparing;

            await _reportAPIService.CreateAsync(report);

            // Redirect back to the index page.
            return RedirectToAction("Index","Report");
        }
    }
}
