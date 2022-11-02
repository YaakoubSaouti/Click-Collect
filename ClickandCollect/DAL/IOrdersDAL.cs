using ClickandCollect.Models;
using System.Collections.Generic;

namespace ClickandCollect.DAL
{
    public interface IOrdersDAL
    {
        public bool CreateOrder(Order d);

        public List<Order> GetAllOrdersOfTheDay(Market market);

        public List<Order> GetOrdersToMake(Market market);

        public Order GetOrder(int id);

        public bool SaveOrder(Order order);

    }
}
