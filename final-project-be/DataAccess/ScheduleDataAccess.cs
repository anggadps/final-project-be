using final_project_be.Models;
using MySql.Data.MySqlClient;

namespace final_project_be.DataAccess
{
    public class ScheduleDataAccess
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public ScheduleDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }


        // get schedule by id course
        public List<Schedule> GetById(Guid id)
        {
            List<Schedule> schedule = new List<Schedule>();

            string query = $"SELECT * FROM schedules WHERE id_course = @id";

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
                                schedule.Add(new Schedule
                                {
                                    Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                    Id_course = reader["id_course"].ToString() ?? string.Empty,
                                    Schedule_date = Convert.ToDateTime(reader["schedule_date"])
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

            return schedule;
        }




        public bool AddSchedule(Schedule schedule)
        {
            bool result = false;
            string query = $"INSERT INTO schedules (Id, Id_course, Schedule_date) VALUES (@id, @id_course, @schedule_date)";
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        command.Connection = connection;
                        command.Parameters.Clear();
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@id", schedule.Id);
                        command.Parameters.AddWithValue("@id_course", schedule.Id_course);
                        command.Parameters.AddWithValue("@schedule_date", schedule.Schedule_date);

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
