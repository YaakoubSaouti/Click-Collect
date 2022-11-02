using ClickandCollect.Models;
using System.Data.SqlClient;

namespace ClickandCollect.DAL
{
    public class ClientDAL : IClientsDAL
    {
        private string connectionString;

        public ClientDAL(string connectionString) 
        {
            this.connectionString = connectionString;
        }

        public Client SignUp(string ln, string fn, string un,string pw) 
        {
            bool alreadyexist = false;
            using (SqlConnection connection = new SqlConnection(connectionString)) 
            {
                SqlCommand cmd = new SqlCommand("SELECT username FROM dbo.Person", connection);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) 
                    {
                        if (reader.GetString(0) == un) 
                            alreadyexist = true;
                    }
                }
            }
            if (alreadyexist == true)
                return null;
            else
                return new Client(ln, fn, un, pw);
        }

        public bool CreateAccount(Client c) 
        {
            bool success = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO dbo.Person(lastname,firstname,username,password,type) VALUES(@lastname,@firstname,@username,@password,@type)", connection);
                cmd.Parameters.AddWithValue("lastname", c.Lastname);
                cmd.Parameters.AddWithValue("firstname", c.Firstname);
                cmd.Parameters.AddWithValue("username", c.Username);
                cmd.Parameters.AddWithValue("password", c.Password);
                cmd.Parameters.AddWithValue("type", 1);
                connection.Open();
                int res = cmd.ExecuteNonQuery();
                success = res > 0;
            }
            int? id = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT id_person FROM dbo.Person WHERE username=@username", connection);
                cmd.Parameters.AddWithValue("username", c.Username);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32(0);
                    }
                }
            }
            if (id == null) return false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO dbo.Client(id_person) VALUES(@id_person)", connection);
                cmd.Parameters.AddWithValue("id_person", id);
                connection.Open();
                int res = cmd.ExecuteNonQuery();
                success = res > 0;
            }
            return success;
        }

        public Client GetClient(int id)
        {
            Client client = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Person WHERE id_person=@id", connection);
                cmd.Parameters.AddWithValue("id",id.ToString());
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        client = new Client(
                            reader.GetInt32(reader.GetOrdinal("id_person")),
                            reader.GetString(reader.GetOrdinal("firstname")),
                            reader.GetString(reader.GetOrdinal("lastname")),
                            reader.GetString(reader.GetOrdinal("username")),
                            reader.GetString(reader.GetOrdinal("password"))
                        );
                    }
                }
            }

            return client;
        }

    }
}
