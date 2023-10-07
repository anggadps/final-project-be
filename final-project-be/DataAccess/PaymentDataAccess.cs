using final_project_be.Models;
using MySql.Data.MySqlClient;

namespace final_project_be.DataAccess
{
    public class PaymentDataAccess
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public PaymentDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }



        public List<Payment> GetAll()
        {
            List<Payment> payments = new List<Payment>();
            string query = $"SELECT * FROM payments WHERE is_active = true";
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        command.Connection = connection;
                        command.CommandText = query;
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Payment payment = new Payment
                                {
                                    Id = reader.GetGuid("Id"),
                                    Name = reader.GetString("Name"),
                                    Logo = reader.GetString("Logo"),
                                    Is_active = reader.GetBoolean("Is_active")
                                };
                                payments.Add(payment);
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
            return payments;
        }


        public List<Payment> GetAllByAdmin()
        {
            List<Payment> payments = new List<Payment>();
            string query = $"SELECT * FROM payments";
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        command.Connection = connection;
                        command.CommandText = query;
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Payment payment = new Payment
                                {
                                    Id = reader.GetGuid("Id"),
                                    Name = reader.GetString("Name"),
                                    Logo = reader.GetString("Logo"),
                                    Is_active = reader.GetBoolean("Is_active")
                                };
                                payments.Add(payment);
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
            return payments;
        }



        /*Get by ID*/
        public Payment? GetById(Guid id)
        {
            Payment? payment = null;

            string query = $"SELECT * FROM payments WHERE Id = @id";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        command.Connection = connection;
                        command.Parameters.Clear();

                        command.CommandText = query;
                        command.Parameters.AddWithValue("@id", id);


                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                payment = new Payment
                                {
                                    Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                    Name = reader["Name"].ToString() ?? string.Empty,
                                    Logo = reader["Logo"].ToString() ?? string.Empty,
                                    Is_active = reader.GetBoolean("Is_active")

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

            return payment;
        }

        public bool Insert(Payment payment)
        {
            bool result = false;
            string query = $"INSERT INTO payments (Id, Name, Logo, Is_active) VALUES (@id, @name, @logo, @is_active)";
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        command.Connection = connection;
                        command.Parameters.Clear();
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@id", payment.Id);
                        command.Parameters.AddWithValue("@name", payment.Name);
                        command.Parameters.AddWithValue("@logo", payment.Logo);
                        command.Parameters.AddWithValue("@is_active", payment.Is_active);

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

        public bool Update(Guid id, Payment payment)
        {
            bool result = false;
            string query = $"UPDATE payments SET Name = @name, Logo = @logo, is_active = @is_active WHERE Id = @id";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    try
                    {
                        command.Connection = connection;
                        command.Parameters.Clear();
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@id", payment.Id);
                        command.Parameters.AddWithValue("@name", payment.Name);
                        command.Parameters.AddWithValue("@logo", payment.Logo);
                        command.Parameters.AddWithValue("@is_active", payment.Is_active);


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
