using ClickandCollect.DAL;

namespace ClickandCollect.Models
{
    public class Client : Person
    {
        public Client() { }
        public Client(int id, string ln, string fn, string un, string pw) : base(id,ln,fn,un,pw) { }

        public Client(string ln, string fn, string un, string pw) : base(ln, fn, un, pw) { }
        
        public static Client SignUp(string ln,string fn,string un,string pw,IClientsDAL clientsDAL)
        {
            return clientsDAL.SignUp(ln,fn,un,pw);
        }

        public bool CreateAccount(IClientsDAL clientsDAL)
        {
            return clientsDAL.CreateAccount(this);
        }

        public static Client GetClient(int id, IClientsDAL clientsDAL)
        {
            return clientsDAL.GetClient(id);
        }

    }
}
