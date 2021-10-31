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
    public class LessonDayRepository : ILessonDayRepository
    {
        private readonly IConfiguration _config;
        public LessonDayRepository(IConfiguration config)
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

        public LessonDay GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name
                                        FROM LessonDay
                                        WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@Id", id);

                    LessonDay lessonDay = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lessonDay = new LessonDay
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Day = reader.GetString(reader.GetOrdinal("Day"))
                        };
                    }
                    reader.Close();

                    return lessonDay;
                }
            }
        }

        public List<LessonDay> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Day FROM LessonDay";

                    var reader = cmd.ExecuteReader();

                    List<LessonDay> lessonDays = new List<LessonDay>();

                    while (reader.Read())
                    {
                        var lessonDay = new LessonDay
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Day = reader.GetString(reader.GetOrdinal("Day"))
                        };

                        lessonDays.Add(lessonDay);
                    }
                    reader.Close();
                    return lessonDays;
                }
            }
        }
    }
}
