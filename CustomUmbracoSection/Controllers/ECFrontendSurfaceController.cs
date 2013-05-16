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
using ScheduleWidget.Enums;
using ScheduleWidget.ScheduledEvents;

namespace EventCalendar.Controllers
{
    [PluginController("EventCalendar")]
    public class ECFrontendSurfaceController : SurfaceController
    {
        UmbracoDatabase _db;

        public ECFrontendSurfaceController()
        {
            this._db = ApplicationContext.DatabaseContext.Database;
        }

        [ChildActionOnly]
        public ActionResult GetEventDetails(int id, int calendar, int type = 0)
        {
            EventLocation l = null;
            EventDetailsModel evm = null;

            if (type == 0)
            {
                //Fetch Event
                CalendarEntry e = this._db.Single<CalendarEntry>("SELECT * FROM ec_events WHERE id=@0 AND calendarid = @1", id, calendar);
                //If it has a location fetch it
                if (e.locationId != 0)
                {
                    l = this._db.Single<EventLocation>("SELECT * FROM ec_locations WHERE id = @0", e.locationId);
                }
                //Fetch all Descriptions
                Dictionary<string, EventDesciption> descriptions = this._db.Query<EventDesciption>("SELECT * FROM ec_eventdescriptions WHERE eventid = @0 AND type = 0 AND calendarid = @1", id, calendar).ToDictionary(x => x.CultureCode);

                evm = new EventDetailsModel()
                {
                    Title = e.title,
                    LocationId = e.locationId,
                    Location = l,
                    Descriptions = descriptions
                };
                if (null != e.start)
                {
                    evm.StartDate = ((DateTime)e.start).ToString("F", CultureInfo.CurrentCulture);
                }
                if (null != e.end)
                {
                    evm.EndDate = ((DateTime)e.end).ToString("F", CultureInfo.CurrentCulture);
                }
            }
            else if (type == 1)
            {
                RecurringEvent e = ApplicationContext.DatabaseContext.Database.Single<RecurringEvent>("SELECT * FROM ec_recevents WHERE id=@0 AND calendarid = @1", id, calendar);
                if (e.locationId != 0)
                {
                    l = ApplicationContext.DatabaseContext.Database.Single<EventLocation>("SELECT * FROM ec_locations WHERE id = @0", e.locationId);
                }
                Schedule schedule = new Schedule(new Event()
                {
                    Title = e.title,
                    DaysOfWeekOptions = (DayOfWeekEnum)e.day,
                    FrequencyTypeOptions = (FrequencyTypeEnum)e.frequency,
                    MonthlyIntervalOptions = (MonthlyIntervalEnum)e.monthly_interval
                });

                //Fetch all Descriptions
                Dictionary<string, EventDesciption> descriptions = this._db.Query<EventDesciption>("SELECT * FROM ec_eventdescriptions WHERE eventid = @0 AND type = 1 AND calendarid = @1", id, calendar).ToDictionary(x => x.CultureCode);

                evm = new EventDetailsModel()
                {
                    Title = e.title,
                    LocationId = e.locationId,
                    Location = l,
                    StartDate = ((DateTime)schedule.NextOccurrence(DateTime.Now)).ToString("F", CultureInfo.CurrentCulture),
                    Descriptions = descriptions
                };
            }
            
            return PartialView("EventDetails",evm);
        }
    }
}