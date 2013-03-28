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
            List<EventLocation> locations = this._db.Query<EventLocation>("SELECT * FROM ec_locations").ToList();
            locations.Insert(0, new EventLocation() { LocationName = "No Location", Id = 0 });
            SelectList list = new SelectList(locations, "Id", "LocationName");
            return PartialView(new AddEventModel() { calendarId = id, locations = list });
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
                    locationId = new_event.selectedLocation
                };
            if (new_event.start > new_event.end)
            {
                entry.end = null;
            }
            else
            {
                entry.end = new_event.end;
            }

            this._db.Insert(entry);

            return RedirectToAction("Index", new { id = cal.Id });
        }

        [HttpGet]
        public ActionResult EditEventForm(int id = 0)
        {
            List<EventLocation> locations = this._db.Query<EventLocation>("SELECT * FROM ec_locations").ToList();
            locations.Insert(0, new EventLocation() { LocationName = "No Location", Id = 0 });
            
            //Get the Event from Database
            CalendarEntry entry = this._db.SingleOrDefault<CalendarEntry>(id);
            //Create SelectList with selected location
            SelectList list = new SelectList(locations, "Id", "LocationName", entry.locationId);
            //Create Model for the View
            EditEventModel eem = new EditEventModel()
            {
                Id = entry.Id,
                title = entry.title,
                start = (DateTime)entry.start,
                description = entry.description,
                calendarId = entry.calendarId,
                allday = entry.allDay,
                locations = list
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
            List<EventLocation> locations = this._db.Query<EventLocation>("SELECT * FROM ec_locations").ToList();
            locations.Insert(0, new EventLocation() { LocationName = "No Location", Id = 0 });
            SelectList list = new SelectList(locations, "Id", "LocationName", e.selectedLocation);
            e.locations = list;
            if (!ModelState.IsValid)
            {
                TempData["StatusEditEvent"] = "Invalid";
                return PartialView("EditEventForm",e);
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
                locationId = e.selectedLocation
            };
            this._db.Update(entry);
            return PartialView("EditEventForm",e);
        }

        [HttpPost]
        public string GetCalendarEventsAsJson(int id)
        {
            List<CalendarEntry> events = this._db.Query<CalendarEntry>("SELECT * FROM ec_events WHERE calendarId = @0",id).ToList();
            string json = JsonConvert.SerializeObject(events);
            return json;
        }

        [HttpGet]
        public ActionResult EditLocation(int id)
        {
            EventLocation el = this._db.SingleOrDefault<EventLocation>("SELECT * FROM ec_locations WHERE Id = @0", id);
            return View(el);
        }

        [HttpPost]
        public ActionResult EditLocation(EventLocation el)
        {
            if (!ModelState.IsValid)
            {
                TempData["StatusEditLocation"] = "Invalid";
                return View(el);
            }
            TempData["StatusEditLocation"] = "Valid";
            this._db.Update(el);
            return View(el);
        }
    }
}