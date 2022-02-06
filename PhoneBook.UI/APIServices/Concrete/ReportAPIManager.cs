using Newtonsoft.Json;
using PhoneBook.UI.APIServices.Abstract;
using PhoneBook.UI.Models;

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

        public Task CreateAsync(Report report)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Report> GetOneByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Report report)
        {
            throw new NotImplementedException();
        }
    }
}
