using ClinicService.Models;
using ClinicService.Services.Interfaces;
using Microsoft.Data.Sqlite;

namespace ClinicService.Services.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private const string connectionString = "Data Source=clinic.db;";

        public int Create(Client item)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO clients (Document, SurName, FirstName, Patronymic, Birthday) VALUES (@Document, @SurName, @FirstName, @Patronymic, @Birthday)";
                command.Parameters.AddWithValue("@Document", item.Document);
                command.Parameters.AddWithValue("@SurName", item.SurName);
                command.Parameters.AddWithValue("@FirstName", item.FirstName);
                command.Parameters.AddWithValue("@Patronymic", item.Patronymic ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Birthday", item.Birthday.Ticks);
                command.Prepare();
                return command.ExecuteNonQuery();
            }
        }

        public int Update(Client item)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE clients SET Document = @Document, SurName = @SurName, FirstName = @FirstName, Patronymic = @Patronymic, Birthday = @Birthday WHERE ClientId = @ClientId";
                command.Parameters.AddWithValue("@ClientId", item.ClientId);
                command.Parameters.AddWithValue("@Document", item.Document);
                command.Parameters.AddWithValue("@SurName", item.SurName);
                command.Parameters.AddWithValue("@FirstName", item.FirstName);
                command.Parameters.AddWithValue("@Patronymic", item.Patronymic ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Birthday", item.Birthday.Ticks);
                command.Prepare();
                return command.ExecuteNonQuery();
            }
        }

        public int Delete(int id)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM clients WHERE ClientId = @ClientId";
                command.Parameters.AddWithValue("@ClientId", id);
                command.Prepare();
                return command.ExecuteNonQuery();
            }
        }

        public Client? GetById(int id)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT ClientId, Document, SurName, FirstName, Patronymic, Birthday FROM clients WHERE ClientId = @ClientId";
                command.Parameters.AddWithValue("@ClientId", id);
                command.Prepare();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Client
                        {
                            ClientId = reader.GetInt32(0),
                            Document = reader.GetString(1),
                            SurName = reader.GetString(2),
                            FirstName = reader.GetString(3),
                            Patronymic = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Birthday = new DateTime(reader.GetInt64(5))
                        };
                    }
                    return null;
                }
            }
        }

        public List<Client> GetAll()
        {
            List<Client> list = new List<Client>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT ClientId, Document, SurName, FirstName, Patronymic, Birthday FROM clients";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Client client = new Client
                        {
                            ClientId = reader.GetInt32(0),
                            Document = reader.GetString(1),
                            SurName = reader.GetString(2),
                            FirstName = reader.GetString(3),
                            Patronymic = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Birthday = new DateTime(reader.GetInt64(5))
                        };
                        list.Add(client);
                    }
                }
            }
            return list;
        }

        public Client? GetByDocument(string document)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT ClientId, Document, SurName, FirstName, Patronymic, Birthday FROM clients WHERE Document = @Document";
                command.Parameters.AddWithValue("@Document", document);
                command.Prepare();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Client
                        {
                            ClientId = reader.GetInt32(0),
                            Document = reader.GetString(1),
                            SurName = reader.GetString(2),
                            FirstName = reader.GetString(3),
                            Patronymic = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Birthday = new DateTime(reader.GetInt64(5))
                        };
                    }
                    return null;
                }
            }
        }

        public List<Client> GetBySurName(string surName)
        {
            List<Client> list = new List<Client>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT ClientId, Document, SurName, FirstName, Patronymic, Birthday FROM clients WHERE SurName = @SurName";
                command.Parameters.AddWithValue("@SurName", surName);
                command.Prepare();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Client
                        {
                            ClientId = reader.GetInt32(0),
                            Document = reader.GetString(1),
                            SurName = reader.GetString(2),
                            FirstName = reader.GetString(3),
                            Patronymic = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Birthday = new DateTime(reader.GetInt64(5))
                        });
                    }
                }
            }
            return list;
        }

        public List<Client> GetBySurNameContains(string searchText)
        {
            List<Client> list = new List<Client>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT ClientId, Document, SurName, FirstName, Patronymic, Birthday FROM clients WHERE SurName LIKE @SearchPattern";
                command.Parameters.AddWithValue("@SearchPattern", $"%{searchText}%");
                command.Prepare();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Client
                        {
                            ClientId = reader.GetInt32(0),
                            Document = reader.GetString(1),
                            SurName = reader.GetString(2),
                            FirstName = reader.GetString(3),
                            Patronymic = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Birthday = new DateTime(reader.GetInt64(5))
                        });
                    }
                }
            }
            return list;
        }

        public List<Client> GetAllWithPets()
        {
            var clients = new Dictionary<int, Client>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT c.ClientId, c.Document, c.SurName, c.FirstName, c.Patronymic, c.Birthday,
                           p.PetId, p.Name, p.Birthday as PetBirthday
                    FROM clients c
                    LEFT JOIN pets p ON c.ClientId = p.ClientId
                    ORDER BY c.ClientId";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int clientId = reader.GetInt32(0);

                        if (!clients.ContainsKey(clientId))
                        {
                            clients[clientId] = new Client
                            {
                                ClientId = clientId,
                                Document = reader.GetString(1),
                                SurName = reader.GetString(2),
                                FirstName = reader.GetString(3),
                                Patronymic = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Birthday = new DateTime(reader.GetInt64(5))
                            };
                        }

                    }
                }
            }

            return clients.Values.ToList();
        }

        public bool Exists(int clientId)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM clients WHERE ClientId = @ClientId";
                command.Parameters.AddWithValue("@ClientId", clientId);
                command.Prepare();

                long count = (long)command.ExecuteScalar();
                return count > 0;
            }
        }

        public List<Client> GetByBirthdayRange(DateTime from, DateTime to)
        {
            List<Client> list = new List<Client>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT ClientId, Document, SurName, FirstName, Patronymic, Birthday FROM clients WHERE Birthday BETWEEN @FromDate AND @ToDate";
                command.Parameters.AddWithValue("@FromDate", from.Ticks);
                command.Parameters.AddWithValue("@ToDate", to.Ticks);
                command.Prepare();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Client
                        {
                            ClientId = reader.GetInt32(0),
                            Document = reader.GetString(1),
                            SurName = reader.GetString(2),
                            FirstName = reader.GetString(3),
                            Patronymic = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Birthday = new DateTime(reader.GetInt64(5))
                        });
                    }
                }
            }
            return list;
        }
    }
}
