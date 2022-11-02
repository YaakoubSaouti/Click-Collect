using ClickandCollect.Models;

namespace ClickandCollect.DAL
{
    public interface IPersonsDAL
    {
        public Person LogIn(string un,string pw);
    }
}
