using System.Collections.Generic;
using System.Linq;
using Arch.Service;
using Arch.Service.Interfaces;
using Arch.Service.Services;
using Arch.Core;
using System.Dynamic;

namespace System.Web.Mvc
{
    public static class Resources
    {
        private static List<Parameters> _parameters;
        public static List<LookupList> _lookupLists;
        private static IUtilityService _utilityService
        {
            get
            {
                return DependencyResolver.Current.GetService<UtilityService>();
            }
        }
        public static dynamic Parameter { get; set; }
        public static void LoadParameters()
        {
            var expando = new ExpandoObject();
            var expandoDic = (IDictionary<string, object>)expando;
            _parameters = _utilityService.GetParameters();
            foreach (var property in _parameters)
            {
                expandoDic.Add(property.Code, property.Value);
            }
            Parameter = expandoDic;
        }
        public static void LoadLookupLists()
        {
            _lookupLists = _utilityService.GetLookupLists();
        }
        public static void Load()
        {
            LoadLookupLists();
            LoadParameters();
        }
    }
}