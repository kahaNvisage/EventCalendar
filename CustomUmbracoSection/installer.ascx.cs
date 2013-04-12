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
                    this.BulletedList1.Items.Add(new ListItem("Creating events table."));
                    if (!this._db.TableExist("ec_events"))
                    {
                        this._db.CreateTable<CalendarEntry>(false);
                        this.BulletedList1.Items.Add(new ListItem("Successfully created table."));
                    }
                    else
                    {
                        int i = this._db.Execute("ALTER TABLE ec_events ALTER COLUMN description ntext", new { });
                        this.BulletedList1.Items.Add(new ListItem("Database already exists. Altered some fields."));
                    }
                }
                catch (Exception ex)
                {
                    this.BulletedList1.Items.Add(new ListItem(ex.Message));
                }

                //Create Recurring Events Table
                try
                {
                    this.BulletedList1.Items.Add(new ListItem("Creating recurring events table."));
                    if (!this._db.TableExist("ec_recevents"))
                    {
                        this._db.CreateTable<RecurringEvent>(false);
                        this.BulletedList1.Items.Add(new ListItem("Successfully created table."));
                    }
                    else
                    {
                        this.BulletedList1.Items.Add(new ListItem("Database already exists. No changes have to be meda or no alter table script has been added"));
                    }
                }
                catch (Exception ex) { }

                //Create Locations table
                try
                {
                    this.BulletedList1.Items.Add(new ListItem("Creating locations table."));
                    if (!this._db.TableExist("ec_locations"))
                    {
                        this._db.CreateTable<EventLocation>(false);
                        this.BulletedList1.Items.Add(new ListItem("Successfully created table."));
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