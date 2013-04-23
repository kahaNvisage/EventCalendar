using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using System.Web.Mvc;
using EventCalendar.Models;

namespace EventCalendar
{
    public class AppStart : IApplicationEventHandler
    {

        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {}

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ModelBinders.Binders.Add(typeof(EditEventModel), new EventModelBinder());
        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {}
    }
}