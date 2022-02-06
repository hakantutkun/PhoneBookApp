using Newtonsoft.Json;
using PhoneBook.UI.APIServices.Abstract;
using PhoneBook.UI.Models;
using System.Text;

namespace PhoneBook.UI.APIServices.Concrete
{
    public class PersonAPIManager : IPersonAPIService
    {
        private readonly HttpClient _httpClient;

        public PersonAPIManager(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri("http://localhost:5038/Person/");
        }

        public async Task<List<Person>> GetAllAsync()
        {
            var responseMessage = await _httpClient.GetAsync("");
            if (responseMessage.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<Person>>(await responseMessage.Content.ReadAsStringAsync());
            }
            return null;
        }

        public async Task CreateAsync(Person person)
        {
            var jsonData = JsonConvert.SerializeObject(person);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            await _httpClient.PostAsync("", content);
        }

        public async Task DeleteAsync(string id)
        {
            await _httpClient.DeleteAsync(id);
        }

        public Task<Person> GetOneByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Person person)
        {
            throw new NotImplementedException();
        }
    }
}
