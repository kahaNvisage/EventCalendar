using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EventCalendar.Models
{
    [TableName("ec_calendars")]
    [PrimaryKey("id", autoIncrement = true)]
    [ExplicitColumns]
    public class ECalendar
    {
        [Column("id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        [HiddenInput(DisplayValue = false)]
        [Required]
        public int Id { get; set; }

        [Column("cname")]
        [Required]
        [Display(Name = "Calendar Name")]
        public string Calendarname { get; set; }

        [Column("gcal")]
        [Display(Name = "Use GCal?")]
        public bool IsGCal { get; set; }

        [Column("visible")]
        [Display(Name = "Show on site?")]
        public bool DisplayOnSite { get; set; }

        [Column("gcalfeed")]
        [Display(Name = "GCal Feed Url")]
        [StringLength(255)]
        public string GCalFeedUrl { get; set; }
    }
}