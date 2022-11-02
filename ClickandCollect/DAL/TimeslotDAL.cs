using ClickandCollect.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ClickandCollect.DAL
{
    public class TimeslotDAL : ITimeslotsDAL
    {
        private string connectionString;

        public TimeslotDAL(string connectionString) 
        {
            this.connectionString = connectionString;
        }

        public List<Timeslot> GetTimeslots(Market m,string date)
        {
            List<Timeslot> timeslots = new List<Timeslot>();
            using (SqlConnection connection = new SqlConnection(connectionString)) 
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Timeslot WHERE id_market=@id_market", connection);
                connection.Open();
                cmd.Parameters.AddWithValue("id_market", m.Id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) 
                    {
                        Timeslot timeslot = new Timeslot(
                            reader.GetInt32(reader.GetOrdinal("id_timeslot")),
                            reader.GetString(reader.GetOrdinal("timeslot_hb")),
                            reader.GetString(reader.GetOrdinal("timeslot_he"))
                        );
                        timeslots.Add(timeslot);
                    }
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT id_timeslot,Count(id_timeslot) AS order_count From OrderSale WHERE order_date=@date GROUP BY id_timeslot", connection);
                connection.Open();
                cmd.Parameters.AddWithValue("date", date);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetInt32(reader.GetOrdinal("order_count"))>=10) 
                        {
                            for (int i = 0; i < timeslots.Count; i++)
                            {
                                if (timeslots[i].Id == reader.GetInt32(reader.GetOrdinal("id_timeslot"))) timeslots.RemoveAt(i);
                            }
                        }
                    }
                }
            }
            return timeslots;
        }

        public Timeslot GetTimeslot(int id)
        {
            Timeslot timeslot = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Timeslot WHERE id_timeslot=@id_timeslot", connection);
                connection.Open();
                cmd.Parameters.AddWithValue("id_timeslot", id);
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
            return timeslot;
        }
    }
}
