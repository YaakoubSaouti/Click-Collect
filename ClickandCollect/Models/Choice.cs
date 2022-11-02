using ClickandCollect.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClickandCollect.Models
{
    public class Choice 
    {

        private Product product;

        public Product Product 
        {
            get { return product; }
            set { product = value; }
        }
        private int quantity;

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Please enter a valid number")]
        [Range(1, 20, ErrorMessage = "The quantity should between 1 and 20")]
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        public Choice(){}
        public Choice(int quantity,Product p)
        {
            Quantity = quantity;
            Product = p;
        }

        public decimal Price() => (decimal)product.Price * quantity;

        public static List<Choice> GetChoices(Order order, IChoicesDAL choicesDAL) 
        {
            return choicesDAL.GetChoices(order);
        }
    }
}
