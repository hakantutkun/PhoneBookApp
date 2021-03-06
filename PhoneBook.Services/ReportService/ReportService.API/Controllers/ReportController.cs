using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReportService.API.Context;
using ReportService.Core.Models;
using ReportService.Core.Mqtt;

namespace ReportService.API.Controllers
{
    /// <summary>
    /// Report Controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ReportController : Controller
    {
        #region Members

        /// <summary>
        /// Context Object
        /// </summary>
        private readonly ReportServiceContext _context;

        /// <summary>
        /// MqttServer object
        /// </summary>
        private readonly MqttServer _mqttServer;

        // Worker Service Object
        private readonly WorkerService _workerService;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Received context with DI</param>
        public ReportController(ReportServiceContext context, MqttServer mqttServer, WorkerService workerService)
        {
            // Inject received context
            _context = context;
            _mqttServer = mqttServer;
            _workerService = workerService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches all reports from database
        /// </summary>
        [HttpGet]
        public async Task<List<Report>> GetAllAsync()
        {
            // Get all 
            return await _context.Reports.ToListAsync();
        }

        /// <summary>
        /// Fetches only one report from database
        /// </summary>
        /// <param name="id">Requested report's id</param>
        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> GetOneByIdAsync(string id)
        {
            // Check if received id is null
            if (id == null)
            {
                return NotFound();
            }

            // Get requested report from database
            var report = await _context.Reports.FirstOrDefaultAsync(m => m.Id == id);

            // Check if report was found
            if (report == null)
            {
                return NotFound();
            }

            // Return the fetched report
            return Ok(report);
        }

        /// <summary>
        /// Creates a new report.
        /// </summary>
        /// <param name="report">Received report object</param>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Report report)
        {
            // Check if model state is valid
            if (ModelState.IsValid && report.Location != null)
            {
                try
                {
                    // Create a new guid
                    report.Id = Guid.NewGuid().ToString();

                    // Add report to db
                    await _context.Reports.AddAsync(report);

                    // Save Changes
                    await _context.SaveChangesAsync();

                    await _mqttServer.PublishMessageAsync(JsonConvert.SerializeObject(report));

                    // Return created report
                    return Created("", report);
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }
            }

            // If model state is not valid, return bad request.
            return BadRequest();
        }

        /// <summary>
        /// Updates requested report.
        /// </summary>
        /// <param name="report">Changed report object</param>
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] Report report)
        {
            // Check if model state is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Update requested report
                    _context.Update(report);

                    // Save db changes
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Check if report exist in database
                    var p = await _context.Reports.FindAsync(report.Id);

                    if (p == null)
                    {
                        return NotFound();
                    }
                }

                // If updated
                return NoContent();
            }

            // If update process failed
            return BadRequest();
        }

        /// <summary>
        /// Deletes requested report.
        /// </summary>
        /// <param name="id">Requested report id.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            // Find report
            var report = await _context.Reports.FindAsync(id);

            // Check if report found.
            if (report == null)
                return NotFound();

            // remove report
            _context.Reports.Remove(report);

            // Save db changes
            await _context.SaveChangesAsync();

            // Return
            return NoContent();
        }

        #endregion
    }
}
