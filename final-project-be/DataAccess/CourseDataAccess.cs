using final_project_be.Models;
using MySql.Data.MySqlClient;

namespace final_project_be.DataAccess
{
    public class CourseDataAccess
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public CourseDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        // get all
        public List<Course> GetAll()
        {
            List<Course> courses = new List<Course>();

            string query = "SELECT * FROM courses";

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
                                courses.Add(new Course
                                {
                                    Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                    Name = reader["Name"].ToString() ?? string.Empty,
                                    Price = int.Parse(reader["Price"].ToString() ?? string.Empty),
                                    TypeCourse = reader["TypeCourse"].ToString() ?? string.Empty,
                                    Img = reader["Img"].ToString() ?? string.Empty
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

            return courses;
        }

        // get by id
        public Course? GetById(Guid id)
        {
            Course? course = null;

            string query = $"SELECT * FROM courses WHERE Id = @id";

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
                                course = new Course
                                {
                                    Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                    Name = reader["Name"].ToString() ?? string.Empty,
                                    Price = int.Parse(reader["Price"].ToString() ?? string.Empty),
                                    TypeCourse = reader["TypeCourse"].ToString() ?? string.Empty,
                                    Img = reader["Img"].ToString() ?? string.Empty
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

            return course;

        }

        // get by name
        public Course? GetByName(string name)
        {
            Course? course = null;

            string query = $"SELECT * FROM courses WHERE Name = @name";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@name", name);

                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                course = new Course
                                {
                                    Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                    Name = reader["Name"].ToString() ?? string.Empty,
                                    Price = int.Parse(reader["Price"].ToString() ?? string.Empty),
                                    TypeCourse = reader["TypeCourse"].ToString() ?? string.Empty,
                                    Img = reader["Img"].ToString() ?? string.Empty
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

            return course;

        }

        // insert data
        public bool Insert(Course course)
        {
            bool result = false;
            string query = $"INSERT INTO courses (Id, Name, Price, TypeCourse, Img) VALUES (@id, @name, @price, @typeCourse, @img)";
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        command.Connection = connection;
                        command.Parameters.Clear();
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@id", course.Id);
                        command.Parameters.AddWithValue("@name", course.Name);
                        command.Parameters.AddWithValue("@price", course.Price);
                        command.Parameters.AddWithValue("@typeCourse", course.TypeCourse);
                        command.Parameters.AddWithValue("@img", course.Img);

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

        // update data
        public bool Update(Guid id, Course course)
        {
            bool result = false;
            string query = $"UPDATE courses SET Name = @name, Price = @price, TypeCourse = @typeCourse, Img = @img WHERE Id = @id";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    try
                    {
                        command.Connection = connection;
                        command.Parameters.Clear();
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@name", course.Name);
                        command.Parameters.AddWithValue("@price", course.Price);
                        command.Parameters.AddWithValue("@typeCourse", course.TypeCourse);
                        command.Parameters.AddWithValue("@img", course.Img);



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

        // delete data
        public bool Delete(Guid id)
        {
            bool result = false;

            string query = $"DELETE FROM courses WHERE Id = @id";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    try
                    {
                        command.Connection = connection;
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@id", id);


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