using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Umbraco.Core.Persistence;
using EventCalendar.Models;

namespace EventCalendar
{
    public partial class installer : Umbraco.Web.UmbracoUserControl
    {
        private UmbracoDatabase _db = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            this._db = ApplicationContext.DatabaseContext.Database;

            this.CreateTable();
        }

        private void CreateTable()
        {
            if (this._db != null)
            {
                this.BulletedList1.Items.Add(new ListItem("Creating tables."));

                //Create Calendar table
                try
                {
                    this.BulletedList1.Items.Add(new ListItem("Creating calendar tables."));
                    if (!this._db.TableExist("ec_calendars"))
                    {
                        this._db.CreateTable<EventCalendar.Models.ECalendar>(false);
                        this.BulletedList1.Items.Add(new ListItem("Successfully created tables."));
                    }
                    else
                    {
                        this.BulletedList1.Items.Add(new ListItem("Database already exists. No changes have to be meda or no alter table script has been added"));
                    }
                }
                catch (Exception ex)
                {
                    this.BulletedList1.Items.Add(new ListItem(ex.Message));
                }

                //Create Events table
                try
                {
                    this.BulletedList1.Items.Add(new ListItem("Creating events tables."));
                    if (!this._db.TableExist("ec_events"))
                    {
                        this._db.CreateTable<CalendarEntry>(false);
                        this.BulletedList1.Items.Add(new ListItem("Successfully created tables."));
                    }
                    else
                    {
                        this.BulletedList1.Items.Add(new ListItem("Database already exists. No changes have to be meda or no alter table script has been added"));
                    }
                }
                catch (Exception ex)
                {
                    this.BulletedList1.Items.Add(new ListItem(ex.Message));
                }

                this.BulletedList1.Items.Add(new ListItem("Done creating tables."));
            }
            else
            {
                this.BulletedList1.Items.Add(new ListItem("Couldn't create necessary tables."));
            }
        }
    }
}