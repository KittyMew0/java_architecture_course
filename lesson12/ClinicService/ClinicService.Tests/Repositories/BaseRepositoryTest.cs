using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClinicService.Services.Repositories;
using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace ClinicService.Tests.Repositories
{
    public abstract class BaseRepositoryTests : IDisposable
    {
        protected readonly SqliteConnection _connection;
        protected readonly ClientRepository _clientRepository;
        protected readonly PetRepository _petRepository;
        protected readonly ConsultationRepository _consultationRepository;

        protected BaseRepositoryTests()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();
            CreateSchema();

            _clientRepository = new ClientRepository();
            _petRepository = new PetRepository();
            _consultationRepository = new ConsultationRepository();
        }

        private void CreateSchema()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                CREATE TABLE Clients(
                    ClientId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Document TEXT NOT NULL,
                    SurName TEXT NOT NULL,
                    FirstName TEXT NOT NULL,
                    Patronymic TEXT,
                    Birthday INTEGER NOT NULL
                );
                
                CREATE TABLE Pets(
                    PetId INTEGER PRIMARY KEY AUTOINCREMENT,
                    ClientId INTEGER NOT NULL,
                    Name TEXT NOT NULL,
                    Birthday INTEGER NOT NULL,
                    FOREIGN KEY(ClientId) REFERENCES Clients(ClientId)
                );
                
                CREATE TABLE Consultations(
                    ConsultationId INTEGER PRIMARY KEY AUTOINCREMENT,
                    ClientId INTEGER NOT NULL,
                    PetId INTEGER NOT NULL,
                    ConsultationDate INTEGER NOT NULL,
                    Description TEXT NOT NULL,
                    FOREIGN KEY(ClientId) REFERENCES Clients(ClientId),
                    FOREIGN KEY(PetId) REFERENCES Pets(PetId)
                );
            ";

            command.ExecuteNonQuery();
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}