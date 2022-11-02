using ClickandCollect.Models;
using System.ComponentModel.DataAnnotations;

namespace ClickandCollect.ViewModels
{
    public class ProductChoiceViewModel
    {
        public Product Product { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Please enter a valid number")]
        [Range(1, 20, ErrorMessage = "The quantity should between 1 and 20")]
        public int Quantity { get; set; }
    }
}
