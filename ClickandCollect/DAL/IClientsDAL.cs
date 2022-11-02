using ClickandCollect.Models;

namespace ClickandCollect.DAL
{
    public interface IClientsDAL
    {
        public Client SignUp(string ln, string fn, string un, string pw);

        public bool CreateAccount(Client c);

        public Client GetClient(int id);
    }
}
