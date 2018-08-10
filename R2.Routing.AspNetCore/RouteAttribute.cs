using System;
using EnsureThat;

namespace R2.Routing.AspNetCore
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RouteAttribute : Attribute
    {
        private string _prefix = string.Empty;
        private string _template;

        public RouteAttribute(string template)
        {
            Template = template;
        }

        public RouteAttribute(string prefix, string template)
        {
            Prefix = prefix;
            Template = template;
        }

        public string Prefix
        {
            get => _prefix;
            set
            {
                EnsureArg.IsNotNull(value, nameof(value));

                _prefix = value;
            }
        }

        public string Template
        {
            get => _template;
            set
            {
                EnsureArg.IsNotNullOrEmpty(value, nameof(value));

                _template = value;
            }
        }
    }
}