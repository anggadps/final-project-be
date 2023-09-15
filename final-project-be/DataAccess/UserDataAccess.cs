using final_project_be.Models;
using MySql.Data.MySqlClient;

namespace final_project_be.DataAccess
{
    public class UserDataAccess
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public UserDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        // get all
        public List<User> GetAll()
        {
            List<User> users = new List<User>();

            string query = "SELECT * FROM users";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(new User
                                {
                                    Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                    Name = reader["Name"].ToString() ?? string.Empty,
                                    Email = reader["Email"].ToString() ?? string.Empty,
                                    Password = reader["Password"].ToString() ?? string.Empty,
                                });
                            }
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return users;
        }

        // insert user
        public bool CreateUserAccount(User user)
        {
            bool result = false;
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                MySqlTransaction transaction = connection.BeginTransaction();

                try
                {


                    MySqlCommand command1 = new MySqlCommand();
                    command1.Connection = connection;
                    command1.Transaction = transaction;
                    command1.Parameters.Clear();

                    command1.CommandText = "INSERT INTO users (Id, id_user_level, Name, Email, Password, Is_active ) VALUES (@Id, @id_user_level, @Name, @Email, @Password, @Is_active )";
                    command1.Parameters.AddWithValue("@Id", user.Id);
                    command1.Parameters.AddWithValue("@id_user_level", user.Id_user_level);
                    command1.Parameters.AddWithValue("@Name", user.Name);
                    command1.Parameters.AddWithValue("@Email", user.Email);
                    command1.Parameters.AddWithValue("@Password", user.Password);
                    command1.Parameters.AddWithValue("@Is_Active", user.Is_active);



                    var result1 = command1.ExecuteNonQuery();

                    transaction.Commit();

                    result = true;

                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }

        // activated
        public bool ActivatedUser(Guid id)
        {
            bool result = false;

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.Parameters.Clear();

                command.CommandText = "UPDATE Users SET is_active = 1 WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);

                try
                {
                    connection.Open();

                    result = command.ExecuteNonQuery() > 0;
                }
                catch
                {
                    throw;
                }
                finally { connection.Close(); }
            }

            return result;

        }


        // get by email
        public User? CheckUser(string email)
        {
            User? user = null;

            string query = "SELECT * FROM users WHERE Email = @email";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@email", email);

                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                user = new User
                                {
                                    Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                    Id_user_level = Guid.Parse(reader["Id_user_level"].ToString() ?? string.Empty),
                                    Name = reader["Name"].ToString() ?? string.Empty,
                                    Email = reader["Email"].ToString() ?? string.Empty,
                                    Password = reader["Password"].ToString() ?? string.Empty,
                                    Is_active = Convert.ToInt32(reader["Is_active"]),
                                };
                            }
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return user;
        }


        // GetUserLevel
        public UserLevel? GetUserLevel(Guid id)
        {
            UserLevel? userLevel = null;


            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();


                    command.CommandText = "SELECT user_levels.name " +
                        "FROM users " +
                        "INNER JOIN user_levels ON users.id_user_level = user_levels.id WHERE user_levels.id = @Id";

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userLevel = new UserLevel
                            {
                                Name = reader["Name"].ToString() ?? string.Empty,
                            };
                        }
                    }

                    connection.Close();
                }
            }

            return userLevel;
        }

        // Reset password
        public bool ResetPassword(string email, string password)
        {
            bool result = false;

            string query = "UPDATE Users SET Password = @Password WHERE Email = @Email";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();

                    command.CommandText = query;

                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }

        // update user
        public bool Update(Guid id, User user)
        {
            bool result = false;

            string query = "UPDATE users SET Name = @Name, Email = @Email, Password = @Password, Id_user_level = @Id_user_level WHERE Id = @Id";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        command.Connection = connection;
                        command.Parameters.Clear();
                        command.CommandText = query;

                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Name", user.Name);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@Id_user_level", user.Id_user_level);
                        connection.Open();

                        result = command.ExecuteNonQuery() > 0 ? true : false;

                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return result;
        }

        // delete user
        public bool Delete(Guid id)
        {
            bool result = false;

            string query = "DELETE FROM users WHERE Id = @Id";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        command.Connection = connection;
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@Id", id);


                        connection.Open();

                        result = command.ExecuteNonQuery() > 0 ? true : false;
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return result;
        }


    }
}