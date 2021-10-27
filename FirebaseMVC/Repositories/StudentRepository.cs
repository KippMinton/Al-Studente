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
                                           s.PlayingSince, s.Level, s.LessonDayId, s.LessonTimeId
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
                            Level = reader.GetInt32(reader.GetOrdinal("Level")),
                            LessonDayId = reader.GetInt32(reader.GetOrdinal("LessonDayId")),
                            LessonTimeId = reader.GetInt32(reader.GetOrdinal("LessonTimeId"))
                        };
                    }
                    reader.Close();

                    return student;
                }
            }
        }
        public List<Student> GetAllByTeacher(int teacherId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT s.Id, s.UserId, s.TeacherId, s.DOB, s.StartDate,
                               s.PlayingSince, s.Level, s.LessonDayId, s.LessonTimeId
                        FROM Student s
                        WHERE s.TeacherId = @TeacherId";

                    cmd.Parameters.AddWithValue("@TeacherId", teacherId);
                    var reader = cmd.ExecuteReader();

                    var students = new List<Student>();

                    var newStudent =

                    while (reader.Read())
                    {
                        students.Add(NewStudent)
                    }
                }
            }
        }







        //public List<Post> GetUserPosts(int userProfileId)
        //{
        //    using (var conn = Connection)
        //    {
        //        conn.Open();
        //        using (var cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = @"
        //               SELECT p.Id, p.Title, p.Content, 
        //                      p.ImageLocation AS HeaderImage,
        //                      p.CreateDateTime, p.PublishDateTime, p.IsApproved,
        //                      p.CategoryId, p.UserProfileId,
        //                      c.[Name] AS CategoryName,
        //                      u.FirstName, u.LastName, u.DisplayName, 
        //                      u.Email, u.CreateDateTime, u.ImageLocation AS AvatarImage,
        //                      u.UserTypeId, 
        //                      ut.[Name] AS UserTypeName     
        //                 FROM Post p
        //                      LEFT JOIN Category c ON p.CategoryId = c.id
        //                      LEFT JOIN UserProfile u ON p.UserProfileId = u.id
        //                      LEFT JOIN UserType ut ON u.UserTypeId = ut.id 
        //                WHERE IsApproved = 1 AND PublishDateTime < SYSDATETIME() AND p.UserProfileId = @userProfileId
        //                ORDER BY p.PublishDateTime DESC";

        //            cmd.Parameters.AddWithValue("@userProfileId", userProfileId);
        //            var reader = cmd.ExecuteReader();

        //            var posts = new List<Post>();

        //            while (reader.Read())
        //            {
        //                posts.Add(NewPostFromReader(reader));
        //            }

        //            reader.Close();

        //            return posts;
        //        }
        //    }
        }
    }
}