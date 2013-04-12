using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;
using ScheduleWidget.Enums;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EventCalendar.Models
{
    [TableName("ec_recevents")]
    [PrimaryKey("id", autoIncrement = true)]
    [ExplicitColumns]
    public class RecurringEvent
    {
        [Column("id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("calendarid")]
        public int calendarId { get; set; }

        [Column("locationId")]
        public int locationId { get; set; }

        [Column("title")]
        public string title { get; set; }

        [Column("allday")]
        public bool allDay { get; set; }

        [Column("description")]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string description { get; set; }

        [Column("day")]
        public int day { get; set; }

        [Column("frequency")]
        public int frequency { get; set; }

        [Column("monthly")]
        public int monthly_interval { get; set; }
    }

    public class RecurringEventListModel
    {
        public int Id { get; set; }
        public string title { get; set; }
        public bool allDay { get; set; }
        public DayOfWeekEnum day { get; set; }
        public FrequencyTypeEnum frequency { get; set; }
    }

    public class EditRecurringEventModel
    {
        [HiddenInput]
        public int id { get; set; }

        [Display(Name = "Title")]
        public string title { get; set; }

        [Display(Name = "Is all day?")]
        public bool allDay { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.Html)]
        [AllowHtml]
        public string description { get; set; }

        [Display(Name = "Day of the event")]
        public DayOfWeekEnum day { get; set; }

        [Display(Name = "Time period")]
        public FrequencyTypeEnum frequency { get; set; }

        [Display(Name = "Monthly period")]
        public MonthlyIntervalEnum monthly { get; set; }

        [HiddenInput]
        public int selectedLocation { get; set; }

        [Display(Name = "Location")]
        public SelectList locations { get; set; }

        [HiddenInput]
        public int calendar { get; set; }
    }
}