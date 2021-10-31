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
    public class LevelRepository : ILevelRepository
    {
        private readonly IConfiguration _config;
        public LevelRepository(IConfiguration config)
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

        public Level GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name
                                        FROM Level
                                        WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@Id", id);

                    Level level = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        level = new Level
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                    }
                    reader.Close();

                    return level;
                }
            }
        }

        public List<Level> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name FROM Level";

                    var reader = cmd.ExecuteReader();

                    List<Level> levels = new List<Level>();

                    while (reader.Read())
                    {
                        var level = new Level
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };

                        levels.Add(level);
                    }
                    reader.Close();
                    return levels;
                }
            }
        }
    }
}
