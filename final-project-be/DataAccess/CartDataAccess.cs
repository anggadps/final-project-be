using final_project_be.DTOs.Cart;
using final_project_be.Models;
using MySql.Data.MySqlClient;

namespace final_project_be.DataAccess
{
    public class CartDataAccess
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public CartDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public bool AddCart(Cart cart)
        {
            bool result = false;

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.Clear();

                    command.CommandText = "INSERT INTO carts (id, id_user, id_schedule) " +
                        "VALUES (@id, @id_user, @id_schedule);";

                    command.Parameters.AddWithValue("@id", cart.Id);
                    command.Parameters.AddWithValue("@id_user", cart.Id_user);
                    command.Parameters.AddWithValue("@id_schedule", cart.Id_schedule);

                    try
                    {
                        connection.Open();

                        int execresult = command.ExecuteNonQuery();

                        result = execresult > 0 ? true : false;
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

        public Cart? CheckCart(string id_schedule, string id_user)
        {
            Cart? cart = null;

            string query = "SELECT * FROM carts WHERE id_schedule = @id_schedule AND id_user = @id_user";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@id_schedule", id_schedule);
                        command.Parameters.AddWithValue("@id_user", id_user);

                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cart = new Cart
                                {
                                    Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                    Id_user = reader["Name"].ToString() ?? string.Empty,
                                    Id_schedule = reader["Name"].ToString() ?? string.Empty,
                                    Id_course = reader["Email"].ToString() ?? string.Empty,
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

            return cart;
        }


        public List<ViewCartDTO> GetViewCart(string id)
        {
            List<ViewCartDTO> carts = new List<ViewCartDTO>();

            string query = "SELECT carts.id, schedules.id AS id_schedule, courses.id AS id_course, categories.name AS category_name, courses.name AS course_name, courses.price, schedules.schedule_date, courses.img " +
                "FROM carts " +
                "INNER JOIN schedules ON carts.id_schedule = schedules.id " +
                "INNER JOIN courses ON schedules.id_course = courses.id " +
                "INNER JOIN categories ON courses.id_category = categories.id " +
                "WHERE carts.id_user = @id;";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Connection = connection;
                    command.Parameters.Clear();

                    command.CommandText = query;
                    command.Parameters.AddWithValue("@id", id);
                    try
                    {
                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                carts.Add(new ViewCartDTO
                                {
                                    Id = Guid.Parse(reader["id"].ToString() ?? string.Empty),
                                    Id_schedule = Guid.Parse(reader["id_schedule"].ToString() ?? string.Empty),
                                    Id_course = Guid.Parse(reader["id_course"].ToString() ?? string.Empty),
                                    Category_name = reader["category_name"].ToString() ?? string.Empty,
                                    Course_name = reader["course_name"].ToString() ?? string.Empty,
                                    Price = Convert.ToInt32(reader["price"]),
                                    Schedule_date = Convert.ToDateTime(reader["schedule_date"]),
                                    Img = reader["img"].ToString() ?? string.Empty,
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
            return carts;
        }

        public bool DeleteCart(Guid id)
        {
            bool result = false;

            string query = "DELETE FROM carts WHERE Id = @Id";

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
