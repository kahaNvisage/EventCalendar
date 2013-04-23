using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Persistence;
using Umbraco.Core;
using EventCalendar.Models;

namespace EventCalendar
{
    public class EventModelBinder : IModelBinder
    {
        private UmbracoDatabase _db = null;
        public EventModelBinder()
        {
            this._db = ApplicationContext.Current.DatabaseContext.Database;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            EditEventModel eem = new EditEventModel();
            eem.allday = (bool)bindingContext.ValueProvider.GetValue("allday").ConvertTo(typeof(bool));
            eem.calendarId = (int)bindingContext.ValueProvider.GetValue("calendarid").ConvertTo(typeof(int));
            eem.end = (DateTime)bindingContext.ValueProvider.GetValue("end").ConvertTo(typeof(DateTime));
            eem.Id = (int)bindingContext.ValueProvider.GetValue("id").ConvertTo(typeof(int));
            eem.selectedLocation = (int)bindingContext.ValueProvider.GetValue("selectedlocation").ConvertTo(typeof(int));
            eem.start = (DateTime)bindingContext.ValueProvider.GetValue("start").ConvertTo(typeof(DateTime));
            eem.title = bindingContext.ValueProvider.GetValue("title").ConvertTo(typeof(string)).ToString();

            var custom_fields = this._db.Query<DynamicPropertyField>("SELECT * FROM ec_dynamicpropertyfields WHERE type = @0", DynamicPropertyType.Event).ToList();
            foreach (var field in custom_fields)
            {
                try
                {
                    if(null != bindingContext.ValueProvider.GetValue(field.Name))
                        eem.SetMember(field.Name, bindingContext.ValueProvider.GetValue(field.Name).ConvertTo(typeof(string)));
                }
                catch (Exception ex) { }
            }

            return eem;
        }
    }
}