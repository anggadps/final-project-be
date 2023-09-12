using final_project_be.Models;
using MySql.Data.MySqlClient;

namespace final_project_be.DataAccess
{
    public class InvoiceDataAccess
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public InvoiceDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        // insert invoice
        public bool Insert(Invoice invoice, InvoiceDetail invoiceDetail)
        {
            bool result = false;

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                MySqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    string query = "INSERT INTO invoices (id, id_user, schedule_date, no_invoice, total_price) VALUES (@id, @id_user, @schedule_date, @no_invoice, @total_price)";
                    using (MySqlCommand command = new MySqlCommand(query, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@id", invoice.Id);
                        command.Parameters.AddWithValue("@id_user", invoice.id_user);
                        command.Parameters.AddWithValue("@schedule_date", invoice.schedule_date);
                        command.Parameters.AddWithValue("@no_invoice", invoice.no_invoice);
                        command.Parameters.AddWithValue("@total_price", invoice.total_price);

                        result = command.ExecuteNonQuery() > 0 ? true : false;
                    }

                    if (result)
                    {
                        string query2 = "INSERT INTO invoice_details (id, id_invoice, id_course) VALUES (@id, @id_invoice, @id_course)";
                        using (MySqlCommand command = new MySqlCommand(query2, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@id", invoiceDetail.Id);
                            command.Parameters.AddWithValue("@id_invoice", invoiceDetail.id_invoice);
                            command.Parameters.AddWithValue("@id_course", invoiceDetail.id_course);

                            result = command.ExecuteNonQuery() > 0 ? true : false;
                        }
                    }

                    if (result)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
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



    }
}