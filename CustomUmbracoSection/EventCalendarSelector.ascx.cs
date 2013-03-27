using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EventCalendar.Models;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace EventCalendar
{
    public partial class EventCalendarSelector : Umbraco.Web.UmbracoUserControl, umbraco.editorControls.userControlGrapper.IUsercontrolDataEditor
    {
        private List<ECalendar> Calendar;
        string selectedCalendar;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.DropDownList1.Items.Add("Not selected");
                this.Calendar = ApplicationContext.Current.DatabaseContext.Database.Query<ECalendar>("SELECT * FROM ec_calendars").ToList();

                foreach (ECalendar cal in this.Calendar)
                {
                    DropDownList1.Items.Add(new ListItem() { Text = cal.Calendarname, Value = cal.Id.ToString() });
                }

                try
                {
                    if (!String.IsNullOrEmpty(this.selectedCalendar.Trim()))
                    {
                        DropDownList1.SelectedValue = this.selectedCalendar;
                    }
                }
                catch (Exception ex) { }
            }
        }

        public object value
        {
            get
            {
                return this.selectedCalendar;
            }
            set
            {
                this.selectedCalendar = (string)value;
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.selectedCalendar = (sender as DropDownList).SelectedItem.Value.ToString();
        }
    }
}