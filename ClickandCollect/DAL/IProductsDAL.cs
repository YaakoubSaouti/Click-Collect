using ClickandCollect.Models;
using System.Collections.Generic;

namespace ClickandCollect.DAL
{
    public interface IProductsDAL
    {
        public List<Product> GetProducts();

        public List<Product> GetProducts(Category category);

        public Product GetProduct(int id);
    }
}
