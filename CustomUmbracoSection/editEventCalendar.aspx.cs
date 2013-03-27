using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.uicontrols;
using Umbraco.Core.Persistence;
using Umbraco.Core;
using umbraco;
using EventCalendar.Models;
using umbraco.BasePages;

namespace EventCalendar
{
    public partial class editEventCalendar : BasePage
    {
        public TabPage FirstTab;
        public TabPage SecondTab;
        public TabPage ThirdTab;

        private EventCalendar.Models.ECalendar _calendar;
        private UmbracoDatabase _db = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request["id"];

            // Load initial values
            _calendar = this._db.SingleOrDefault<EventCalendar.Models.ECalendar>("SELECT * FROM ec_calendars WHERE ID=@0", id);

            CalendarName.Text = _calendar.Calendarname;
            DisplayOnSite.Checked = _calendar.DisplayOnSite;
            IsGCal.Checked = _calendar.IsGCal;

            //List<CalendarEntry> entrys = (List<CalendarEntry>)this._db.Query<CalendarEntry>("SELECT * FROM ec_events WHERE calendarId = @0", id);
        }

        protected override void OnInit(EventArgs e)
        {
            this._db = ApplicationContext.Current.DatabaseContext.Database;

            // Tab setup
            FirstTab = TabView1.NewTabPage("Settings");
            FirstTab.Controls.Add(SettingsPane);

            //save button
            ImageButton save = FirstTab.Menu.NewImageButton();
            save.Click += new ImageClickEventHandler(this.save_click);
            save.AlternateText = "Save";
            save.ImageUrl = GlobalSettings.Path + "/images/editor/save.gif";
        }

        //Save Calendar Settings
        private void save_click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            // Save changes
            //int id = int.Parse(Request["id"]);
            
            //EventCalendar.Models.Calendar tmp = new EventCalendar.Models.Calendar()
            //{
            //    Id = id,
            //    Calendarname = CalendarName.Text,
            //    DisplayOnSite = DisplayOnSite.Checked,
            //    IsGCal = IsGCal.Checked
            //};
            this._calendar.Calendarname = CalendarName.Text.Trim();
            this._calendar.DisplayOnSite = DisplayOnSite.Checked;
            this._calendar.IsGCal = IsGCal.Checked;
            this._db.Update(_calendar);

            this.ClientTools.ShowSpeechBubble(umbraco.BasePages.BasePage.speechBubbleIcon.save, "Save complete",
                                                   "Calendar has been updated.");
            this.ClientTools.RefreshTree("initeventCalendarTree");
        }

        //Save Event
        private void save_event_click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            // Save changes
        }
    }
}