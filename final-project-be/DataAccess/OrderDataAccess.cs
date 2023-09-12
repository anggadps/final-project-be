using final_project_be.Models;
using MySql.Data.MySqlClient;

namespace final_project_be.DataAccess
{
    public class OrderDataAccess
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public OrderDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        // insert order
        public bool Insert(Order order)
        {
            bool result = false;
            string query = "INSERT INTO orders (id, id_user, id_course, schedule_date) VALUES (@id, @id_user, @id_course,  @schedule_date)";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@id", order.Id);
                        command.Parameters.AddWithValue("@id_user", order.id_user);
                        command.Parameters.AddWithValue("@id_course", order.id_course);
                        command.Parameters.AddWithValue("@schedule_date", order.schedule_date);


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

                return result;
            }
        }
    }
}