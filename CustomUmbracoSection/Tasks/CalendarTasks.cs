using System;
using System.Collections.Generic;
using System.Web;
using umbraco.interfaces;
using Umbraco.Core.Persistence;
using Umbraco.Core;
using EventCalendar.Models;

namespace EventCalendar.Tasks
{
    public class CalendarTasks : umbraco.interfaces.ITaskReturnUrl, ITask
    {
        private string _alias;
        private int _parentID;
        private int _typeID;
        private int _userID;

        private UmbracoDatabase _db = ApplicationContext.Current.DatabaseContext.Database;

        public int UserId
        {
            set { _userID = value; }
        }

        public int TypeID
        {
            set { _typeID = value; }
            get { return _typeID; }
        }

        public int ParentID
        {
            set { _parentID = value; }
            get { return _parentID; }
        }

        public string Alias
        {
            get
            {
                return _alias;
            }
            set
            {
                _alias = value;
            }
        }

        public bool Save()
        {
                        
            //Code that will execute on creation
            this._db.Insert(new ECalendar()
            {
                Calendarname = Alias,
                DisplayOnSite = true,
                IsGCal = false
            });
            int id = this._db.SingleOrDefault<ECalendar>("SELECT TOP 1 * FROM ec_calendars ORDER BY id DESC").Id;

            m_returnUrl = string.Format("Plugins/EventCalendar/editEventCalendar.aspx?id={0}", id);

            return true;
        }

        public bool Delete()
        {
            //Code that will execute when deleting
            ECalendar c = this._db.SingleOrDefault<ECalendar>(ParentID);
            List<CalendarEntry> events = (List<CalendarEntry>)this._db.Query<CalendarEntry>("SELECT * FROM ec_events WHERE calendarId = @0", c.Id);
            foreach (CalendarEntry e in events)
            {
                this._db.Delete(e);
            }
            this._db.Delete(c);
            return true;
        }

        public CalendarTasks()
        {

        }

        #region ITaskReturnUrl Members
        private string m_returnUrl = "";
        public string ReturnUrl
        {
            get { return m_returnUrl; }
        }

        #endregion
    }
}
