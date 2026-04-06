using ClinicService.Models;

namespace ClinicService.Services.Interfaces
{
    public interface IPetRepository : IRepository<Pet, int>
    {
    }
}
