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

        /// <summary>
        /// Default Index Action
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Create Get Method
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Create()
        {
            return View();
        }

        /// <summary>
        /// Creates new report
        /// </summary>
        /// <param name="report">Received report object.</param>
        [HttpPost]
        public async Task<IActionResult> Create(Report report)
        {
            // Set the id of the report
            report.Id = Guid.NewGuid().ToString();

            // Set Creation time
            report.CreationTime = DateTime.Now;

            // Set report state as preparing.
            report.ReportState = ReportState.Preparing;

            // Send create request to report service.
            await _reportAPIService.CreateAsync(report);

            // Redirect back to the index page.
            return RedirectToAction("Index","Report");
        }

        /// <summary>
        /// Report Detail Action
        /// </summary>
        /// <param name="id">The id of the requested report</param>
        /// <returns></returns>
        public async Task<IActionResult> Detail(string id)
        {
            // Get all from api service.
            var report = await _reportAPIService.GetOneByIdAsync(id);

            // Return report.
            return View(report);
        }

        /// <summary>
        /// Deletes requested person
        /// </summary>
        /// <param name="id">requested person id</param>
        public async Task<IActionResult> Delete(string id)
        {
            // Send delete request to api service. 
            await _reportAPIService.DeleteAsync(id);

            // Redirect back to the index page.
            return RedirectToAction("Index");
        }
    }
}
