using System;
using System.ComponentModel.DataAnnotations;

namespace ClickandCollect.ViewModels
{
    public class DateViewModel
    {
            
            [DataType(DataType.Date)]
            
            public string Date { get; set; }
    }
}
