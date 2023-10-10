using final_project_be.Models;
using MySql.Data.MySqlClient;

namespace final_project_be.DataAccess
{
    public class OrderDetailDataAccess
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public OrderDetailDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public bool Insert(OrderDetail orderDetail)
        {
            bool result = false;
            string query = "INSERT INTO order_details (id, id_order, id_schedule, id_course) VALUES (@id, @id_order, @id_schedule,  @id_course)";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@id", orderDetail.Id);
                        command.Parameters.AddWithValue("@id_order", orderDetail.Id_order);
                        command.Parameters.AddWithValue("@id_schedule", orderDetail.Id_schedule);
                        command.Parameters.AddWithValue("@id_course", orderDetail.Id_course);


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
