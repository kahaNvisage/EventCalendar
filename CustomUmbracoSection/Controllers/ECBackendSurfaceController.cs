using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web;
using System.Web.Mvc;
using EventCalendar.Models;
using Umbraco.Core.Persistence;
using Newtonsoft.Json;

namespace EventCalendar.Controllers
{
    [PluginController("EventCalendar")]
    public class ECBackendSurfaceController : SurfaceController
    {
        private UmbracoDatabase _db;
        public ECBackendSurfaceController() {
            this._db = ApplicationContext.DatabaseContext.Database;
        }

        public ECBackendSurfaceController(UmbracoContext context) : base(context) {
            this._db = context.Application.DatabaseContext.Database;
        }

        public ActionResult Index(int id = 0)
        {
            ECalendar cal = null;
            if (0 != id && null != this._db)
            {
                cal = this._db.SingleOrDefault<ECalendar>(id);
            }
            ViewData["calendar"] = cal;
            return View();
        }

        [HttpPost]
        public ActionResult EditCalendar(ECalendar calendar)
        {
            if (!ModelState.IsValid)
            {
                TempData["StatusSettings"] = "Invalid";
                return RedirectToAction("Index", new { id = calendar.Id });
                //ViewData["calendar"] = calendar;
                //return PartialView("Index");
            }

            TempData["StatusSettings"] = "Valid";
            this._db.Update(calendar);
            //ViewData["calendar"] = calendar;
            //return PartialView("Index");
            return RedirectToAction("Index", new { id = calendar.Id });
        }

        [ChildActionOnly]
        public ActionResult ShowEvents(int id)
        {
            List<CalendarEntry> events = null;
            events = this._db.Query<CalendarEntry>("SELECT * FROM ec_events").ToList();

            return PartialView(events);
        }

        [HttpGet]
        public ActionResult AddEvent(int id)
        {
            return PartialView(new AddEventModel() { calendarId = id });
        }

        [HttpPost]
        public ActionResult AddEvent(AddEventModel new_event)
        {
            ECalendar cal = null;
            if (0 != new_event.calendarId && null != this._db)
            {
                cal = this._db.SingleOrDefault<ECalendar>(new_event.calendarId);
            }
            if (!ModelState.IsValid)
            {
                TempData["StatusNewEvent"] = "Invalid";
                return RedirectToAction("Index", new { id = cal.Id });

                //ViewData["calendar"] = cal;
                //return PartialView("Index");
            }
            TempData["StatusNewEvent"] = "Valid";
            CalendarEntry entry = new CalendarEntry()
                {
                    allDay = new_event.allday,
                    calendarId = new_event.calendarId,
                    description = new_event.description,
                    title = new_event.title,
                    start = new_event.start,
                    lat = new_event.lat,
                    lon = new_event.lon
                };
            if (null != new_event.end)
            {
                entry.end = new_event.end;
            }
            this._db.Insert(entry);

            //ViewData["calendar"] = cal;
            //return PartialView("Index");
            return RedirectToAction("Index", new { id = cal.Id });
        }

        [HttpGet]
        public ActionResult EditEventForm(int id = 0)
        {
            CalendarEntry entry = this._db.SingleOrDefault<CalendarEntry>(id);
            EditEventModel eem = new EditEventModel()
            {
                Id = entry.Id,
                title = entry.title,
                start = (DateTime)entry.start,
                description = entry.description,
                calendarId = entry.calendarId,
                allday = entry.allDay,
                lat = entry.lat,
                lon = entry.lon
            };
            if (null != entry.end)
            {
                eem.end = (DateTime)entry.end;
            }
            return PartialView("EditEventForm",eem);
        }

        [HttpPost]
        public ActionResult EditEventForm(EditEventModel e)
        {
            if (!ModelState.IsValid)
            {
                TempData["StatusEditEvent"] = "Invalid";
                return PartialView("EditEventForm");
            }
            TempData["StatusEditEvent"] = "Valid";
            CalendarEntry entry = new CalendarEntry()
            {
                allDay = e.allday,
                calendarId = e.calendarId,
                description = e.description,
                title = e.title,
                end = e.end,
                start = e.start,
                Id = e.Id,
                lat = e.lat,
                lon = e.lon
            };
            this._db.Update(entry);
            return PartialView("EditEventForm");
        }

        [HttpPost]
        public string GetCalendarEventsAsJson(int id)
        {
            List<CalendarEntry> events = this._db.Query<CalendarEntry>("SELECT * FROM ec_events WHERE calendarId = @0",id).ToList();
            string json = JsonConvert.SerializeObject(events);
            return json;
        }
    }
}