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
            string query = "INSERT INTO orders (id, no_invoice, id_user, total_course, total_price, pay_date) VALUES (@id, @no_invoice, @id_user,  @total_course, @total_price, @pay_date)";

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
                        command.Parameters.AddWithValue("@pay_date", order.Pay_date);



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


        public List<Order> ViewInvoice(string id)
        {
            List<Order> orders = new List<Order>();

            string query = $"SELECT * FROM orders WHERE id_user = @id ORDER BY pay_date DESC";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@id", id);
                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Order order = new Order
                                {
                                    Id = Guid.Parse(reader["id"].ToString() ?? string.Empty),
                                    No_invoice = reader["no_invoice"].ToString() ?? string.Empty,
                                    Id_user = reader["id_user"].ToString() ?? string.Empty,
                                    Total_course = decimal.Parse(reader["total_course"].ToString() ?? "0"),
                                    Total_price = decimal.Parse(reader["total_price"].ToString() ?? "0"),
                                    Pay_date = DateTime.Parse(reader["pay_date"].ToString() ?? string.Empty),
                                };

                                orders.Add(order);
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

            return orders;
        }



        public List<Order> ViewInvoiceAdmin()
        {
            List<Order> orders = new List<Order>();

            string query = $"SELECT * FROM orders";

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
                                Order order = new Order
                                {
                                    Id = Guid.Parse(reader["id"].ToString() ?? string.Empty),
                                    No_invoice = reader["no_invoice"].ToString() ?? string.Empty,
                                    Id_user = reader["id_user"].ToString() ?? string.Empty,
                                    Total_course = decimal.Parse(reader["total_course"].ToString() ?? "0"),
                                    Total_price = decimal.Parse(reader["total_price"].ToString() ?? "0"),
                                    Pay_date = DateTime.Parse(reader["pay_date"].ToString() ?? string.Empty),
                                };

                                orders.Add(order);
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

            return orders;
        }


        public List<Order> ViewInvoiceDetail(Guid id, string id_user)
        {
            List<Order> orders = new List<Order>();

            string query = $"SELECT o.id, o.id_user, o.no_invoice, o.pay_date, o.total_course , o.total_price, c.name AS course_name, cat.name AS category_name, s.schedule_date, c.price , c.img " +
                $"FROM orders AS o " +
                $"INNER JOIN order_details AS od ON o.id = od.id_order " +
                $"INNER JOIN courses AS c ON od.id_course = c.id " +
                $"INNER JOIN categories AS cat ON c.id_category = cat.id " +
                $"INNER JOIN schedules AS s ON od.id_schedule = s.id " +
                $"WHERE o.id = @id AND o.id_user = @id_user";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@id_user", id_user);
                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Order order = new Order
                                {
                                    Id = Guid.Parse(reader["id"].ToString() ?? string.Empty),
                                    No_invoice = reader["no_invoice"].ToString() ?? string.Empty,
                                    Id_user = reader["id_user"].ToString() ?? string.Empty,
                                    Course_name = reader["course_name"].ToString() ?? string.Empty,
                                    Category_name = reader["category_name"].ToString() ?? string.Empty,
                                    Schedule_date = DateTime.Parse(reader["schedule_date"].ToString() ?? string.Empty),
                                    Price = Decimal.Parse(reader["price"].ToString() ?? string.Empty),
                                    Total_course = decimal.Parse(reader["total_course"].ToString() ?? "0"),
                                    Total_price = decimal.Parse(reader["total_price"].ToString() ?? "0"),
                                    Pay_date = DateTime.Parse(reader["pay_date"].ToString() ?? string.Empty),
                                    Img = reader["img"].ToString() ?? string.Empty,
                                };

                                orders.Add(order);
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

            return orders;
        }


        public List<Order> ViewInvoiceDetailAdmin(Guid id)
        {
            List<Order> orders = new List<Order>();

            string query = $"SELECT o.id, o.id_user, o.no_invoice, o.pay_date, o.total_course , o.total_price, c.name AS course_name, cat.name AS category_name, s.schedule_date, c.price , c.img " +
                $"FROM orders AS o " +
                $"INNER JOIN order_details AS od ON o.id = od.id_order " +
                $"INNER JOIN courses AS c ON od.id_course = c.id " +
                $"INNER JOIN categories AS cat ON c.id_category = cat.id " +
                $"INNER JOIN schedules AS s ON od.id_schedule = s.id " +
                $"WHERE o.id = @id";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@id", id);
                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Order order = new Order
                                {
                                    Id = Guid.Parse(reader["id"].ToString() ?? string.Empty),
                                    No_invoice = reader["no_invoice"].ToString() ?? string.Empty,
                                    Id_user = reader["id_user"].ToString() ?? string.Empty,
                                    Course_name = reader["course_name"].ToString() ?? string.Empty,
                                    Category_name = reader["category_name"].ToString() ?? string.Empty,
                                    Schedule_date = DateTime.Parse(reader["schedule_date"].ToString() ?? string.Empty),
                                    Price = Decimal.Parse(reader["price"].ToString() ?? string.Empty),
                                    Total_course = decimal.Parse(reader["total_course"].ToString() ?? "0"),
                                    Total_price = decimal.Parse(reader["total_price"].ToString() ?? "0"),
                                    Pay_date = DateTime.Parse(reader["pay_date"].ToString() ?? string.Empty),
                                    Img = reader["img"].ToString() ?? string.Empty,
                                };

                                orders.Add(order);
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

            return orders;
        }


        public List<Order> ViewMyClass(string id)
        {
            List<Order> orders = new List<Order>();

            string query = $"SELECT o.id, o.id_user, c.name AS course_name, cat.name AS category_name, s.schedule_date, c.img " +
                $"FROM orders AS o " +
                $"INNER JOIN order_details AS od ON o.id = od.id_order " +
                $"INNER JOIN courses AS c ON od.id_course = c.id " +
                $"INNER JOIN categories AS cat ON c.id_category = cat.id " +
                $"INNER JOIN schedules AS s ON od.id_schedule = s.id " +
                $"WHERE o.id_user = @id ORDER BY s.schedule_date ASC";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@id", id);
                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Order order = new Order
                                {
                                    Id = Guid.Parse(reader["id"].ToString() ?? string.Empty),
                                    Id_user = reader["id_user"].ToString() ?? string.Empty,
                                    Course_name = reader["course_name"].ToString() ?? string.Empty,
                                    Category_name = reader["category_name"].ToString() ?? string.Empty,
                                    Schedule_date = DateTime.Parse(reader["schedule_date"].ToString() ?? string.Empty),
                                    Img = reader["img"].ToString() ?? string.Empty,
                                };

                                orders.Add(order);
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

            return orders;
        }



    }
}