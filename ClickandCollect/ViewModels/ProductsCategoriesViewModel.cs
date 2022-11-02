using ClickandCollect.Models;
using System.Collections.Generic;

namespace ClickandCollect.ViewModels
{
    public class ProductsCategoriesViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
    }
}
