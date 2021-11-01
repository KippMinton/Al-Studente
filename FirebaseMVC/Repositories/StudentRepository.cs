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
    public class StudentRepository : IStudentRepository
    {
        private readonly IConfiguration _config;
        public StudentRepository(IConfiguration config)
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


        //void Add(Student student);
        public Student GetByUserId(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT s.Id, s.UserId, s.TeacherId, s.DOB, s.StartDate,
                                           s.PlayingSince, s.LevelId, s.LessonDayId, s.LessonTimeId
                                    FROM Student s
                                    WHERE s.UserId = @Id";

                    cmd.Parameters.AddWithValue("@Id", id);

                    Student student = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        student = new Student()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            TeacherId = reader.GetInt32(reader.GetOrdinal("TeacherId")),
                            DOB = reader.GetDateTime(reader.GetOrdinal("DOB")),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            PlayingSince = reader.GetInt32(reader.GetOrdinal("PlayingSince")),
                            LevelId = reader.GetInt32(reader.GetOrdinal("LevelId")),
                            LessonDayId = reader.GetInt32(reader.GetOrdinal("LessonDayId")),
                            LessonTimeId = reader.GetInt32(reader.GetOrdinal("LessonTimeId"))
                        };
                    }
                    reader.Close();

                    return student;
                }
            }
        }
        public List<StudentUserViewModel> GetAllByTeacher(int teacherId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT s.Id, s.UserId, s.TeacherId, s.DOB, s.StartDate,
                               s.PlayingSince, s.LevelId, s.LessonDayId, s.LessonTimeId,
                               u.Id as ProfileId, u.FirstName, u.LastName, u.DisplayName, u.Email, 
                               u.CreateDateTime, u.InstrumentId, u.ImageLocation, u.CreateDateTime,
                               u.UserTypeId, l.Id as LId, l.Name as Level, ut.Id as UtId, ut.Name as UserTypeName, i.Id as InstId,
                               i.Name as InstName, ld.Id as DayId, ld.Day as Day, lt.Id as TimeId,
                               lt.Time as Time
                        FROM Student s
                        LEFT JOIN UserProfile u ON s.UserId = u.Id
                        LEFT JOIN UserType ut ON u.UserTypeId = ut.Id
                        LEFT JOIN Instrument i on u.InstrumentId = i.Id
                        LEFT JOIN Level l on s.LevelId = l.Id
                        LEFT JOIN LessonDay ld on s.LessonDayId = ld.Id
                        LEFT JOIN LessonTime lt on s.LessonTimeId = lt.Id
                        WHERE s.TeacherId = @TeacherId";

                    cmd.Parameters.AddWithValue("@TeacherId", teacherId);
                    var reader = cmd.ExecuteReader();

                    var students = new List<StudentUserViewModel>();

                    while (reader.Read())
                    {
                        var newStudentVM = new StudentUserViewModel()
                        {
                            Student = new Student
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                TeacherId = reader.GetInt32(reader.GetOrdinal("TeacherId")),
                                DOB = reader.GetDateTime(reader.GetOrdinal("DOB")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                PlayingSince = reader.GetInt32(reader.GetOrdinal("PlayingSince")),
                                LevelId = reader.GetInt32(reader.GetOrdinal("LevelId")),
                                LessonDayId = reader.GetInt32(reader.GetOrdinal("LessonDayId")),
                                LessonTimeId = reader.GetInt32(reader.GetOrdinal("LessonTimeId"))
                            },

                            UserProfile = new UserProfile
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ProfileId")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                                ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                                InstrumentId = reader.GetInt32(reader.GetOrdinal("InstrumentId")),
                                UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                UserType = new UserType()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                    Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                                },
                            },

                            Instrument = new Instrument
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("InstId")),
                                Name = reader.GetString(reader.GetOrdinal("InstName"))
                            },
                            
                            Level = new Level
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("LId")),
                                Name = reader.GetString(reader.GetOrdinal("Level"))
                            },

                            LessonDay = new LessonDay 
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("DayId")),
                                Day = reader.GetString(reader.GetOrdinal("Day"))
                            },

                            LessonTime = new LessonTime
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("TimeId")),
                                Time = reader.GetString(reader.GetOrdinal("Time"))
                            }
                        };
                        students.Add(newStudentVM);
                    }

                    reader.Close();

                    return students;
                }
            }
        }

        public void Add(Student student)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    INSERT INTO
                                    Student (UserId, TeacherId, DOB, StartDate,
                                             PlayingSince, LevelId, LessonDayId, LessonTimeId)
                                    OUTPUT INSERTED.ID
                                    VALUES(@userId, @teacherId, @dOB, @startDate,
                                           @playingSince, @levelId, @lessonDayId, @lessonTimeId)";

                    cmd.Parameters.AddWithValue("@userId", student.UserId);
                    cmd.Parameters.AddWithValue("@teacherId", student.TeacherId);
                    cmd.Parameters.AddWithValue("@dOB", student.DOB);
                    cmd.Parameters.AddWithValue("@startDate", student.StartDate);
                    cmd.Parameters.AddWithValue("@playingSince", student.PlayingSince);
                    cmd.Parameters.AddWithValue("@levelId", student.LevelId);
                    cmd.Parameters.AddWithValue("@lessonDayId", student.LessonDayId);
                    cmd.Parameters.AddWithValue("@lessonTimeId", student.LessonTimeId);

                    student.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Student student)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Student
                            SET DOB = @dOB,
                                StartDate = @startDate,
                                PlayingSince = @playingSince,
                                LevelId = @levelId,
                                LessonDayId = @lessonDayId,
                                LessonTimeId = @lessonTimeId
                            WHERE UserId = @userId";
                    cmd.Parameters.AddWithValue("@dOB", student.DOB);
                    cmd.Parameters.AddWithValue("@startDate", student.StartDate);
                    cmd.Parameters.AddWithValue("@playingSince", student.PlayingSince);
                    cmd.Parameters.AddWithValue("@levelId", student.LevelId);
                    cmd.Parameters.AddWithValue("@lessonDayId", student.LessonDayId);
                    cmd.Parameters.AddWithValue("@lessonTimeId", student.LessonTimeId);
                    cmd.Parameters.AddWithValue("@userId", student.UserId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}