using ClickandCollect.Models;
using System.Collections.Generic;

namespace ClickandCollect.DAL
{
    public interface IMarketsDAL
    {
        public List<Market> GetMarkets();
        public Market GetMarket(int id);
    }
}
