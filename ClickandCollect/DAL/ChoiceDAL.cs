using ClickandCollect.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ClickandCollect.DAL
{
    public class ChoiceDAL : IChoicesDAL
    {
        private string connectionString;

        public ChoiceDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Choice> GetChoices(Order order)
        {
            List<Choice> choices = new List<Choice>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Product INNER JOIN Choice ON Product.id_product=Choice.id_Product WHERE id_order=@id_order", connection);
                cmd.Parameters.AddWithValue("id_order", order.Id);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        choices.Add(
                            new Choice(
                                reader.GetInt32(reader.GetOrdinal("quantity")),
                                new Product(
                                    reader.GetInt32(reader.GetOrdinal("id_product")),
                                    reader.GetString(reader.GetOrdinal("name")),
                                    reader.GetString(reader.GetOrdinal("description")),
                                    reader.GetDouble(reader.GetOrdinal("price"))
                                )
                            )
                        );
                    }
                }
            }
            return choices;
        }
    }
}
