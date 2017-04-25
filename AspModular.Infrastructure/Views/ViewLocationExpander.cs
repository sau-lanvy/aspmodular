using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspModular.Infrastructure.Views
{
    public class ViewLocationExpander : IViewLocationExpander
    {
        private const string _moduleKey = "module";
        private const string _modulePath = "/Modules/";
        private const string _prefixModule = "AspModular.Module.";

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {

            //var controllerActionDescriptor = (context.ActionContext.ActionDescriptor);
            //if (controllerActionDescriptor != null && controllerActionDescriptor.DisplayName.Contains("AspModular"))
            //    return new List<string> { GetFeatureLocation(controllerActionDescriptor.DisplayName) };

            if (context.Values.ContainsKey(_moduleKey))
            {
                var moduleName = context.Values[_moduleKey];
                if (!string.IsNullOrWhiteSpace(moduleName))
                {
                    moduleName = string.Format("{0}{1}", _prefixModule, moduleName);
                    var moduleViewLocations = new string[]
                    {
                        string.Format("{0}{1}{2}", _modulePath, moduleName, "/Views/{1}/{0}.cshtml"),
                        string.Format("{0}{1}{2}", _modulePath, moduleName, "/Views/Shared/{0}.cshtml")
                    };

                    viewLocations = moduleViewLocations.Concat(viewLocations);
                }
            }
            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var controller = context.ActionContext.ActionDescriptor.DisplayName;
            var moduleName = controller.Split('.')[2];
            context.Values[_moduleKey] = moduleName;
        }

        private string GetFeatureLocation(string fullControllerName)
        {
            var words = fullControllerName.Split('.');
            var path = "\\Modules\\AspModular.Module.Dashboard";
            //bool isInFeature = false;
            
            return System.IO.Path.Combine(path, "views","home", "{0}.cshtml");
        }
    }
}
