using ClickandCollect.DAL;
using System.Collections.Generic;

namespace ClickandCollect.Models
{
    public class Product
    {
        private int id;
        public int Id
        { 
            get { return id; }
            set { id = value; }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private double price;
        public double Price
        {
            get { return price; }
            set { price = value; }
        }
        public Product(){}
        public Product(int id,string name, string description, double price)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.price = price;
        }
        public static List<Product> GetProducts(IProductsDAL productsDAL) => productsDAL.GetProducts();

        public static List<Product> GetProducts(Category category,IProductsDAL productsDAL) => productsDAL.GetProducts(category);

        public static Product GetProduct(int id,IProductsDAL productsDAL) => productsDAL.GetProduct(id);
    }
}
