using ClickandCollect.Models;
using System.Collections.Generic;

namespace ClickandCollect.DAL
{
    public interface ITimeslotsDAL
    {
        public List<Timeslot> GetTimeslots(Market market, string date);

        public Timeslot GetTimeslot(int id);
    }
}