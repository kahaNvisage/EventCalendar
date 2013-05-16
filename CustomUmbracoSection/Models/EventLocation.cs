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
    [TableName("ec_locations")]
    [PrimaryKey("id", autoIncrement = true)]
    [ExplicitColumns]
    public class EventLocation
    {
        [Column("id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        [HiddenInput(DisplayValue = false)]
        [Required]
        public int Id { get; set; }

        [Column("lname")]
        [Required]
        [Display(Name = "Location Name")]
        public string LocationName { get; set; }

        [Column("street")]
        [Display(Name = "Street")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Street { get; set; }

        [Column("zip")]
        [Display(Name = "Zipcode")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string ZipCode { get; set; }

        [Column("city")]
        [Display(Name = "City")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string City { get; set; }

        [Column("country")]
        [Display(Name = "Country")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Country { get; set; }

        [Column("latitude")]
        [Required]
        public string lat { get; set; }

        [Column("longitude")]
        [Required]
        public string lon { get; set; }
    }
}