using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventCalendar.Models
{
    public class EventDetailsModel
    {
        public string CalendarName { get; set; }
        public string Title { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}