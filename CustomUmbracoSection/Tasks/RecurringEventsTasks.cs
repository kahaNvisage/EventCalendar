using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.interfaces;
using EventCalendar.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core;

namespace EventCalendar.Tasks
{
    public class RecurringEventsTasks : umbraco.interfaces.ITaskReturnUrl, ITask
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
            var id = this._db.Insert(new RecurringEvent()
            {
                title = this.Alias,
                allDay = false,
                description = "",
                locationId = 0,
                calendarId = this.ParentID - 2,
                day = 0,
                frequency = 0,
                monthly_interval = 0
            });
            //int id = this._db.SingleOrDefault<RecurringEvent>("SELECT TOP 1 * FROM ec_rcevents ORDER BY id DESC").Id;

            m_returnUrl = string.Format("/EventCalendar/ECBackendSurface/EditRecurringEvent/?id={0}", Convert.ToInt32(id.ToString()));

            return true;
        }

        public bool Delete()
        {
            //Code that will execute when deleting
            return false;
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