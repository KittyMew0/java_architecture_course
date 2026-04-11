using ClinicService.Models;

namespace ClinicService.Services.Interfaces
{
    public interface IConsultationRepository : IRepository<Consultation, int>
    {
        List<Consultation> GetByClientId(int clientId);
        List<Consultation> GetByPetId(int petId);
        List<Consultation> GetByDateRange(DateTime from, DateTime to);
        List<Consultation> GetByDate(DateTime date);
        List<Consultation> GetUpcomingConsultations();
        List<Consultation> GetAllWithDetails();
        List<Consultation> GetByDescriptionContains(string searchText);
        Dictionary<int, int> GetConsultationStatsByClient();
        bool HasConsultationInPeriod(int petId, DateTime from, DateTime to);
    }
}