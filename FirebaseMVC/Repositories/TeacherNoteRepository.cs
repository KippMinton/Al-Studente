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
    public class TeacherNoteRepository : ITeacherNoteRepository
    {
        private readonly IConfiguration _config;
        public TeacherNoteRepository(IConfiguration config)
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

        public TeacherNote GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select tn.Id, tn.TeacherId, tn.StudentId,
                                        tn.Title, tn.Content, tn.CreateDateTime
                                        FROM TeacherNote tn
                                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@Id", id);

                    TeacherNote teacherNote = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        teacherNote = new TeacherNote
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            TeacherId = reader.GetInt32(reader.GetOrdinal("TeacherId")),
                            StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"))
                        };
                    }
                    reader.Close();

                    return teacherNote;
                }
            }
        }

        public List<TeacherNote> GetAllByStudentId(int studentId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select tn.Id, tn.TeacherId, tn.StudentId,
                                        tn.Title, tn.Content, tn.CreateDateTime
                                        FROM TeacherNote tn
                                        WHERE StudentId = @studentId";

                    cmd.Parameters.AddWithValue("@studentId", studentId);
                    var reader = cmd.ExecuteReader();

                    var notes = new List<TeacherNote>();

                    while (reader.Read())
                    {
                        var teacherNote = new TeacherNote
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            TeacherId = reader.GetInt32(reader.GetOrdinal("TeacherId")),
                            StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"))
                        };

                        notes.Add(teacherNote);
                    }
                    reader.Close();

                    return notes;
                }
            }
        }

        public void Add(TeacherNote teacherNote)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    INSERT INTO
                                    TeacherNote (TeacherId, StudentId, Title,
                                                 Content, CreateDateTime)
                                    OUTPUT INSERTED.ID
                                    VALUES (@teacherId, @studentId, @title,
                                                 @content, GETDATE())";

                    cmd.Parameters.AddWithValue("@teacherId", teacherNote.TeacherId);
                    cmd.Parameters.AddWithValue("@studentId", teacherNote.StudentId);
                    cmd.Parameters.AddWithValue("@title", teacherNote.Title);
                    cmd.Parameters.AddWithValue("@content", teacherNote.Content);

                    teacherNote.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(TeacherNote teacherNote)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE TeacherNote
                            Set Title = @title,
                                Content = @content
                            WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@title", teacherNote.Title);
                    cmd.Parameters.AddWithValue("@content", teacherNote.Content);
                    cmd.Parameters.AddWithValue("@id", teacherNote.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE from TeacherNote WHERE Id = @id";
                    
                    cmd.Parameters.AddWithValue(@"id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}