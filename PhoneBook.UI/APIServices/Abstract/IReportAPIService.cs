using PhoneBook.UI.Models;

namespace PhoneBook.UI.APIServices.Abstract
{
    public interface IReportAPIService
    {
        Task<List<Report>> GetAllAsync();

        Task<Report> GetOneByIdAsync(string id);

        Task CreateAsync(Report report);

        Task UpdateAsync(Report report);

        Task DeleteAsync(string id);
    }
}
