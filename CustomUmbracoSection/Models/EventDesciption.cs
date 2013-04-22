using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EventCalendar.Models
{
    [TableName("ec_eventdescriptions")]
    [PrimaryKey("id", autoIncrement = true)]
    [ExplicitColumns]
    public class EventDesciption
    {
        [Column("id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        [HiddenInput(DisplayValue = false)]
        [Required]
        public int Id { get; set; }

        [Column("eventid")]
        [HiddenInput(DisplayValue = false)]
        [Required]
        public int EventId { get; set; }

        [Column("culture")]
        [StringLength(5)]
        [Required]
        public string CultureCode { get; set; }

        [Column("content")]
        [NullSetting(NullSetting = NullSettings.Null)]
        [AllowHtml]
        public string Content { get; set; }
    }
}