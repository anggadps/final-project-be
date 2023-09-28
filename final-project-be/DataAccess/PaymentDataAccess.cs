﻿using final_project_be.Models;
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
