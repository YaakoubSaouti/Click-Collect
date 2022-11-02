using ClickandCollect.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ClickandCollect.DAL
{
    public class CategoryDAL : ICategorysDAL
    {
        private string connectionString;

        public CategoryDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Category", connection);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Category category = new Category(
                            reader.GetInt32(reader.GetOrdinal("id_category")),
                            reader.GetString(reader.GetOrdinal("name"))
                        );
                        categories.Add(category);
                    }
                }
            }
            return categories;
        }

        public Category GetCategory(int id)
        {
            Category category= null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Category WHERE id_category=@id_category", connection);
                cmd.Parameters.AddWithValue("id_category", id);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        category = new Category(
                            reader.GetInt32(reader.GetOrdinal("id_category")),
                            reader.GetString(reader.GetOrdinal("name"))
                        );
                    }
                }
            }
            return category;
        }
    }
}
