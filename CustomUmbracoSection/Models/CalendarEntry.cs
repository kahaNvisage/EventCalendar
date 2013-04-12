using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace EventCalendar.Models
{
    [TableName("ec_events")]
    [PrimaryKey("id", autoIncrement = true)]
    [ExplicitColumns]
    public class CalendarEntry
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

        [Column("starttime")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public DateTime? start { get; set; }

        [Column("endtime")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public DateTime? end { get; set; }

        [Column("allday")]
        public bool allDay { get; set; }

        [Column("description")]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string description { get; set; }
    }
}