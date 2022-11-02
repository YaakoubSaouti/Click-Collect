using ClickandCollect.Models;
using System.Collections.Generic;
namespace ClickandCollect.ViewModels
{
    public class ProductCategoryViewModel
    {
        public List<Product> listProducts { get; set; }
        public List<Category> categories { get; set; }
    }
}
