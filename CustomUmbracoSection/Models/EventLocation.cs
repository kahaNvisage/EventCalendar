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
        [Display(Name = "Calendar Name")]
        public string LocationName { get; set; }

        [Column("adress")]
        [Required]
        [Display(Name = "Adress")]
        public string LocationAdress { get; set; }

        [Column("latitude")]
        public string lat { get; set; }

        [Column("longitude")]
        public string lon { get; set; }
    }
}