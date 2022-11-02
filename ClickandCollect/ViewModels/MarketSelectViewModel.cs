using ClickandCollect.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ClickandCollect.ViewModels
{
    public class MarketSelectViewModel
    {
        public string MarketSelect { get; set; }

        public List<SelectListItem> MarketsSelect { get; } = new List<SelectListItem>();

        public void FillTheSelect(List<Market> markets) 
        {
            foreach (Market m in markets) 
            {
                MarketsSelect.Add(new SelectListItem { Value = (m.Id).ToString(), Text = m.ToString() });
            }
        } 
    }
}
