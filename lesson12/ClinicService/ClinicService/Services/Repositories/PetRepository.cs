using ClinicService.Models;
using ClinicService.Services.Interfaces;
using Microsoft.Data.Sqlite;

namespace ClinicService.Services.Repositories
{
    public class PetRepository : IPetRepository
    {
        private const string connectionString = "Data Source=clinic.db;";

        public int Create(Pet item)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO pets (ClientId, Name, Birthday) VALUES (@ClientId, @Name, @Birthday)";
                command.Parameters.AddWithValue("@ClientId", item.ClientId);
                command.Parameters.AddWithValue("@Name", item.Name);
                command.Parameters.AddWithValue("@Birthday", item.Birthday.Ticks); 
                command.Prepare();
                return command.ExecuteNonQuery();
            }
        }

        public int Update(Pet item)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE pets SET ClientId = @ClientId, Name = @Name, Birthday = @Birthday WHERE PetId = @PetId";
                command.Parameters.AddWithValue("@PetId", item.PetId);
                command.Parameters.AddWithValue("@ClientId", item.ClientId);
                command.Parameters.AddWithValue("@Name", item.Name);
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
                command.CommandText = "DELETE FROM pets WHERE PetId = @PetId";
                command.Parameters.AddWithValue("@PetId", id);
                command.Prepare();
                return command.ExecuteNonQuery();
            }
        }

        public Pet? GetById(int id)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT PetId, ClientId, Name, Birthday FROM pets WHERE PetId = @PetId";
                command.Parameters.AddWithValue("@PetId", id);
                command.Prepare();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Pet
                        {
                            PetId = reader.GetInt32(0),
                            ClientId = reader.GetInt32(1),
                            Name = reader.GetString(2),
                            Birthday = new DateTime(reader.GetInt64(3))
                        };
                    }
                    return null;
                }
            }
        }

        public List<Pet> GetAll()
        {
            List<Pet> list = new List<Pet>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT PetId, ClientId, Name, Birthday FROM pets";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Pet pet = new Pet
                        {
                            PetId = reader.GetInt32(0),
                            ClientId = reader.GetInt32(1),
                            Name = reader.GetString(2),
                            Birthday = new DateTime(reader.GetInt64(3))
                        };
                        list.Add(pet);
                    }
                }
            }
            return list;
        }

        public List<Pet> GetByClientId(int clientId)
        {
            List<Pet> list = new List<Pet>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT PetId, ClientId, Name, Birthday FROM pets WHERE ClientId = @ClientId";
                command.Parameters.AddWithValue("@ClientId", clientId);
                command.Prepare();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Pet
                        {
                            PetId = reader.GetInt32(0),
                            ClientId = reader.GetInt32(1),
                            Name = reader.GetString(2),
                            Birthday = new DateTime(reader.GetInt64(3))
                        });
                    }
                }
            }
            return list;
        }

        public List<Pet> GetByName(string name)
        {
            List<Pet> list = new List<Pet>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT PetId, ClientId, Name, Birthday FROM pets WHERE Name = @Name";
                command.Parameters.AddWithValue("@Name", name);
                command.Prepare();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Pet
                        {
                            PetId = reader.GetInt32(0),
                            ClientId = reader.GetInt32(1),
                            Name = reader.GetString(2),
                            Birthday = new DateTime(reader.GetInt64(3))
                        });
                    }
                }
            }
            return list;
        }

        public List<Pet> GetByNameContains(string searchText)
        {
            List<Pet> list = new List<Pet>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT PetId, ClientId, Name, Birthday FROM pets WHERE Name LIKE @SearchPattern";
                command.Parameters.AddWithValue("@SearchPattern", $"%{searchText}%");
                command.Prepare();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Pet
                        {
                            PetId = reader.GetInt32(0),
                            ClientId = reader.GetInt32(1),
                            Name = reader.GetString(2),
                            Birthday = new DateTime(reader.GetInt64(3))
                        });
                    }
                }
            }
            return list;
        }

        public List<Pet> GetByAgeRange(int minAgeYears, int maxAgeYears)
        {
            List<Pet> list = new List<Pet>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                DateTime now = DateTime.Now;
                DateTime maxBirthday = now.AddYears(-minAgeYears);
                DateTime minBirthday = now.AddYears(-maxAgeYears - 1);

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT PetId, ClientId, Name, Birthday FROM pets WHERE Birthday BETWEEN @MinBirthday AND @MaxBirthday";
                command.Parameters.AddWithValue("@MinBirthday", minBirthday.Ticks);
                command.Parameters.AddWithValue("@MaxBirthday", maxBirthday.Ticks);
                command.Prepare();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Pet
                        {
                            PetId = reader.GetInt32(0),
                            ClientId = reader.GetInt32(1),
                            Name = reader.GetString(2),
                            Birthday = new DateTime(reader.GetInt64(3))
                        });
                    }
                }
            }
            return list;
        }

        public List<Pet> GetAllWithOwner()
        {
            List<Pet> list = new List<Pet>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT p.PetId, p.ClientId, p.Name, p.Birthday, 
                           c.SurName, c.FirstName, c.Patronymic, c.Document
                    FROM pets p
                    INNER JOIN clients c ON p.ClientId = c.ClientId
                    ORDER BY p.PetId";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var pet = new Pet
                        {
                            PetId = reader.GetInt32(0),
                            ClientId = reader.GetInt32(1),
                            Name = reader.GetString(2),
                            Birthday = new DateTime(reader.GetInt64(3))
                        };

                        list.Add(pet);
                    }
                }
            }
            return list;
        }

        public int CountByClientId(int clientId)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM pets WHERE ClientId = @ClientId";
                command.Parameters.AddWithValue("@ClientId", clientId);
                command.Prepare();

                long count = (long)command.ExecuteScalar();
                return (int)count;
            }
        }

        public List<Pet> GetYoungestPets(int count)
        {
            List<Pet> list = new List<Pet>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT PetId, ClientId, Name, Birthday 
                    FROM pets 
                    ORDER BY Birthday DESC 
                    LIMIT @Count";
                command.Parameters.AddWithValue("@Count", count);
                command.Prepare();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Pet
                        {
                            PetId = reader.GetInt32(0),
                            ClientId = reader.GetInt32(1),
                            Name = reader.GetString(2),
                            Birthday = new DateTime(reader.GetInt64(3))
                        });
                    }
                }
            }
            return list;
        }

    }
}