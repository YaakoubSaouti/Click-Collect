using ClickandCollect.Models;
using System.Collections.Generic;

namespace ClickandCollect.DAL
{
    public interface ICategorysDAL
    {
        public List<Category> GetCategories();

        public Category GetCategory(int id);
    }
}
