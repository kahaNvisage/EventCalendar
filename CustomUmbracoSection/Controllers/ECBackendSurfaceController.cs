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
using ScheduleWidget.Enums;
using ScheduleWidget.ScheduledEvents;
using Umbraco.Core.Models;

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
            //ViewData["calendar"] = cal;
            return View(cal);
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

        [HttpGet]
        public ActionResult ShowEvents(int id)
        {
            List<CalendarEntry> events = null;
            events = this._db.Query<CalendarEntry>("SELECT * FROM ec_events ORDER BY id DESC").Where(x => x.calendarId == id).ToList();

            return PartialView(events);
        }

        [HttpGet]
        public ActionResult ShowRecurringEvents(int id)
        {
            List<RecurringEventListModel> events = new List<RecurringEventListModel>();
            var tmp = this._db.Query<RecurringEvent>("SELECT * FROM ec_recevents ORDER BY id DESC").Where(x => x.calendarId == id).ToList();
            foreach (var t in tmp)
            {
                events.Add(new RecurringEventListModel()
                {
                    title = t.title,
                    allDay = t.allDay,
                    day = (DayOfWeekEnum)t.day,
                    frequency = (FrequencyTypeEnum)t.frequency,
                    Id = t.Id
                });
            }
            return PartialView(events);
        }

        [HttpGet]
        [Obsolete("Was used before using Tasks to create Events")]
        public ActionResult AddEvent(int id)
        {
            List<EventLocation> locations = this._db.Query<EventLocation>("SELECT * FROM ec_locations").ToList();
            locations.Insert(0, new EventLocation() { LocationName = "No Location", Id = 0 });
            SelectList list = new SelectList(locations, "Id", "LocationName");
            return PartialView(new AddEventModel() { calendarId = id, locations = list });
        }

        [HttpPost]
        [Obsolete("Was used before using Tasks to create Events")]
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
            //Get Descriptions for Event
            Dictionary<string, EventDesciption> descriptions = this._db.Query<EventDesciption>("SELECT * FROM ec_eventdescriptions WHERE eventid = @0",id).ToDictionary(x => x.CultureCode);
            List<ILanguage> languages = Services.LocalizationService.GetAllLanguages().ToList();
            foreach (var lang in languages)
            {
                if(!descriptions.ContainsKey(lang.CultureInfo.ToString())){
                    descriptions.Add(lang.CultureInfo.ToString(), new EventDesciption() { CultureCode = lang.CultureInfo.ToString(), EventId = eem.Id, Id = 0 });
                }
            }

            eem.Descriptions = descriptions;
            
            return PartialView("EditEventForm",eem);
        }

        [HttpPost]
        public ActionResult EditEventForm(EditEventModel e)
        {
            //Get locations
            List<EventLocation> locations = this._db.Query<EventLocation>("SELECT * FROM ec_locations").ToList();
            locations.Insert(0, new EventLocation() { LocationName = "No Location", Id = 0 });
            SelectList list = new SelectList(locations, "Id", "LocationName", e.selectedLocation);
            e.locations = list;

            //Get Descriptions for Event
            Dictionary<string, EventDesciption> descriptions = this._db.Query<EventDesciption>("SELECT * FROM ec_eventdescriptions WHERE eventid = @0", e.Id).ToDictionary(x => x.CultureCode);
            List<ILanguage> languages = Services.LocalizationService.GetAllLanguages().ToList();
            foreach (var lang in languages)
            {
                if (!descriptions.ContainsKey(lang.CultureInfo.ToString()))
                {
                    descriptions.Add(lang.CultureInfo.ToString(), new EventDesciption() { CultureCode = lang.CultureInfo.ToString(), EventId = e.Id, Id = 0 });
                }
            }

            e.Descriptions = descriptions;

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

        public ActionResult EditRecurringEvent(int id = 0)
        {
            List<EventLocation> locations = this._db.Query<EventLocation>("SELECT * FROM ec_locations").ToList();
            locations.Insert(0, new EventLocation() { LocationName = "No Location", Id = 0 });
            
            var e = _db.Single<RecurringEvent>(id);

            //Create SelectList with selected location
            SelectList list = new SelectList(locations, "Id", "LocationName", e.locationId);

            EditRecurringEventModel ere = new EditRecurringEventModel(){
                title = e.title,
                description = e.description,
                allDay = e.allDay,
                id = e.Id,
                day = (DayOfWeekEnum)e.day,
                frequency = (FrequencyTypeEnum)e.frequency,
                monthly = (MonthlyIntervalEnum)e.monthly_interval,
                calendar = e.calendarId,
                locations = list,
                selectedLocation = e.locationId
            };

            return PartialView(ere);
        }

        [HttpPost]
        public ActionResult EditRecurringEvent(EditRecurringEventModel ere)
        {
            List<EventLocation> locations = this._db.Query<EventLocation>("SELECT * FROM ec_locations").ToList();
            locations.Insert(0, new EventLocation() { LocationName = "No Location", Id = 0 });
            SelectList list = new SelectList(locations, "Id", "LocationName", ere.selectedLocation);
            ere.locations = list;

            if (!ModelState.IsValid)
            {
                TempData["StatusEditEvent"] = "Invalid";
                return PartialView(ere);
            }

            TempData["StatusEditEvent"] = "Valid";

            RecurringEvent re = new RecurringEvent()
            {
                Id = ere.id,
                title = ere.title,
                allDay = ere.allDay,
                calendarId = ere.calendar,
                locationId = ere.selectedLocation,
                description = ere.description,
                day = (int)ere.day,
                frequency = (int)ere.frequency,
                monthly_interval = (int)ere.monthly
            };

            _db.Update(re, re.Id);

            return PartialView(ere);
        }

        [HttpPost]
        public string GetCalendarEventsAsJson(int id = 0, int start = 0, int end = 0)
        {
            List<EventsOverviewModel> events = new List<EventsOverviewModel>();
            //List<Schedule> schedules = new List<Schedule>();

            if (id != 0)
            {
                events.AddRange(this.GetNormalEvents(id));
                events.AddRange(this.GetRecurringEvents(id, start, end));
            }
            else
            {
                var calendar = this._db.Query<ECalendar>("SELECT * FROM ec_calendars").ToList();
                foreach (var cal in calendar)
                {
                    events.AddRange(this.GetNormalEvents(cal.Id));
                    events.AddRange(this.GetRecurringEvents(cal.Id, start, end));
                }
            }
            

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

        [HttpGet]
        public ActionResult DeleteEvent(int id)
        {
            var eve = this._db.Single<CalendarEntry>(id);
            return PartialView(eve);
        }

        [HttpPost]
        public ActionResult DeleteEvent(CalendarEntry entry)
        {
            this._db.Delete<CalendarEntry>(entry.Id);
            return RedirectToAction("ShowEvents", new { id = entry.calendarId });
        }

        [HttpGet]
        public ActionResult DeleteRecurringEvent(int id)
        {
            var eve = this._db.Single<RecurringEvent>(id);
            return PartialView(eve);
        }

        [HttpPost]
        public ActionResult DeleteRecurringEvent(RecurringEvent e)
        {
            this._db.Delete<RecurringEvent>(e.Id);
            return RedirectToAction("ShowRecurringEvents", new { id = e.calendarId });
        }

        [HttpPost]
        public ActionResult SaveEventDescription(EditDescriptionModel edm)
        {
            EventDesciption ed = new EventDesciption()
            {
                Content = edm.content,
                CultureCode = edm.culture,
                EventId = edm.eventid
            };
            if (edm.id != 0)
            {
                ed.Id = edm.id;
                this._db.Update(ed);
            }
            else
            {
                this._db.Insert(ed);
            }
            
            return RedirectToAction("EditEventForm", new { id = edm.eventid });
        }

        private List<EventsOverviewModel> GetNormalEvents(int id)
        {
            //Handle normal events
            List<EventsOverviewModel> events = new List<EventsOverviewModel>();
            var calendar = this._db.SingleOrDefault<ECalendar>(id);
            var normal_events = this._db.Query<CalendarEntry>("SELECT * FROM ec_events WHERE calendarId = @0", id).ToList();
            foreach (var ne in normal_events)
            {
                events.Add(
                    new EventsOverviewModel()
                    {
                        type = EventType.Normal,
                        title = ne.title,
                        allDay = ne.allDay,
                        description = ne.description,
                        end = ne.end,
                        start = ne.start,
                        id = ne.Id,
                        color = !String.IsNullOrEmpty(calendar.Color) ? calendar.Color : ""
                    });
            }
            return events;
        }

        private List<EventsOverviewModel> GetRecurringEvents(int id, int start = 0, int end = 0)
        {
            //Handle recurring events
            List<EventsOverviewModel> events = new List<EventsOverviewModel>();

            DateTime startDate = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            startDate = startDate.AddSeconds(start);
            DateTime endDate = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            endDate = endDate.AddSeconds(end);

            DateRange range = new DateRange();
            range.StartDateTime = startDate;
            range.EndDateTime = endDate;

            var calendar = this._db.SingleOrDefault<ECalendar>(id);
            var recurring_events = this._db.Query<RecurringEvent>("SELECT * FROM ec_recevents WHERE calendarId = @0 ORDER BY id DESC", id).ToList();
            foreach(var e in recurring_events) {
                var schedule = new Schedule(
                    new Event()
                    {
                        Title = e.title,
                        ID = e.Id,
                        DaysOfWeekOptions = (DayOfWeekEnum)e.day,
                        FrequencyTypeOptions = (FrequencyTypeEnum)e.frequency,
                        MonthlyIntervalOptions = (MonthlyIntervalEnum)e.monthly_interval
                    });
                foreach (var tmp in schedule.Occurrences(range))
                {
                    events.Add(new EventsOverviewModel()
                    {
                        title = e.title,
                        id = e.Id,
                        allDay = e.allDay,
                        description = e.description,
                        start = tmp,
                        type = EventType.Recurring,
                        color = !String.IsNullOrEmpty(calendar.Color) ? calendar.Color : ""
                    });
                }
            }
            return events;
        }
    }
}