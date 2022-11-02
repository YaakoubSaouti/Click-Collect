using ClickandCollect.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ClickandCollect.ViewModels
{
    public class TimeslotSelectViewModel
    {
        public string TimeslotSelect { get; set; }

        public List<SelectListItem> TimeslotsSelect { get; } = new List<SelectListItem>();

        public void FillTheSelect(List<Timeslot> timeslots) 
        {
            foreach (Timeslot t in timeslots) 
            {
                TimeslotsSelect.Add(new SelectListItem { Value = (t.Id).ToString(), Text = t.ToString() });
            }
        } 
    }
}
