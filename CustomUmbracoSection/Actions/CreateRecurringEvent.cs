using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.interfaces;

namespace EventCalendar.Actions
{
    public class CreateRecurringEvent
    {
        public string Alias
        {
            get { return "create_recurring_event"; }
        }

        public bool CanBePermissionAssigned
        {
            get { return false; }
        }

        public string Icon
        {
            get { return ".sprNew"; }
        }

        public string JsFunctionName
        {
            get { return "CreateRecurringEvent()"; }
        }

        public string JsSource
        {
            get { return @"function CreateRecurringEvent() {}
                    
"; }
        }

        public char Letter
        {
            get { throw new NotImplementedException(); }
        }

        public bool ShowInNotifier
        {
            get { return false; }
        }
    }
}