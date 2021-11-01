using Microsoft.Data.SqlClient;
using AlStudente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AlStudente.Utils;

namespace AlStudente.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {

        private readonly IConfiguration _config;

        public TeacherRepository(IConfiguration config)
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

        public Teacher GetByUserId(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT t.Id, t.UserId, t.AcceptingStudents, t.LessonRate
                                    FROM Teacher t
                                    WHERE t.UserId = @Id";

                    cmd.Parameters.AddWithValue("@Id", id);

                    Teacher teacher = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        teacher = new Teacher()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            AcceptingStudents = reader.GetBoolean(reader.GetOrdinal("AcceptingStudents")),
                            LessonRate = reader.GetInt32(reader.GetOrdinal("LessonRate"))
                        };
                    }
                    reader.Close();

                    return teacher;
                }
            }
        }

        public void Add(Teacher teacher)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    INSERT INTO
                                    Teacher (UserId, AcceptingStudents, LessonRate)
                                    OUTPUT INSERTED.ID
                                    VALUES(@userId, @acceptingStudents, @lessonRate)";

                    cmd.Parameters.AddWithValue("@userId", teacher.UserId);
                    cmd.Parameters.AddWithValue("@acceptingStudents", teacher.AcceptingStudents);
                    cmd.Parameters.AddWithValue("@lessonRate", teacher.LessonRate);

                    teacher.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Teacher teacher)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE teacher
                            SET AcceptingStudents = @acceptingStudents,
                                LessonRate = @lessonRate
                            WHERE UserId = @userId";
                    cmd.Parameters.AddWithValue("@acceptingStudents", teacher.AcceptingStudents);
                    cmd.Parameters.AddWithValue("@lessonRate", teacher.LessonRate);
                    cmd.Parameters.AddWithValue("@userId", teacher.UserId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}