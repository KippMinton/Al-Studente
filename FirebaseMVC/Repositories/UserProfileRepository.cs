using Microsoft.Data.SqlClient;
using AlStudente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AlStudente.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {

        private readonly IConfiguration _config;

        public UserProfileRepository(IConfiguration config)
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

        public UserProfile GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT Id, Email, FirebaseUserId
                                    FROM UserProfile
                                    WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@id", id);

                    UserProfile userProfile = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userProfile = new UserProfile
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirebaseUserId = reader.GetString(reader.GetOrdinal("FirebaseUserId")),
                        };
                    }
                    reader.Close();

                    return userProfile;
                }
            }
        }

        public UserProfile GetByFirebaseUserId(string firebaseUserId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT Id, Email, FirebaseUserId
                                    FROM UserProfile
                                    WHERE FirebaseUserId = @FirebaseuserId";

                    cmd.Parameters.AddWithValue("@FirebaseUserId", firebaseUserId);

                    UserProfile userProfile = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userProfile = new UserProfile
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirebaseUserId = reader.GetString(reader.GetOrdinal("FirebaseUserId")),
                        };
                    }
                    reader.Close();

                    return userProfile;
                }
            }
        }

        public void Add(UserProfile userProfile)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO
                                        UserProfile (FirebaseUserId, FirstName, LastName, DisplayName, Email, CreateDateTime, ImageLocation, UserTypeId, InstrumentId, Bio) 
                                        OUTPUT INSERTED.ID
                                        VALUES(@firebaseUserId, @FirstName, @LastName, @DisplayName, @Email, @CreateDateTime, @ImageLocation, @UserTypeId, @InstrumentId, @Bio)";

                    cmd.Parameters.AddWithValue("@firebaseUserId", userProfile.FirebaseUserId);
                    cmd.Parameters.AddWithValue("@email", userProfile.Email);
                    cmd.Parameters.AddWithValue();
                    cmd.Parameters.AddWithValue();
                    cmd.Parameters.AddWithValue();
                    cmd.Parameters.AddWithValue();
                    cmd.Parameters.AddWithValue();

                    userProfile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}
