using Newtonsoft.Json;
using PhoneBook.UI.APIServices.Abstract;
using PhoneBook.UI.Models;
using System.Text;

namespace PhoneBook.UI.APIServices.Concrete
{
    /// <summary>
    /// Base API Manager Class
    /// </summary>
    public class PersonAPIManager : IPersonAPIService
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
        public PersonAPIManager(HttpClient httpClient)
        {
            // Inject httpclient.
            _httpClient = httpClient;

            // Set base address of the client.
            _httpClient.BaseAddress = new Uri("http://localhost:5038/Person/");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all person from database.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Person>> GetAllAsync()
        {
            // Send get request.
            var responseMessage = await _httpClient.GetAsync("");

            // Check if response message status code is success
            if (responseMessage.IsSuccessStatusCode)
            {
                // return the list of person
                return JsonConvert.DeserializeObject<List<Person>>(await responseMessage.Content.ReadAsStringAsync());
            }

            // If failed, return null.
            return null;
        }

        /// <summary>
        /// Creates a new person.
        /// </summary>
        /// <param name="person">Received person object</param>
        public async Task CreateAsync(Person person)
        {
            // Serialize received object to string.
            var jsonData = JsonConvert.SerializeObject(person);

            // Create a string content with json data.
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Post the content to the contact service.
            await _httpClient.PostAsync("", content);
        }

        /// <summary>
        /// Deletes a person.
        /// </summary>
        /// <param name="id">Id of requested person.</param>
        public async Task DeleteAsync(string id)
        {
            // Send delete request to contact service.
            await _httpClient.DeleteAsync(id);
        }

        /// <summary>
        /// Gets one person.
        /// </summary>
        /// <param name="id">Id of the requested person</param>
        /// <returns></returns>
        public async Task<Person> GetOneByIdAsync(string id)
        {
            // Send request with id.
            var responseMessage = await _httpClient.GetAsync(id);

            // Deserialize received string to the person object and return it.
            return JsonConvert.DeserializeObject<Person>(await responseMessage.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Updates requested person.
        /// </summary>
        /// <param name="person">Requested person object</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task UpdateAsync(Person person)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Creates a contact info
        /// </summary>
        /// <param name="contactInfo">received contact info object</param>
        /// <returns></returns>
        public async Task CreateInfoAsync(ContactInfo contactInfo)
        {
            // Serialize received object to string.
            var jsonData = JsonConvert.SerializeObject(contactInfo);

            // Create a string content with json data.
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Post the content to the contact service.
            await _httpClient.PostAsync("Info/", content);
        }


        /// <summary>
        /// Deletes Contact Info
        /// </summary>
        /// <param name="infoId">The contact info id</param>
        /// <returns></returns>
        public async Task DeleteInfoAsync(int infoId)
        {
            // Send delete request to the contact service.
            await _httpClient.DeleteAsync("Info/" + infoId.ToString());
        }

        /// <summary>
        /// Gets one contact info.
        /// </summary>
        /// <param name="id">Requested info id.</param>
        /// <returns></returns>
        public async Task<ContactInfo> GetOneInfoByIdAsync(int id)
        {
            // Send request with given id.
            var responseMessage = await _httpClient.GetAsync("Info/" + id.ToString());

            // Deserialize received string to the contact info object and return it.
            return JsonConvert.DeserializeObject<ContactInfo>(await responseMessage.Content.ReadAsStringAsync());
        }

        #endregion
    }
}
