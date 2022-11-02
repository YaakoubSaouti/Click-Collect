using ClickandCollect.Models;
using System;
using System.Data.SqlClient;

namespace ClickandCollect.DAL
{
    public class PersonDAL : IPersonsDAL
    {
        private string connectionString;

        public PersonDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Person LogIn(string un,string pw)
        {
            Person p = null;
            string[] array = new string[6];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand("SELECT id_person,firstname,lastname,username,password,type FROM dbo.Person WHERE username=@username AND password=@password", connection);
                cmd.Parameters.AddWithValue("username", un);
                cmd.Parameters.AddWithValue("password", pw);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetInt32(5) == 1)
                        {
                            p = new Client(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
                        }
                        else if (reader.GetInt32(5) == 2)
                        {
                            array[0] = (reader.GetInt32(0)).ToString();
                            for (int i = 1; i < array.Length-1; i++)
                                array[i]=reader.GetString(i);
                            array[5] = (reader.GetInt32(5)).ToString();
                        }
                        else 
                        {
                            array[0] = (reader.GetInt32(0)).ToString();
                            for (int i = 1; i < array.Length - 1; i++)
                                array[i] = reader.GetString(i);
                            array[5] = (reader.GetInt32(5)).ToString();
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(array[5])) 
            {
                if (array[5] == "2")
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {

                        SqlCommand cmd = new SqlCommand("SELECT * FROM Market LEFT JOIN Cashier ON Market.id_market = Cashier.id_market WHERE id_person=@id_person", connection);
                        cmd.Parameters.AddWithValue("id_person", array[0]);
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                p = new Cashier(
                                    Convert.ToInt32(array[0]),
                                    array[1],
                                    array[2],
                                    array[3],
                                    array[4],
                                    new Market( 
                                        reader.GetInt32(reader.GetOrdinal("id_market")), 
                                        reader.GetString(reader.GetOrdinal("address")),
                                        reader.GetString(reader.GetOrdinal("locality")),
                                        reader.GetString(reader.GetOrdinal("pc"))
                                    )
                                );
                            }
                        }
                    }
                }
                if (array[5] == "3")
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {

                        SqlCommand cmd = new SqlCommand("SELECT * FROM Market LEFT JOIN OrderPicker ON Market.id_market = OrderPicker.id_market WHERE id_person=@id_person", connection);
                        cmd.Parameters.AddWithValue("id_person", array[0]);
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                p = new OrderPicker(
                                    Convert.ToInt32(array[0]),
                                    array[1],
                                    array[2],
                                    array[3],
                                    array[4],
                                    new Market(
                                        reader.GetInt32(reader.GetOrdinal("id_market")),
                                        reader.GetString(reader.GetOrdinal("address")),
                                        reader.GetString(reader.GetOrdinal("locality")),
                                        reader.GetString(reader.GetOrdinal("pc"))
                                    )
                                );
                            }
                        }
                    }
                }
            }

            return p;
        }
    }
}
