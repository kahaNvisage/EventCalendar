using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.uicontrols;
using umbraco;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using EventCalendar.Models;

namespace EventCalendar
{
    public partial class editCustom : System.Web.UI.Page
    {
        public TabPage FirstTab;
        public TabPage SecondTab;
        public TabPage ThirdTab;

        private EventCalendar.Models.ECalendar _calendar;
        private UmbracoDatabase _db = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = Request["id"];

                // Load initial values
                _calendar = this._db.SingleOrDefault<EventCalendar.Models.ECalendar>("SELECT * FROM ec_calendars WHERE ID=@0", id);

                CalendarName.Text = _calendar.Calendarname;
                DisplayOnSite.Checked = _calendar.DisplayOnSite;
                IsGCal.Checked = _calendar.IsGCal;

                List<CalendarEntry> entrys = (List<CalendarEntry>)this._db.Query<CalendarEntry>("SELECT * FROM ec_events WHERE calendarId = @0", id);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this._db = ApplicationContext.Current.DatabaseContext.Database;

            // Tab setup
            FirstTab = tabControl.NewTabPage("First");
            FirstTab.Controls.Add(SettingsPane);
           

            SecondTab = tabControl.NewTabPage("Second");
            SecondTab.Controls.Add(CreateEventPane);

            ThirdTab = tabControl.NewTabPage("Third");
            ThirdTab.Controls.Add(ShowEventsPane);


            //save button
            ImageButton save = FirstTab.Menu.NewImageButton();
            save.Click += new ImageClickEventHandler(this.save_click);
            save.AlternateText = "Save";
            save.ImageUrl = GlobalSettings.Path + "/images/editor/save.gif";



            ImageButton save2 = SecondTab.Menu.NewImageButton();
            save2.Click += new ImageClickEventHandler(this.save_event_click);
            save2.AlternateText = "Save";
            save2.ImageUrl = GlobalSettings.Path + "/images/editor/save.gif";
           
        }

        //Save Calendar Settings
        private void save_click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            // Save changes
        }

        //Save Event
        private void save_event_click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            // Save changes
        }
    }
}
