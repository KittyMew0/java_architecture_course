// Pet/Responses/PetResponse.cs
namespace ClinicService.Models.Responses
{
    public class PetResponse
    {
        public int PetId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
    }
}