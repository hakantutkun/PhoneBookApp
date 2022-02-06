using Newtonsoft.Json;
using PhoneBook.UI.APIServices.Abstract;
using PhoneBook.UI.Models;
using System.Text;

namespace PhoneBook.UI.APIServices.Concrete
{
    public class ReportAPIManager : IReportAPIService
    {
        #region Members

        /// <summary>
        /// HttpClient Object
        /// </summary>
        private readonly HttpClient _httpClient;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClient">HttpClient DI object</param>
        public ReportAPIManager(HttpClient httpClient)
        {
            // Inject httpclient.
            _httpClient = httpClient;

            // Set base address of the client.
            _httpClient.BaseAddress = new Uri("http://localhost:5088/Report/");
        }

        #endregion

        /// <summary>
        /// Gets all reports from database.
        /// </summary>
        public async Task<List<Report>> GetAllAsync()
        {
            // Send get request.
            var responseMessage = await _httpClient.GetAsync("");

            // Check if response message status code is success
            if (responseMessage.IsSuccessStatusCode)
            {
                // return the list of person
                return JsonConvert.DeserializeObject<List<Report>>(await responseMessage.Content.ReadAsStringAsync());
            }

            // If failed, return null.
            return null;
        }

        /// <summary>
        /// Creates a new report
        /// </summary>
        /// <param name="report">Received report object</param>
        /// <returns></returns>
        public async Task CreateAsync(Report report)
        {
            // Serialize received object to string.
            var jsonData = JsonConvert.SerializeObject(report);

            // Create a string content with json data.
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Post the content to the contact service.
            await _httpClient.PostAsync("", content);
        }

        /// <summary>
        /// Deletes a report
        /// </summary>
        /// <param name="id">The id of the report that will be deleted.</param>
        /// <returns></returns>
        public async Task DeleteAsync(string id)
        {
            // Send delete request to contact service.
            await _httpClient.DeleteAsync(id);
        }

        /// <summary>
        /// Gets the requested report.
        /// </summary>
        /// <param name="id">The id of the requested report.</param>
        /// <returns></returns>
        public async Task<Report> GetOneByIdAsync(string id)
        {
            // Send request with id.
            var responseMessage = await _httpClient.GetAsync(id);

            // Deserialize received string to the person object and return it.
            return JsonConvert.DeserializeObject<Report>(await responseMessage.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Updates the requested report.
        /// </summary>
        /// <param name="report">Requested report.</param>
        /// <returns></returns>
        public Task UpdateAsync(Report report)
        {
            throw new NotImplementedException();
        }
    }
}
