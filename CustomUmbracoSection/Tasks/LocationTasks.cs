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
    public class LocationTasks : umbraco.interfaces.ITaskReturnUrl, ITask
    {
        private string _alias;
        private int _parentID;
        private int _typeID;
        private int _userID;

        private UmbracoDatabase _db = ApplicationContext.Current.DatabaseContext.Database;

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

        public bool Delete()
        {
            //Code that will execute when deleting
            EventLocation c = this._db.SingleOrDefault<EventLocation>(ParentID);            
            this._db.Delete(c);
            return true;
        }

        public int ParentID
        {
            set { _parentID = value; }
            get { return _parentID; }
        }

        public bool Save()
        {
            //Code that will execute on creation
            this._db.Insert(new EventLocation()
            {
                LocationName = Alias,
                lat = "",
                lon = "",
                LocationAdress = ""
            });
            int id = this._db.SingleOrDefault<EventLocation>("SELECT TOP 1 * FROM ec_locations ORDER BY id DESC").Id;

            //m_returnUrl = string.Format("Plugins/EventCalendar/editEventCalendar.aspx?id={0}", id);
            m_returnUrl = string.Format("/EventCalendar/ECBackendSurface/EditLocation/?id={0}", id);

            return true;
        }

        public int TypeID
        {
            set { _typeID = value; }
            get { return _typeID; }
        }

        public int UserId
        {
            set { _userID = value; }
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