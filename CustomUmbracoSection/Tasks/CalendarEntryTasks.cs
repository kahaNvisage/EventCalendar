using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.interfaces;
using Umbraco.Core.Persistence;
using Umbraco.Core;
using EventCalendar.Models;

namespace EventCalendar.Tasks
{
    public class CalendarEntryTasks : umbraco.interfaces.ITaskReturnUrl, ITask
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
            this._db.Insert(new CalendarEntry()
            {
                title = this.Alias,
                allDay = false,
                start = DateTime.Now,
                end = null,
                description = "",
                locationId = 0,
                calendarId = this.ParentID
            });
            int id = this._db.SingleOrDefault<CalendarEntry>("SELECT TOP 1 * FROM ec_events ORDER BY id DESC").Id;

            m_returnUrl = string.Format("/EventCalendar/ECBackendSurface/EditEventForm?id={0}", id);

            return true;
        }

        public bool Delete()
        {
            //Code that will execute when deleting
            return false;
        }

        public CalendarEntryTasks()
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