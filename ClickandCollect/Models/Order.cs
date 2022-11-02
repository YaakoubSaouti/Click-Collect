using ClickandCollect.DAL;
using System;
using System.Collections.Generic;

namespace ClickandCollect.Models
{
    public class Order
    {
        private Timeslot timeslot;

        public Timeslot Timeslot
        {
            get { return timeslot; }
            set { timeslot = value; }
        }

        public Order(Client client, Timeslot timeslot) 
        {
            this.timeslot = timeslot;
            this.client = client;
        }

        public Order(Client client)
        {
            this.client = client;
        }
        public Order() { }

        private Client client;
        public Client Client 
        {
            get { return client; }
            set { client = value; }
        }

        private List<Choice> choices = new List<Choice>();

        private Market market;

        public Market Market
        {
            get { return market; }
            set { market = value; }
        }

        public List<Choice> Choices
        {
            get { return choices; }
            set { choices = value; }
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        
        private int numberOfBoxes=0;

        public int NumberOfBoxes
        {
            get { return numberOfBoxes; }
            set { numberOfBoxes = value; }
        }

        private int state = 1;

        public int State
        {
            get { return state; }
            set { state = value; }
        }


        private string date;

        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        public void GetChoices(IChoicesDAL choiceDAL)
        {
            List<Choice> choices = Choice.GetChoices(this, choiceDAL);
            AddChoices(choices);
        }

        private void AddChoices(List<Choice> c) => Choices = c;

        public void AddChoice(Choice c) 
        {
            bool doesntexist = true;
            foreach (Choice choice in choices) 
            {
                if (c.Product.Id == choice.Product.Id) 
                {
                    choice.Quantity += c.Quantity;
                    doesntexist = false;
                }
            }
            if(doesntexist)choices.Add(c);
        }
        public void RemoveAChoiceByProductId(int id)
        {
            for (int i=0;i<choices.Count;i++)
            {
                if(id == choices[i].Product.Id)
                  choices.RemoveAt(i);    
            }         
        }

        public decimal Price() 
        {
            decimal price = 0;
            foreach (Choice choice in choices) 
            {
                price += choice.Price();
            }

            price += (decimal)5.95;

            price += (decimal)(numberOfBoxes * 5.95);

            return price;
        }

        public bool CreateOrder(IOrdersDAL ordersDAL)
        {
            return ordersDAL.CreateOrder(this);
        }
        public bool SaveOrder(IOrdersDAL ordersDAL)
        {
            return ordersDAL.SaveOrder(this);
        }
        public static List<Order> GetAllOrdersOfTheDay(Market market,IOrdersDAL ordersDAL) => ordersDAL.GetAllOrdersOfTheDay(market);

        public static List<Order> GetOrdersToMake(Market market, IOrdersDAL ordersDAL) => ordersDAL.GetOrdersToMake(market);

        public static Order GetOrder(int id, IOrdersDAL ordersDAL) => ordersDAL.GetOrder(id);
    }
}
