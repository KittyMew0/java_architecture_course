using ClinicService.Models;
using ClinicService.Services.Interfaces;
using Microsoft.Data.Sqlite;

namespace ClinicService.Services.Repositories
{
    public class ConsultationRepository : IConsultationRepository
    {
        private const string connectionString = "Data Source=clinic.db;";

        public int Create(Consultation item)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO consultations (ClientId, PetId, ConsultationDate, Description) VALUES (@ClientId, @PetId, @ConsultationDate, @Description)";
                command.Parameters.AddWithValue("@ClientId", item.ClientId);
                command.Parameters.AddWithValue("@PetId", item.PetId);
                command.Parameters.AddWithValue("@ConsultationDate", item.ConsultationDate.Ticks); 
                command.Parameters.AddWithValue("@Description", item.Description);
                command.Prepare();
                return command.ExecuteNonQuery();
            }
        }

        public int Update(Consultation item)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE consultations SET ClientId = @ClientId, PetId = @PetId, ConsultationDate = @ConsultationDate, Description = @Description WHERE ConsultationId = @ConsultationId";
                command.Parameters.AddWithValue("@ConsultationId", item.ConsultationId);
                command.Parameters.AddWithValue("@ClientId", item.ClientId);
                command.Parameters.AddWithValue("@PetId", item.PetId);
                command.Parameters.AddWithValue("@ConsultationDate", item.ConsultationDate.Ticks);
                command.Parameters.AddWithValue("@Description", item.Description);
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
                command.CommandText = "DELETE FROM consultations WHERE ConsultationId = @ConsultationId";
                command.Parameters.AddWithValue("@ConsultationId", id);
                command.Prepare();
                return command.ExecuteNonQuery();
            }
        }

        public Consultation? GetById(int id)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT ConsultationId, ClientId, PetId, ConsultationDate, Description FROM consultations WHERE ConsultationId = @ConsultationId";
                command.Parameters.AddWithValue("@ConsultationId", id);
                command.Prepare();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Consultation
                        {
                            ConsultationId = reader.GetInt32(0),
                            ClientId = reader.GetInt32(1),
                            PetId = reader.GetInt32(2),
                            ConsultationDate = new DateTime(reader.GetInt64(3)),
                            Description = reader.GetString(4)
                        };
                    }
                    return null;
                }
            }
        }

        public List<Consultation> GetAll()
        {
            List<Consultation> list = new List<Consultation>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT ConsultationId, ClientId, PetId, ConsultationDate, Description FROM consultations";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Consultation consultation = new Consultation
                        {
                            ConsultationId = reader.GetInt32(0),
                            ClientId = reader.GetInt32(1),
                            PetId = reader.GetInt32(2),
                            ConsultationDate = new DateTime(reader.GetInt64(3)),
                            Description = reader.GetString(4)
                        };
                        list.Add(consultation);
                    }
                }
            }
            return list;
        }

        public List<Consultation> GetByClientId(int clientId)
        {
            List<Consultation> list = new List<Consultation>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT ConsultationId, ClientId, PetId, ConsultationDate, Description FROM consultations WHERE ClientId = @ClientId ORDER BY ConsultationDate DESC";
                command.Parameters.AddWithValue("@ClientId", clientId);
                command.Prepare();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Consultation
                        {
                            ConsultationId = reader.GetInt32(0),
                            ClientId = reader.GetInt32(1),
                            PetId = reader.GetInt32(2),
                            ConsultationDate = new DateTime(reader.GetInt64(3)),
                            Description = reader.GetString(4)
                        });
                    }
                }
            }
            return list;
        }

        public List<Consultation> GetByPetId(int petId)
        {
            List<Consultation> list = new List<Consultation>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT ConsultationId, ClientId, PetId, ConsultationDate, Description FROM consultations WHERE PetId = @PetId ORDER BY ConsultationDate DESC";
                command.Parameters.AddWithValue("@PetId", petId);
                command.Prepare();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Consultation
                        {
                            ConsultationId = reader.GetInt32(0),
                            ClientId = reader.GetInt32(1),
                            PetId = reader.GetInt32(2),
                            ConsultationDate = new DateTime(reader.GetInt64(3)),
                            Description = reader.GetString(4)
                        });
                    }
                }
            }
            return list;
        }

        public List<Consultation> GetByDateRange(DateTime from, DateTime to)
        {
            List<Consultation> list = new List<Consultation>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT ConsultationId, ClientId, PetId, ConsultationDate, Description FROM consultations WHERE ConsultationDate BETWEEN @FromDate AND @ToDate ORDER BY ConsultationDate";
                command.Parameters.AddWithValue("@FromDate", from.Ticks);
                command.Parameters.AddWithValue("@ToDate", to.Ticks);
                command.Prepare();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Consultation
                        {
                            ConsultationId = reader.GetInt32(0),
                            ClientId = reader.GetInt32(1),
                            PetId = reader.GetInt32(2),
                            ConsultationDate = new DateTime(reader.GetInt64(3)),
                            Description = reader.GetString(4)
                        });
                    }
                }
            }
            return list;
        }

        public List<Consultation> GetByDate(DateTime date)
        {
            DateTime startOfDay = date.Date;
            DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1);
            return GetByDateRange(startOfDay, endOfDay);
        }

        public List<Consultation> GetUpcomingConsultations()
        {
            List<Consultation> list = new List<Consultation>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT ConsultationId, ClientId, PetId, ConsultationDate, Description FROM consultations WHERE ConsultationDate >= @Now ORDER BY ConsultationDate ASC LIMIT 50";
                command.Parameters.AddWithValue("@Now", DateTime.Now.Ticks);
                command.Prepare();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Consultation
                        {
                            ConsultationId = reader.GetInt32(0),
                            ClientId = reader.GetInt32(1),
                            PetId = reader.GetInt32(2),
                            ConsultationDate = new DateTime(reader.GetInt64(3)),
                            Description = reader.GetString(4)
                        });
                    }
                }
            }
            return list;
        }

        public List<Consultation> GetAllWithDetails()
        {
            List<Consultation> list = new List<Consultation>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT c.ConsultationId, c.ClientId, c.PetId, c.ConsultationDate, c.Description,
                           cl.SurName, cl.FirstName, cl.Patronymic,
                           p.Name as PetName
                    FROM consultations c
                    INNER JOIN clients cl ON c.ClientId = cl.ClientId
                    INNER JOIN pets p ON c.PetId = p.PetId
                    ORDER BY c.ConsultationDate DESC";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var consultation = new Consultation
                        {
                            ConsultationId = reader.GetInt32(0),
                            ClientId = reader.GetInt32(1),
                            PetId = reader.GetInt32(2),
                            ConsultationDate = new DateTime(reader.GetInt64(3)),
                            Description = reader.GetString(4)
                        };
                        list.Add(consultation);
                    }
                }
            }
            return list;
        }

        public List<Consultation> GetByDescriptionContains(string searchText)
        {
            List<Consultation> list = new List<Consultation>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT ConsultationId, ClientId, PetId, ConsultationDate, Description FROM consultations WHERE Description LIKE @SearchPattern ORDER BY ConsultationDate DESC";
                command.Parameters.AddWithValue("@SearchPattern", $"%{searchText}%");
                command.Prepare();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Consultation
                        {
                            ConsultationId = reader.GetInt32(0),
                            ClientId = reader.GetInt32(1),
                            PetId = reader.GetInt32(2),
                            ConsultationDate = new DateTime(reader.GetInt64(3)),
                            Description = reader.GetString(4)
                        });
                    }
                }
            }
            return list;
        }

        public Dictionary<int, int> GetConsultationStatsByClient()
        {
            var stats = new Dictionary<int, int>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT ClientId, COUNT(*) as ConsultationCount 
                    FROM consultations 
                    GROUP BY ClientId 
                    ORDER BY ConsultationCount DESC";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int clientId = reader.GetInt32(0);
                        int count = reader.GetInt32(1);
                        stats[clientId] = count;
                    }
                }
            }
            return stats;
        }

        public bool HasConsultationInPeriod(int petId, DateTime from, DateTime to)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM consultations WHERE PetId = @PetId AND ConsultationDate BETWEEN @FromDate AND @ToDate";
                command.Parameters.AddWithValue("@PetId", petId);
                command.Parameters.AddWithValue("@FromDate", from.Ticks);
                command.Parameters.AddWithValue("@ToDate", to.Ticks);
                command.Prepare();

                long count = (long)command.ExecuteScalar();
                return count > 0;
            }
        }
    }
}