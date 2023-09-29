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
            string query = "INSERT INTO orders (id, no_invoice, id_user, total_course, total_price) VALUES (@id, @no_invoice, @id_user,  @total_course, @total_price)";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@id", order.Id);
                        command.Parameters.AddWithValue("@no_invoice", order.No_invoice);
                        command.Parameters.AddWithValue("@id_user", order.Id_user);
                        command.Parameters.AddWithValue("@total_course", order.Total_course);
                        command.Parameters.AddWithValue("@total_price", order.Total_price);



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