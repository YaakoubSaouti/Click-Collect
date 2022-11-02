using ClickandCollect.DAL;
using System;
using System.Collections.Generic;

namespace ClickandCollect.Models
{
    public class Market
    {
        private List<Timeslot> timeslots;

        public List<Timeslot> Timeslots 
        {
            get { return timeslots; }
            set { timeslots = value; }
        }

        private List<Order> orders;

        public List<Order> Orders
        {
            get { return orders; }
            set { orders = value; }
        }

        public Market() { }
        public Market(int id, string address, string locality, string postalCode) 
        {
            this.id = id;
            this.address = address;
            this.postalCode = postalCode;
            this.locality = locality;
            this.timeslots = new List<Timeslot>();
        }


        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string address;

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        private string locality;

        public string Locality
        {
            get { return locality; }
            set { locality = value; }
        }
        private string postalCode;
        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }

        public static List<Market> GetMarkets(IMarketsDAL marketsDAL) => marketsDAL.GetMarkets();

        public static Market GetMarket(int id, IMarketsDAL marketsDAL) => marketsDAL.GetMarket(id);

        public void GetTimeslots(string date, ITimeslotsDAL timeslotsDAL)
        {
            List<Timeslot> timeslots = Timeslot.GetTimeslots(this, date, timeslotsDAL);
            AddTimeslots(timeslots);
        }
        private void AddTimeslots(List<Timeslot> timeslots) => Timeslots = timeslots;
        public override string ToString() => $"{Address} | {PostalCode} | {Locality}";
        public void GetAllOrdersOfTheDay(IOrdersDAL ordersDAL)
        {
            List<Order> orders = Order.GetAllOrdersOfTheDay(this,ordersDAL);
            AddOrders(orders);
        }

        public void GetOrdersToMake(IOrdersDAL ordersDAL)
        {
            List<Order> orders = Order.GetOrdersToMake(this, ordersDAL);
            AddOrders(orders);
        }

        private void AddOrders(List<Order> orders) => Orders = orders;
    }
}
