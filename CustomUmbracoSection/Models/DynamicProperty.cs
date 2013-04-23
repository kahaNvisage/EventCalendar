using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace EventCalendar.Models
{
    public enum DynamicPropertyType
    {
        Event = 1,
        Calendar = 2,
        Location = 3
    }

    public enum DynamicPropertyValueType
    {
        Textstring = 1,
        RichText = 2,
        Boolean = 3
    }

    [TableName("ec_dynamicpropertyfields")]
    [PrimaryKey("id", autoIncrement = true)]
    [ExplicitColumns]
    public class DynamicPropertyField
    {
        [Column("id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("type")]
        public int Type { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("valuetype")]
        public int ValueType { get; set; }
    }

    [TableName("ec_dynamicproperties")]
    [PrimaryKey("id", autoIncrement = true)]
    [ExplicitColumns]
    public class DynamicProperty
    {
        [Column("id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("fieldid")]
        public int FieldId { get; set; }

        [Column("owner")]
        public int Owner { get; set; }

        [Column("value")]
        public string Value { get; set; }

        [ResultColumn]
        public string Name { get; set; }

        [ResultColumn]
        public int ValueType { get; set; }
    }
}