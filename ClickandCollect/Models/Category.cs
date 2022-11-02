using ClickandCollect.DAL;
using System.Collections.Generic;

namespace ClickandCollect.Models
{
    public class Category
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
        public Category(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        List<Product> products;
        public List<Product> Products
        {
            get { return products; }
            set { products = value; }
        }

        public void GetProducts(IProductsDAL productsDAL) 
        {
            products = Product.GetProducts(this,productsDAL);
        }
        public static List<Category> GetCategories(ICategorysDAL categorysDAL) => categorysDAL.GetCategories();
        public static Category GetCategory(int id,ICategorysDAL categorysDAL) => categorysDAL.GetCategory(id);
    }
}
