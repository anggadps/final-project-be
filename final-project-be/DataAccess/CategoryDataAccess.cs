﻿using final_project_be.Models;
using MySql.Data.MySqlClient;
using static System.Reflection.Metadata.BlobBuilder;

namespace final_project_be.DataAccess
{
    public class CategoryDataAccess
    {
        private readonly string _connectionString = "server=localhost;port=3306;database=final-project;user=root;password=";


        /*Get all data*/
        public List<Category> GetAll()
        {
            List<Category> categories = new List<Category>();

            string query = "SELECT * FROM categories";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new Category
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Img = reader["Img"].ToString() ?? string.Empty,
                                Description = reader["Description"].ToString() ?? string.Empty
                            });
                        }
                    }

                    connection.Close();

                }
            }

            return categories;
        }


        /*Get by ID*/
        public Category? GetById(Guid id)
        {
            Category? category = null;

            string query = $"SELECT * FROM categories WHERE Id = '{id}'";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            category = new Category
                            {
                                Id = Guid.Parse(reader["Id"].ToString() ?? string.Empty),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                Img = reader["Img"].ToString() ?? string.Empty,
                                Description = reader["Description"].ToString() ?? string.Empty
                            };
                        }
                    }

                    connection.Close();
                }
            }

            return category;
        }

        /*insert data*/
        public bool Insert(Category category)
        {
            bool result = false;

            string query = $"INSERT INTO categories(id, name, img, description) " +
               $"VALUES ('{category.Id}','{category.Name}', '{category.Img}', '{category.Description}')";

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

        /*update data*/
        public bool Update(Guid id, Category category)
        {
            bool result = false;

            

            string query = $"UPDATE categories SET name = '{category.Name}', img = '{category.Img}', description = '{category.Description}' " +
                $"WHERE id = '{id}'";

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

        /*delete data*/
        public bool Delete(Guid id)
        {
            bool result = false;

            string query = $"DELETE FROM categories WHERE id = '{id}'";

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
