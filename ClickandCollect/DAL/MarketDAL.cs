using ClickandCollect.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ClickandCollect.DAL
{
    public class MarketDAL : IMarketsDAL
    {



        private string connectionString;

        public MarketDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Market> GetMarkets()
        {
            List<Market> marketList = new List<Market>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Market", connection);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Market market = new Market(
                            reader.GetInt32(reader.GetOrdinal("id_market")),
                            reader.GetString(reader.GetOrdinal("address")),
                            reader.GetString(reader.GetOrdinal("pc")),
                            reader.GetString(reader.GetOrdinal("locality"))
                        );
                        marketList.Add(market);
                    }
                }
            }
            return marketList;
        }

        public Market GetMarket(int id)
        {
            Market market = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Market WHERE id_market=@id_market", connection);
                cmd.Parameters.AddWithValue("id_market", id);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        market = new Market(
                            reader.GetInt32(reader.GetOrdinal("id_market")),
                            reader.GetString(reader.GetOrdinal("address")),
                            reader.GetString(reader.GetOrdinal("pc")),
                            reader.GetString(reader.GetOrdinal("locality"))
                        );
                    }
                }
            }
            return market;
        }
 


    }
}
