using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.interfaces;
using umbraco.businesslogic;
using EventCalendar.Models;
using Umbraco.Web;
using System.Web.Mvc;
using Umbraco.Core;

namespace EventCalendar
{
    [Application("eventCalendar", "EventCalendar", "event_calendar.gif")]
    public class EventCalendarApp : IApplication
    {
        public static ECalendar GetCalendar(int id)
        {
            if (id == 0)
            {
                return null;
            }
            else
            {
                return ApplicationContext.Current.DatabaseContext.Database.SingleOrDefault<ECalendar>(id);
            }
        }
    }
}