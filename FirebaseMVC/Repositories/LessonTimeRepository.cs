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
    public class LessonTimeRepository : ILessonTimeRepository
    {
        private readonly IConfiguration _config;
        public LessonTimeRepository(IConfiguration config)
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

        public LessonTime GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Time
                                        FROM LessonTime
                                        WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@Id", id);

                    LessonTime lessonTime = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lessonTime = new LessonTime
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Time = reader.GetString(reader.GetOrdinal("Time"))
                        };
                    }
                    reader.Close();

                    return lessonTime;
                }
            }
        }

        public List<LessonTime> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Time FROM LessonTime";

                    var reader = cmd.ExecuteReader();

                    List<LessonTime> lessonTimes = new List<LessonTime>();

                    while (reader.Read())
                    {
                        var lessonTime = new LessonTime
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Time = reader.GetString(reader.GetOrdinal("Time"))
                        };

                        lessonTimes.Add(lessonTime);
                    }
                    reader.Close();
                    return lessonTimes;
                }
            }
        }
    }
}
