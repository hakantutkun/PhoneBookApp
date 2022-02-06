using PhoneBook.UI.Models;

namespace PhoneBook.UI.APIServices.Abstract
{
    public interface IPersonAPIService
    {
        Task<List<Person>> GetAllAsync();

        Task<Person> GetOneByIdAsync(string id);

        Task CreateAsync(Person person);

        Task UpdateAsync(Person person);

        Task DeleteAsync(string id);
    }
}
