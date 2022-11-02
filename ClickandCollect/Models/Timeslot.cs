using ClickandCollect.DAL;
using System;
using System.Collections.Generic;

namespace ClickandCollect.Models
{
    public class Timeslot
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string hourBeg;

        public string HourBeg
        {
            get { return hourBeg; }
            set { hourBeg = value; }
        }

        private string hourEnd;

        public string HourEnd
        {
            get { return hourEnd; }
            set { hourEnd = value; }
        }

        public Timeslot(){}

        public Timeslot(int id, string hourBeg, string hourEnd) 
        {
            this.id = id;
            this.hourBeg = hourBeg;
            this.hourEnd = hourEnd;
        }

        public override string ToString() => $"Between {HourBeg} and {HourEnd}";
        public static List<Timeslot> GetTimeslots(Market market,string date, ITimeslotsDAL timeslotsDAL) => timeslotsDAL.GetTimeslots(market,date);
        public static Timeslot GetTimeslot(int id, ITimeslotsDAL timeslotsDAL) => timeslotsDAL.GetTimeslot(id);
    }
}
