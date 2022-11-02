using ClickandCollect.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ClickandCollect.DAL
{
    public class ProductDAL : IProductsDAL
    {
        private string connectionString;

        public ProductDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Product", connection);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product product = new Product(
                            reader.GetInt32(reader.GetOrdinal("id_product")),
                            reader.GetString(reader.GetOrdinal("name")), 
                            reader.GetString(reader.GetOrdinal("description")), 
                            reader.GetDouble(reader.GetOrdinal("price"))
                        );
                        products.Add(product);
                    }
                }
            }
            return products;
        }

        public List<Product> GetProducts(Category category)
        {
            List<Product> products = new List<Product>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Product LEFT JOIN ProductCategory ON Product.id_product = ProductCategory.id_product WHERE ProductCategory.id_category=@id_category", connection);
                cmd.Parameters.AddWithValue("id_category", category.Id);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product product = new Product(
                            reader.GetInt32(reader.GetOrdinal("id_product")),
                            reader.GetString(reader.GetOrdinal("name")),
                            reader.GetString(reader.GetOrdinal("description")),
                            reader.GetDouble(reader.GetOrdinal("price"))
                        );
                        products.Add(product);
                    }
                }
            }
            return products;
        }

        public Product GetProduct(int id)
        {
            Product product = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Product WHERE id_product=@id_product", connection);
                cmd.Parameters.AddWithValue("id_product", id);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        product = new Product(
                            reader.GetInt32(reader.GetOrdinal("id_product")),
                            reader.GetString(reader.GetOrdinal("name")),
                            reader.GetString(reader.GetOrdinal("description")),
                            reader.GetDouble(reader.GetOrdinal("price"))
                        );
                    }
                }
            }
            return product;
        }
    }
}
