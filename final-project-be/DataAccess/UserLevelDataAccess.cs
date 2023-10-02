using final_project_be.Models;
using MySql.Data.MySqlClient;

namespace final_project_be.DataAccess
{
    public class UserLevelDataAccess
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public UserLevelDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }


        /*Get all data*/
        public List<UserLevel> GetAll()
        {
            List<UserLevel> user_levels = new List<UserLevel>();

            string query = "SELECT * FROM user_levels";

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
                                user_levels.Add(new UserLevel
                                {
                                    Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                    Name = reader["Name"].ToString() ?? string.Empty,
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

            return user_levels;
        }
    }
}
