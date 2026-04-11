using ClinicService.Models;

namespace ClinicService.Services.Interfaces
{
    public interface IPetRepository : IRepository<Pet, int>
    {
        List<Pet> GetByClientId(int clientId);
        List<Pet> GetByName(string name);
        List<Pet> GetByNameContains(string searchText);
        List<Pet> GetByAgeRange(int minAgeYears, int maxAgeYears);
        List<Pet> GetAllWithOwner();
        int CountByClientId(int clientId);
        List<Pet> GetYoungestPets(int count);
    }
}