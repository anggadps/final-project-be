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

            string query = "SELECT courses.id, courses.id_category, courses.name, courses.price, courses.img, categories.name AS category_name, courses.description, courses.is_active " +
                "FROM courses " +
                "INNER JOIN categories " +
                "ON courses.id_category = categories.id WHERE courses.is_active = true ";

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
                                    Id = Guid.Parse(reader["id"].ToString() ?? string.Empty),
                                    Name = reader["name"].ToString() ?? string.Empty,
                                    Price = int.Parse(reader["price"].ToString() ?? string.Empty),
                                    id_category = reader["id_category"].ToString() ?? string.Empty,
                                    Category_name = reader["category_name"].ToString() ?? string.Empty,
                                    Img = reader["img"].ToString() ?? string.Empty,
                                    Description = reader["description"].ToString() ?? string.Empty,
                                    Is_active = reader.GetBoolean("Is_active")

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


        // get all
        public List<Course> GetAllByAdmin()
        {
            List<Course> courses = new List<Course>();

            string query = "SELECT courses.id, courses.id_category, courses.name, courses.price, courses.img, categories.name AS category_name, courses.description, courses.is_active " +
                "FROM courses " +
                "INNER JOIN categories " +
                "ON courses.id_category = categories.id ";

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
                                    Id = Guid.Parse(reader["id"].ToString() ?? string.Empty),
                                    Name = reader["name"].ToString() ?? string.Empty,
                                    Price = int.Parse(reader["price"].ToString() ?? string.Empty),
                                    id_category = reader["id_category"].ToString() ?? string.Empty,
                                    Category_name = reader["category_name"].ToString() ?? string.Empty,
                                    Img = reader["img"].ToString() ?? string.Empty,
                                    Description = reader["description"].ToString() ?? string.Empty,
                                    Is_active = reader.GetBoolean("Is_active")

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

        // get by id_category
        public List<CourseByCategory> GetByIdCategory(Guid id)
        {
            List<CourseByCategory> coursesByCategory = new List<CourseByCategory>();

            string query = $"SELECT * FROM courses INNER JOIN categories ON courses.id_category = categories.id WHERE courses.id_category = @id";

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
                                CourseByCategory courseByCategory = new CourseByCategory
                                {
                                    Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                    Name = reader["name"].ToString() ?? string.Empty,
                                    Price = int.Parse(reader["price"].ToString() ?? "0"),
                                    Img = reader["img"].ToString() ?? string.Empty
                                };

                                coursesByCategory.Add(courseByCategory);
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

            return coursesByCategory;
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
                                    id_category = reader["id_category"].ToString() ?? string.Empty,
                                    Img = reader["Img"].ToString() ?? string.Empty,
                                    Description = reader["Description"].ToString() ?? string.Empty,
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
                                    id_category = reader["id_category"].ToString() ?? string.Empty,
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
            string query = $"INSERT INTO courses (Id, Name, Price, id_category, Img, Description, Is_active) VALUES (@id, @name, @price, @id_category, @img, @description, @is_active)";
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
                        command.Parameters.AddWithValue("@id_category", course.id_category);
                        command.Parameters.AddWithValue("@img", course.Img);
                        command.Parameters.AddWithValue("@description", course.Description);
                        command.Parameters.AddWithValue("@is_active", course.Is_active);

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
            string query = $"UPDATE courses SET Name = @name, Price = @price, id_category = @id_category, Img = @img, Description = @description, Is_active = @is_active WHERE Id = @id";

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
                        command.Parameters.AddWithValue("@id_category", course.id_category);
                        command.Parameters.AddWithValue("@img", course.Img);
                        command.Parameters.AddWithValue("@description", course.Description);
                        command.Parameters.AddWithValue("@is_active", course.Is_active);



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