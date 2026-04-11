// Consultation/Responses/ConsultationResponse.cs
namespace ClinicService.Models.Responses
{
    public class ConsultationResponse
    {
        public int ConsultationId { get; set; }
        public int ClientId { get; set; }
        public int PetId { get; set; }
        public DateTime ConsultationDate { get; set; }
        public string Description { get; set; }
    }
}