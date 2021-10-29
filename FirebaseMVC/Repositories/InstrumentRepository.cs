using Microsoft.Data.SqlClient;
using AlStudente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AlStudente.Utils;
using AlStudente.Models.ViewModels;

namespace AlStudente.Repositories
{
    public class InstrumentRepository : IInstrumentRepository
    {
        private readonly IConfiguration _config;
        public InstrumentRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public Instrument GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name
                                        FROM Instrument
                                        WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@Id", id);

                    Instrument instrument = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        instrument = new Instrument
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                    }
                    reader.Close();

                    return instrument;
                }
            }
        }

        public List<Instrument> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name FROM Instrument";

                    var reader = cmd.ExecuteReader();

                    List<Instrument> instruments = new List<Instrument>();

                    while (reader.Read())
                    {
                        var instrument = new Instrument
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };

                        instruments.Add(instrument);
                    }
                    reader.Close();
                    return instruments;
                }
            }
        }
    }
}
