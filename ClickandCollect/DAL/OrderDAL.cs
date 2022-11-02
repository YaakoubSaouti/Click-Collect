using ClickandCollect.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ClickandCollect.DAL
{
    public class OrderDAL : IOrdersDAL
    {

        private string connectionString;

        public OrderDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool CreateOrder(Order order)
        {
            bool success = false;
            int last_id = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO dbo.OrderSale(id_client,id_timeslot,price,number_of_boxes,state,order_date) VALUES(@id_client,@id_timeslot,@price,@number_of_boxes,@state,@order_date); SELECT SCOPE_IDENTITY()", connection);
                cmd.Parameters.AddWithValue("id_client", order.Client.Id);
                cmd.Parameters.AddWithValue("id_timeslot", order.Timeslot.Id);
                cmd.Parameters.AddWithValue("price", order.Price());
                cmd.Parameters.AddWithValue("number_of_boxes", order.NumberOfBoxes);
                cmd.Parameters.AddWithValue("state", order.State);
                cmd.Parameters.AddWithValue("order_date", order.Date);
                connection.Open();
                last_id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            foreach (Choice c in order.Choices) 
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO dbo.Choice(id_product,id_order,quantity,price) VALUES(@id_product,@id_order,@quantity,@price)", connection);
                    cmd.Parameters.AddWithValue("id_product", c.Product.Id);
                    cmd.Parameters.AddWithValue("id_order", last_id);
                    cmd.Parameters.AddWithValue("quantity", c.Quantity);
                    cmd.Parameters.AddWithValue("price", c.Price());
                    connection.Open();
                    int res = cmd.ExecuteNonQuery();
                    success = res > 0;
                }
            }
            return success;
        }

        public List<Order> GetAllOrdersOfTheDay(Market market)
        {
            List<Order> Orders= new List<Order>();
            DateTime now = DateTime.Today;
            string nowstring=now.ToString("yyyy-MM-dd");
            List<int> ids= new List<int>();
            List<Timeslot> timeslots = new List<Timeslot>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand("SELECT * FROM OrderSale INNER JOIN Timeslot ON OrderSale.id_timeslot = Timeslot.id_timeslot WHERE id_market=@id_market AND order_date=@order_date AND state=@state", connection);
                cmd.Parameters.AddWithValue("id_market", market.Id);
                cmd.Parameters.AddWithValue("order_date", nowstring);
                cmd.Parameters.AddWithValue("state", 2);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ids.Add(reader.GetInt32(reader.GetOrdinal("id_order")));
                        timeslots.Add(
                            new Timeslot(
                                reader.GetInt32(reader.GetOrdinal("id_timeslot")),
                                reader.GetString(reader.GetOrdinal("timeslot_hb")),
                                reader.GetString(reader.GetOrdinal("timeslot_he"))
                            )
                        );
                    }
                }
            }
            int i = 0;
            foreach (int id in ids) 
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("SELECT * FROM OrderSale,Person WHERE id_order=@id_order AND Person.id_person = OrderSale.id_client", connection);
                    cmd.Parameters.AddWithValue("id_order", id);
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order o = new Order(
                                new Client
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id_person")),
                                    Firstname = reader.GetString(reader.GetOrdinal("firstname")),
                                    Lastname = reader.GetString(reader.GetOrdinal("lastname"))
                                },
                                timeslots[i]
                            );
                            o.Id = reader.GetInt32(reader.GetOrdinal("id_order"));
                            o.Date = reader.GetDateTime(reader.GetOrdinal("order_date")).ToString("yyyy-MM-dd");
                            o.Market = market;
                            o.NumberOfBoxes = reader.GetInt32(reader.GetOrdinal("number_of_boxes"));
                            o.State = reader.GetInt32(reader.GetOrdinal("state"));
                            Orders.Add(o);
                        }
                    }
                }
                i++;
            }
            return Orders;
        }

        public List<Order> GetOrdersToMake(Market market)
        {
            List<Order> Orders = new List<Order>();
            DateTime now = DateTime.Today;
            DateTime tomorrow = now.AddDays(1);
            string tomorrowstring = tomorrow.ToString("yyyy-MM-dd");
            List<int> ids = new List<int>();
            List<Timeslot> timeslots = new List<Timeslot>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand("SELECT * FROM OrderSale INNER JOIN Timeslot ON OrderSale.id_timeslot = Timeslot.id_timeslot WHERE id_market=@id_market AND order_date=@order_date AND state=@state", connection);
                cmd.Parameters.AddWithValue("id_market", market.Id);
                cmd.Parameters.AddWithValue("order_date", tomorrow);
                cmd.Parameters.AddWithValue("state", 1);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ids.Add(reader.GetInt32(reader.GetOrdinal("id_order")));
                        timeslots.Add(
                            new Timeslot(
                                reader.GetInt32(reader.GetOrdinal("id_timeslot")),
                                reader.GetString(reader.GetOrdinal("timeslot_hb")),
                                reader.GetString(reader.GetOrdinal("timeslot_he"))
                            )
                        );
                    }
                }
            }
            int i = 0;
            foreach (int id in ids)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("SELECT * FROM OrderSale,Person WHERE id_order=@id_order AND Person.id_person = OrderSale.id_client", connection);
                    cmd.Parameters.AddWithValue("id_order", id);
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order o = new Order(
                                new Client
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id_person")),
                                    Firstname = reader.GetString(reader.GetOrdinal("firstname")),
                                    Lastname = reader.GetString(reader.GetOrdinal("lastname"))
                                },
                                timeslots[i]
                            );
                            o.Id = reader.GetInt32(reader.GetOrdinal("id_order"));
                            o.Date = reader.GetDateTime(reader.GetOrdinal("order_date")).ToString("yyyy-MM-dd");
                            o.Market = market;
                            o.NumberOfBoxes = reader.GetInt32(reader.GetOrdinal("number_of_boxes"));
                            o.State = reader.GetInt32(reader.GetOrdinal("state"));
                            Orders.Add(o);
                        }
                    }
                }
                i++;
            }
            return Orders;
        }

        public Order GetOrder(int id) 
        {
            Order order = null;
            Timeslot timeslot = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand("SELECT * FROM OrderSale INNER JOIN Timeslot ON OrderSale.id_timeslot = Timeslot.id_timeslot WHERE id_order=@id_order", connection);
                cmd.Parameters.AddWithValue("id_order", id);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        timeslot = new Timeslot(
                                reader.GetInt32(reader.GetOrdinal("id_timeslot")),
                                reader.GetString(reader.GetOrdinal("timeslot_hb")),
                                reader.GetString(reader.GetOrdinal("timeslot_he"))
                        );
                    }
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand("SELECT * FROM OrderSale,Person WHERE id_order=@id_order AND Person.id_person = OrderSale.id_client", connection);
                cmd.Parameters.AddWithValue("id_order", id);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        order = new Order(
                            new Client
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id_person")),
                                Firstname = reader.GetString(reader.GetOrdinal("firstname")),
                                Lastname = reader.GetString(reader.GetOrdinal("lastname"))
                            },
                            timeslot
                        );
                        order.Id = reader.GetInt32(reader.GetOrdinal("id_order"));
                        order.Date = reader.GetDateTime(reader.GetOrdinal("order_date")).ToString("yyyy-MM-dd");
                        order.NumberOfBoxes = reader.GetInt32(reader.GetOrdinal("number_of_boxes"));
                        order.State = reader.GetInt32(reader.GetOrdinal("state"));
                    }
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand("SELECT * FROM Market INNER JOIN Timeslot ON Market.id_market = Timeslot.id_market WHERE id_timeslot=@id_timeslot", connection);
                cmd.Parameters.AddWithValue("id_timeslot", order.Timeslot.Id);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        order.Market = new Market(
                            reader.GetInt32(reader.GetOrdinal("id_market")),
                            reader.GetString(reader.GetOrdinal("address")),
                            reader.GetString(reader.GetOrdinal("locality")),
                            reader.GetString(reader.GetOrdinal("pc"))
                        );
                    }
                }
            }

            return order;

        }

        public bool SaveOrder(Order order) 
        {
            bool success = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE OrderSale SET price = @price, number_of_boxes=@number_of_boxes,state=@state WHERE id_order=@id_order", connection);
                cmd.Parameters.AddWithValue("id_order", order.Id);
                cmd.Parameters.AddWithValue("price", order.Price());
                cmd.Parameters.AddWithValue("number_of_boxes", order.NumberOfBoxes);
                cmd.Parameters.AddWithValue("state", order.State);
                cmd.Connection.Open();
                int res = cmd.ExecuteNonQuery();
                success = res > 0;
            }
            return success;
        }
    }
}
