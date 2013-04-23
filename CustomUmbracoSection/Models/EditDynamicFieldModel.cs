using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EventCalendar.Models
{
    public class EditDynamicFieldModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public int SelectedType { get; set; }

        [Required]
        [Display(Name = "Property name")]
        public string Name { get; set; }

        [HiddenInput]
        public int SelectedValueType { get; set; }

        [Display(Name = "Editor")]
        public SelectList ValueTypes { get; set; }

        public List<DynamicPropertyField> Fields { get; set; }
    }
}