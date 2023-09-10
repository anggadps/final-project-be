using final_project_be.Models;
using MySql.Data.MySqlClient;

namespace final_project_be.DataAccess
{
    public class CourseDataAccess
    {
        private readonly string _connectionString = "server=localhost;port=3306;database=final-project;user=root;password=";
        // get all
        public List<Course> GetAll()
        {
            List<Course> courses = new List<Course>();

            string query = "SELECT * FROM courses";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
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

                    connection.Close();

                }
            }

            return courses;
        }

        // get by id
        public Course? GetById(Guid id)
        {
            Course? course = null;

            string query = $"SELECT * FROM courses WHERE Id = '{id}'";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
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

                    connection.Close();

                }
            }

            return course;
        }

        // insert data
        public bool Insert(Course course)
        {
            bool result = false;
            string query = $"INSERT INTO courses (Id, Name, Price, TypeCourse, Img) VALUES ('{course.Id}', '{course.Name}', '{course.Price}', '{course.TypeCourse}', '{course.Img}')";
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Connection = connection;
                    command.CommandText = query;
                    connection.Open();
                    result = command.ExecuteNonQuery() > 0 ? true : false;
                    connection.Close();
                }
            }
            return result;
        }

        // update data
        public bool Update(Guid id, Course course)
        {
            bool result = false;
            string query = $"UPDATE courses SET Name = '{course.Name}', Price = '{course.Price}', TypeCourse = '{course.TypeCourse}', Img = '{course.Img}' WHERE Id = '{id}'";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }

        // delete data
        public bool Delete(Guid id)
        {
            bool result = false;

            string query = $"DELETE FROM courses WHERE Id = '{id}'";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }
    }
}