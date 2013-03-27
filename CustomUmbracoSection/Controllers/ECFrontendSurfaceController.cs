using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web;
using System.Web.Mvc;
using EventCalendar.Models;
using Umbraco.Core.Persistence;
using System.Globalization;

namespace EventCalendar.Controllers
{
    [PluginController("EventCalendar")]
    public class ECFrontendSurfaceController : SurfaceController
    {
        [ChildActionOnly]
        public ActionResult GetEventDetails(int id)
        {
            CalendarEntry e = ApplicationContext.DatabaseContext.Database.Single<CalendarEntry>("SELECT * FROM ec_events WHERE id=@0",id);
            EventDetailsModel evm = new EventDetailsModel()
            {
                Title = e.title,
                Description = e.description,
                Latitude = e.lat,
                Longitude = e.lon
            };
            if (null != e.start)
            {
                evm.StartDate = ((DateTime)e.start).ToString("F", CultureInfo.CurrentCulture);
            }
            if (null != e.end)
            {
                evm.EndDate = ((DateTime)e.end).ToString("F", CultureInfo.CurrentCulture);
            }
            return PartialView("EventDetails",evm);
        }
    }
}