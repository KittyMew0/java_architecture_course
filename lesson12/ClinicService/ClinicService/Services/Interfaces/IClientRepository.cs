using ClinicService.Models;

namespace ClinicService.Services.Interfaces
{
    public interface IClientRepository : IRepository<Client, int>
    {
        Client? GetByDocument(string document);
        List<Client> GetBySurName(string surName);
        List<Client> GetBySurNameContains(string searchText);
        List<Client> GetAllWithPets();
        bool Exists(int clientId);
        List<Client> GetByBirthdayRange(DateTime from, DateTime to);
    }
}