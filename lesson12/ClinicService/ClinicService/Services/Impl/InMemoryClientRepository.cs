using ClinicService.Models;
using ClinicService.Services.Interfaces;

namespace ClinicService.Services.Impl
{
    public class InMemoryClientRepository : IClientRepository
    {
        private readonly List<Client> _clients = new List<Client>();
        private int _nextId = 1;

        public int Create(Client item)
        {
            item.ClientId = _nextId++;
            _clients.Add(item);
            return 1; 
        }

        public int Update(Client item)
        {
            var existing = _clients.FirstOrDefault(c => c.ClientId == item.ClientId);
            if (existing == null) return 0;

            existing.Document = item.Document;
            existing.SurName = item.SurName;
            existing.FirstName = item.FirstName;
            existing.Patronymic = item.Patronymic;
            existing.Birthday = item.Birthday;

            return 1;
        }

        public int Delete(int id)
        {
            var client = _clients.FirstOrDefault(c => c.ClientId == id);
            if (client == null) return 0;

            return _clients.Remove(client) ? 1 : 0;
        }

        public Client? GetById(int id)
        {
            return _clients.FirstOrDefault(c => c.ClientId == id);
        }

        public List<Client> GetAll()
        {
            return _clients.ToList();
        }
    }
}