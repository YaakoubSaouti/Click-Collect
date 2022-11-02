using ClickandCollect.Models;
using System.Collections.Generic;

namespace ClickandCollect.DAL
{
    public interface IChoicesDAL
    {
        public List<Choice> GetChoices(Order order);
    }
}
